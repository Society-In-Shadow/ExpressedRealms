using ExpressedRealms.Characters.Reports.CRB.CrbPages;
using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using QuestPDF;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Characters.Reports.CRB;


public static class CharacterReferenceBookletReport
{
    public static MemoryStream GenerateReport(ReportData data)
    {
        Settings.License = LicenseType.Community;
        GlobalFontSettings.FontResolver ??= new MultiFontResolver();

        return MergeAllFields(data);
    }

    private static MemoryStream MergeAllFields(ReportData data)
    {
        var pdfPath = Path.Combine(AppContext.BaseDirectory, "overallCRB.pdf");
        using var document = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Modify);

        for (int i = 0; i < document.Pages.Count; i++)
        {
            using var page = XGraphics.FromPdfPage(document.Pages[i]);

            var centerX = XUnitPt.FromInch(3.5);
            if (i % 2 == 1)
            {
                centerX = XUnitPt.FromInch(5);
            }

            TextPrintUtilities.Print90DegreeMessage(
                page,
                $"{data.BasicInfo.EventName} - {DateTime.Now:MMM dd, yyyy}",
                centerX,
                XUnitPt.FromInch(7.5),
                XBrushes.DimGray
            );

            // Add Staple Markers
            TextPrintUtilities.Print90DegreeMessage(
                page,
                "— —",
                centerX,
                XUnitPt.FromInch(9.5),
                XBrushes.DimGray
            );
            TextPrintUtilities.Print90DegreeMessage(
                page,
                "— —",
                centerX,
                XUnitPt.FromInch(5.5),
                XBrushes.DimGray
            );
        }

        if (document.AcroForm != null)
        {
            var fields = document.AcroForm.Fields;

            FillInBasicInfo(fields, data.BasicInfo);
            FillInTraits(fields, data.Traits);
            FillInSkills(fields, data.SkillInfo, document);
            FillInPowers(data.Powers, document);
            FillInAdminPowers(data.Powers, document);
            StatPage.FillInProficiencies(fields, data.ProficiencyInfo, document, data.BasicInfo.Expression);
            FillInStatInfo(fields, data.StatInfo, document);
            FillInContacts(fields, data.Contacts);

            FillInKnowledges(data.Knowledges, document);
            FillInAdminKnowledges(data.Knowledges, document);
            RechargePage.FillInRechargePage(data, document);
        }

        document.Flatten();

        var finalStream = new MemoryStream();
        document.Save(finalStream, false);
        finalStream.Position = 0;
        return finalStream;
    }

    private static void FillInStatInfo(
        PdfAcroField.PdfAcroFieldCollection fields,
        StatModifierInfo dataStatInfo,
        PdfDocument document
    )
    {
        var page = document.Pages[5];
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Agility.Stat.ToString(),
            XUnitPt.FromInch(2.20),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Agility.Bonus.ToString(),
            XUnitPt.FromInch(2.20),
            XUnitPt.FromInch(9.7)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Constitution.Stat.ToString(),
            XUnitPt.FromInch(2.50),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Constitution.Bonus.ToString(),
            XUnitPt.FromInch(2.50),
            XUnitPt.FromInch(9.7)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Dexterity.Stat.ToString(),
            XUnitPt.FromInch(2.80),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Dexterity.Bonus.ToString(),
            XUnitPt.FromInch(2.80),
            XUnitPt.FromInch(9.7)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Intelligence.Stat.ToString(),
            XUnitPt.FromInch(3.10),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Intelligence.Bonus.ToString(),
            XUnitPt.FromInch(3.10),
            XUnitPt.FromInch(9.7)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Strength.Stat.ToString(),
            XUnitPt.FromInch(3.40),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Strength.Bonus.ToString(),
            XUnitPt.FromInch(3.40),
            XUnitPt.FromInch(9.7)
        );

        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Willpower.Stat.ToString(),
            XUnitPt.FromInch(3.70),
            XUnitPt.FromInch(10.15)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataStatInfo.Willpower.Bonus.ToString(),
            XUnitPt.FromInch(3.70),
            XUnitPt.FromInch(9.7)
        );

        Helpers.MergeField(fields, "AglStat", dataStatInfo.Agility.Stat.ToString());
        Helpers.MergeField(fields, "StrStat", dataStatInfo.Strength.Stat.ToString());
        Helpers.MergeField(fields, "ConStat", dataStatInfo.Constitution.Stat.ToString());
        Helpers.MergeField(fields, "DexStat", dataStatInfo.Dexterity.Stat.ToString());
        Helpers.MergeField(fields, "IntStat", dataStatInfo.Intelligence.Stat.ToString());
        Helpers.MergeField(fields, "WilStat", dataStatInfo.Willpower.Stat.ToString());

        Helpers.MergeField(fields, "AglBonus", dataStatInfo.Agility.Bonus.ToString());
        Helpers.MergeField(fields, "StrBonus", dataStatInfo.Strength.Bonus.ToString());
        Helpers.MergeField(fields, "ConBonus", dataStatInfo.Constitution.Bonus.ToString());
        Helpers.MergeField(fields, "DexBonus", dataStatInfo.Dexterity.Bonus.ToString());
        Helpers.MergeField(fields, "IntBonus", dataStatInfo.Intelligence.Bonus.ToString());
        Helpers.MergeField(fields, "WilBonus", dataStatInfo.Willpower.Bonus.ToString());
    }



    private static void FillInBasicInfo(
        PdfAcroField.PdfAcroFieldCollection fields,
        BasicInfo basicInfo
    )
    {
        Helpers.MergeField(fields, "PlayerNumber", basicInfo.PlayerNumber);
        Helpers.MergeField(
            fields,
            "PlayerNumberAndName",
            $"{basicInfo.PlayerNumber} - {basicInfo.PlayerName}"
        );
        Helpers.MergeField(
            fields,
            "EventNameAndTimeStamp",
            $"{basicInfo.EventName} - {DateTime.Now:MMM dd, yyyy}"
        );
        Helpers.MergeField(fields, "CharacterName", basicInfo.CharacterName);
        Helpers.MergeField(fields, "PlayerName", basicInfo.PlayerName);
        Helpers.MergeField(fields, "Expression", basicInfo.Expression);
        Helpers.MergeField(fields, "CharacterClass", basicInfo.Expression);
        Helpers.MergeField(fields, "Subtype", basicInfo.ProgressionPath);
        Helpers.MergeField(fields, "XL", basicInfo.CharacterLevel);
        Helpers.MergeField(fields, "FactionName", basicInfo.FactionName);
        Helpers.MergeField(fields, "FactionRank", basicInfo.FactionRank);
    }

    private static void FillInTraits(PdfAcroField.PdfAcroFieldCollection fields, Traits traits)
    {
        int advantageCount = 0;
        foreach (var advantage in traits.Advantages)
        {
            Helpers.MergeField(fields, $"Advantages{advantageCount.ToString()}", advantage.Name);
            Helpers.MergeField(fields, $"AdvantagesCost{advantageCount.ToString()}", advantage.Cost);
            advantageCount++;
        }

        int disadvantageCount = 0;
        foreach (var advantage in traits.Disadvantages)
        {
            Helpers.MergeField(fields, $"Disadvantages{disadvantageCount.ToString()}", advantage.Name);
            Helpers.MergeField(fields, $"DisadvantagesCost{disadvantageCount.ToString()}", advantage.Cost);
            disadvantageCount++;
        }
    }

    private static void FillInSkills(
        PdfAcroField.PdfAcroFieldCollection fields,
        SkillInfo skillInfo,
        PdfDocument document
    )
    {
        using var page = XGraphics.FromPdfPage(document.Pages[5]);

        var yPosition = XUnitPt.FromInch(5.95);
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.HandToHandOffense.ToString(),
            XUnitPt.FromInch(1.95),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.MeleeOffense.ToString(),
            XUnitPt.FromInch(2.14),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.ThrownWeapons.ToString(),
            XUnitPt.FromInch(2.32),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.Marksmanship.ToString(),
            XUnitPt.FromInch(2.51),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.Spellcasting.ToString(),
            XUnitPt.FromInch(2.70),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.Projection.ToString(),
            XUnitPt.FromInch(2.89),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.HandToHandDefense.ToString(),
            XUnitPt.FromInch(3.09),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.MeleeDefense.ToString(),
            XUnitPt.FromInch(3.26),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.Acrobatics.ToString(),
            XUnitPt.FromInch(3.43),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.Spellwarding.ToString(),
            XUnitPt.FromInch(3.62),
            yPosition
        );
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.Deflection.ToString(),
            XUnitPt.FromInch(3.81),
            yPosition
        );

        Helpers.MergeField(fields, "H2hOffenseLevel", skillInfo.HandToHandOffense.ToString());
        Helpers.MergeField(fields, "MeleeOffenseLevel", skillInfo.MeleeOffense.ToString());
        Helpers.MergeField(fields, "ThrownWeaponsLevel", skillInfo.ThrownWeapons.ToString());
        Helpers.MergeField(fields, "MarksmanshipLevel", skillInfo.Marksmanship.ToString());
        Helpers.MergeField(fields, "SpellcastingLevel", skillInfo.Spellcasting.ToString());
        Helpers.MergeField(fields, "ProjectionLevel", skillInfo.Projection.ToString());
        Helpers.MergeField(fields, "H2hDefenseLevel", skillInfo.HandToHandDefense.ToString());
        Helpers.MergeField(fields, "MeleeDefenseLevel", skillInfo.MeleeDefense.ToString());
        Helpers.MergeField(fields, "AcrobaticsLevel", skillInfo.Acrobatics.ToString());
        Helpers.MergeField(fields, "SpellwardingLevel", skillInfo.Spellwarding.ToString());
        Helpers.MergeField(fields, "DeflectionLevel", skillInfo.Deflection.ToString());
    }

    private static void FillInAdminPowers(List<PowerInfo> dataPowers, PdfDocument document)
    {
        double startY = XUnitPt.FromInch(0.7); // your starting Y position
        double startX = XUnitPt.FromInch(1.7);
        double lineWidth = XUnitPt.FromInch(4.5);
        int lineCount = dataPowers.Count;

        double lineHeight = 12;
        double fontSize = lineHeight * 0.65;
        var font = new XFont(TextPrintUtilities.DefaultFontFace, fontSize, XFontStyleEx.Regular);
        using (var gfx = XGraphics.FromPdfPage(document.Pages[4]))
        {
            var linePen = new XPen(XColors.Black, 0.5);
            for (int i = 0; i < lineCount; i++)
            {
                // Max Number that can be shown in admin area
                if (i == 20)
                {
                    break;
                }
                double baselineY = startY + (i * lineHeight) + (lineHeight * 0.75) - 3;
                double lineY = baselineY + 1; // sit the rule just under the text baseline

                gfx.DrawString(
                    dataPowers[i].Name,
                    font,
                    XBrushes.Black,
                    XUnitPt.FromInch(1.7),
                    baselineY
                );
                gfx.DrawString(
                    dataPowers[i].Level.Substring(0, 1),
                    font,
                    XBrushes.Black,
                    XUnitPt.FromInch(5.7),
                    baselineY
                );
                gfx.DrawString(
                    dataPowers[i].XPCost.Substring(0, 1),
                    font,
                    XBrushes.Black,
                    XUnitPt.FromInch(6.10),
                    baselineY
                );

                // Draw the underline rule
                gfx.DrawLine(linePen, startX, lineY, startX + lineWidth, lineY);
            }
        }
    }

    private static void FillInPowers(List<PowerInfo> dataPowers, PdfDocument document)
    {
        double totalHeight = XUnitPt.FromInch(6.15);
        double startY = XUnitPt.FromInch(4.60); // your starting Y position
        double startX = XUnitPt.FromInch(1.80);
        double lineWidth = XUnitPt.FromInch(2.90);
        int lineCount = 30;

        double lineHeight = totalHeight / lineCount;
        double fontSize = lineHeight * 0.65;
        var font = new XFont(TextPrintUtilities.DefaultFontFace, fontSize, XFontStyleEx.Regular);
        using (var gfx = XGraphics.FromPdfPage(document.Pages[3]))
        {
            var linePen = new XPen(XColors.Black, 0.5);
            for (int i = 0; i < lineCount; i++)
            {
                double baselineY = startY + (i * lineHeight) + (lineHeight * 0.75);
                double lineY = baselineY + 1; // sit the rule just under the text baseline

                // Draw text (if any for this line)
                if (i < dataPowers.Count)
                {
                    gfx.DrawString(
                        dataPowers[i].Name,
                        font,
                        XBrushes.Black,
                        startX + XUnitPt.FromInch(0.08),
                        baselineY - 3
                    );
                    gfx.DrawString(
                        dataPowers[i].Level.Substring(0, 1),
                        font,
                        XBrushes.Black,
                        startX + XUnitPt.FromInch(2.6),
                        baselineY - 3
                    );
                }

                // Draw the underline rule
                gfx.DrawLine(linePen, startX, lineY, startX + lineWidth, lineY);
            }
        }
    }

    private static void FillInKnowledges(List<KnowledgeInfo> dataPowers, PdfDocument document)
    {
        double totalHeight = XUnitPt.FromInch(6.15);
        double startY = XUnitPt.FromInch(4.60); // your starting Y position
        double startX = XUnitPt.FromInch(1.83);
        double lineWidth = XUnitPt.FromInch(2.90);
        int lineCount = 30;

        double lineHeight = totalHeight / lineCount;
        double fontSize = lineHeight * 0.65;
        var font = new XFont(TextPrintUtilities.DefaultFontFace, fontSize, XFontStyleEx.Regular);
        using (var gfx = XGraphics.FromPdfPage(document.Pages[1]))
        {
            var linePen = new XPen(XColors.Black, 0.5);
            for (int i = 0; i < lineCount; i++)
            {
                double baselineY = startY + (i * lineHeight) + (lineHeight * 0.75);
                double lineY = baselineY + 1; // sit the rule just under the text baseline

                // Draw text (if any for this line)
                if (i < Math.Min(30, dataPowers.Count))
                {
                    gfx.DrawString(
                        dataPowers[i].Name.Substring(0, Math.Min(30, dataPowers[i].Name.Length)),
                        font,
                        XBrushes.Black,
                        XUnitPt.FromInch(1.90),
                        baselineY - 3
                    );
                    gfx.DrawString(
                        dataPowers[i].Level.Substring(0, Math.Min(1, dataPowers[i].Level.Length)),
                        font,
                        XBrushes.Black,
                        XUnitPt.FromInch(4.55),
                        baselineY - 3
                    );
                }

                // Draw the underline rule
                gfx.DrawLine(linePen, startX, lineY, startX + lineWidth, lineY);
            }
        }
    }

    private static void FillInAdminKnowledges(List<KnowledgeInfo> dataPowers, PdfDocument document)
    {
        double startY = XUnitPt.FromInch(0.75); // your starting Y position
        double startX = XUnitPt.FromInch(0.3);
        double lineWidth = XUnitPt.FromInch(3.08);
        int lineCount = dataPowers.Count;

        double lineHeight = 12;
        double fontSize = lineHeight * 0.65;
        var font = new XFont(TextPrintUtilities.DefaultFontFace, fontSize, XFontStyleEx.Regular);
        using (var gfx = XGraphics.FromPdfPage(document.Pages[5]))
        {
            var linePen = new XPen(XColors.Black, 0.5);
            var maxSize = Math.Min(lineCount, 20);
            for (int i = 0; i < maxSize; i++)
            {
                double baselineY = startY + (i * lineHeight) + (lineHeight * 0.75) - 3;
                double lineY = baselineY + 1; // sit the rule just under the text baseline

                gfx.DrawString(
                    dataPowers[i].Name.Substring(0, Math.Min(30, dataPowers[i].Name.Length)),
                    font,
                    XBrushes.Black,
                    XUnitPt.FromInch(0.30),
                    baselineY
                );
                gfx.DrawString(
                    dataPowers[i].Level.Substring(0, Math.Min(1, dataPowers[i].Level.Length)),
                    font,
                    XBrushes.Black,
                    XUnitPt.FromInch(2.95),
                    baselineY
                );
                gfx.DrawString(
                    dataPowers[i].XPCost,
                    font,
                    XBrushes.Black,
                    XUnitPt.FromInch(3.24),
                    baselineY
                );

                // Draw the underline rule
                gfx.DrawLine(linePen, startX, lineY, startX + lineWidth, lineY);
            }
        }
    }

    private static void FillInContacts(
        PdfAcroField.PdfAcroFieldCollection fields,
        List<ContactInfo> dataPowers
    )
    {
        int count = 0;
        foreach (var model in dataPowers)
        {
            Helpers.MergeField(fields, $"ContactName{count.ToString()}", model.Name);
            Helpers.MergeField(fields, $"ContactKnowledge{count.ToString()}", model.KnowledgeName);
            Helpers.MergeField(
                fields,
                $"ContactLevelUses{count.ToString()}",
                $"{model.KnowledgeLevel} ({model.NumberOfUses})"
            );
            count++;
        }
    }
}

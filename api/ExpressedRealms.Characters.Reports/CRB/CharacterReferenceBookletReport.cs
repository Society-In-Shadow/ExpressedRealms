using ExpressedRealms.Characters.Reports.CRB.CrbPages;
using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Pdf.AcroForms;
using PdfSharp.Pdf.IO;
using QRCoder;
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
            TextPrintUtilities.Print90DegreeMessage(page, "— —", centerX, XUnitPt.FromInch(9.5), XBrushes.DimGray);
            TextPrintUtilities.Print90DegreeMessage(page, "— —", centerX, XUnitPt.FromInch(5.5), XBrushes.DimGray);
        }

        if (document.AcroForm != null)
        {
            var fields = document.AcroForm.Fields;

            FillInBasicInfo(fields, data.BasicInfo, document);
            FillInTraits(fields, data.Traits);
            FillInSkills(fields, data.SkillInfo, document);
            FillInPowers(data.Powers, document);
            FillInAdminPowers(data.Powers, document);
            FillInProficiencies(fields, data.ProficiencyInfo, document, data.BasicInfo.Expression);
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

        MergeField(fields, "AglStat", dataStatInfo.Agility.Stat.ToString());
        MergeField(fields, "StrStat", dataStatInfo.Strength.Stat.ToString());
        MergeField(fields, "ConStat", dataStatInfo.Constitution.Stat.ToString());
        MergeField(fields, "DexStat", dataStatInfo.Dexterity.Stat.ToString());
        MergeField(fields, "IntStat", dataStatInfo.Intelligence.Stat.ToString());
        MergeField(fields, "WilStat", dataStatInfo.Willpower.Stat.ToString());

        MergeField(fields, "AglBonus", dataStatInfo.Agility.Bonus.ToString());
        MergeField(fields, "StrBonus", dataStatInfo.Strength.Bonus.ToString());
        MergeField(fields, "ConBonus", dataStatInfo.Constitution.Bonus.ToString());
        MergeField(fields, "DexBonus", dataStatInfo.Dexterity.Bonus.ToString());
        MergeField(fields, "IntBonus", dataStatInfo.Intelligence.Bonus.ToString());
        MergeField(fields, "WilBonus", dataStatInfo.Willpower.Bonus.ToString());
    }

    private static void FillInProficiencies(
        PdfAcroField.PdfAcroFieldCollection fields,
        ProficiencyData dataProficiencyInfo,
        PdfDocument document,
        string expression
    )
    {
        var page = document.Pages[5];

        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Vitality.ToString(),
            XUnitPt.FromInch(2.25),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Health.ToString(),
            XUnitPt.FromInch(2.50),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Blood.ToString(),
            XUnitPt.FromInch(2.75),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.RWP.ToString(),
            XUnitPt.FromInch(3.00),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Reaction.ToString(),
            XUnitPt.FromInch(3.26),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Psyche.ToString(),
            XUnitPt.FromInch(3.53),
            XUnitPt.FromInch(4.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Mortis.ToString(),
            XUnitPt.FromInch(3.80),
            XUnitPt.FromInch(4.60)
        );

        MergeField(fields, "Vitality", dataProficiencyInfo.Vitality.ToString());
        MergeField(fields, "Health", dataProficiencyInfo.Health.ToString());
        MergeField(fields, "Blood", dataProficiencyInfo.Blood.ToString());
        MergeField(fields, "Reaction", dataProficiencyInfo.Reaction.ToString());
        MergeField(fields, "Psyche", dataProficiencyInfo.Psyche.ToString());
        MergeField(fields, "RWP", dataProficiencyInfo.RWP.ToString());
        MergeField(fields, "Mortis", dataProficiencyInfo.Mortis.ToString());

        switch (expression)
        {
            case "Adepts":
                TextPrintUtilities.PrintStatLabelInfo(page, "Chi", XUnitPt.FromInch(4.12), XUnitPt.FromInch(5.48));
                TextPrintUtilities.PrintPPIdentifier(page, XUnitPt.FromInch(4.06), XUnitPt.FromInch(5.68));
                TextPrintUtilities.PrintStatInfo(
                    page,
                    dataProficiencyInfo.Chi.ToString(),
                    XUnitPt.FromInch(4.05),
                    XUnitPt.FromInch(4.60)
                );
                break;
            case "Shammas":
                TextPrintUtilities.PrintStatLabelInfo(
                    page,
                    "Noumenon",
                    XUnitPt.FromInch(4.12),
                    XUnitPt.FromInch(5.48)
                );
                TextPrintUtilities.PrintPPIdentifier(page, XUnitPt.FromInch(4.06), XUnitPt.FromInch(5.68));
                TextPrintUtilities.PrintStatInfo(
                    page,
                    dataProficiencyInfo.Noumenon.ToString(),
                    XUnitPt.FromInch(4.05),
                    XUnitPt.FromInch(4.60)
                );
                break;
            case "Sorcerers":
                TextPrintUtilities.PrintStatLabelInfo(page, "Mana", XUnitPt.FromInch(4.12), XUnitPt.FromInch(5.48));
                TextPrintUtilities.PrintPPIdentifier(page, XUnitPt.FromInch(4.06), XUnitPt.FromInch(5.68));
                TextPrintUtilities.PrintStatInfo(
                    page,
                    dataProficiencyInfo.Mana.ToString(),
                    XUnitPt.FromInch(4.05),
                    XUnitPt.FromInch(4.60)
                );
                break;
            case "Sidhe":
                TextPrintUtilities.PrintStatLabelInfo(page, "Essence", XUnitPt.FromInch(4.12), XUnitPt.FromInch(5.48));
                TextPrintUtilities.PrintPPIdentifier(page, XUnitPt.FromInch(4.06), XUnitPt.FromInch(5.68));
                TextPrintUtilities.PrintStatInfo(
                    page,
                    dataProficiencyInfo.Essence.ToString(),
                    XUnitPt.FromInch(4.05),
                    XUnitPt.FromInch(4.60)
                );
                break;
            case "Aeternari":
                TextPrintUtilities.PrintPPIdentifier(page, XUnitPt.FromInch(2.25), XUnitPt.FromInch(5.68));
                break;
            case "Vampyres":
                TextPrintUtilities.PrintPPIdentifier(page, XUnitPt.FromInch(2.76), XUnitPt.FromInch(5.68));
                break;
        }

        var powerPoints = new List<int>()
        {
            dataProficiencyInfo.Chi,
            dataProficiencyInfo.Essence,
            dataProficiencyInfo.Mana,
            dataProficiencyInfo.Noumenon,
        };

        MergeField(fields, "PowerPoints", powerPoints.Max().ToString());

        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Strike.ToString(),
            XUnitPt.FromInch(2.15),
            XUnitPt.FromInch(8.75)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Thrust.ToString(),
            XUnitPt.FromInch(2.40),
            XUnitPt.FromInch(8.75)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Throw.ToString(),
            XUnitPt.FromInch(2.63),
            XUnitPt.FromInch(8.75)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Shoot.ToString(),
            XUnitPt.FromInch(2.87),
            XUnitPt.FromInch(8.75)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Cast.ToString(),
            XUnitPt.FromInch(3.10),
            XUnitPt.FromInch(8.75)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Project.ToString(),
            XUnitPt.FromInch(3.33),
            XUnitPt.FromInch(8.75)
        );

        MergeField(fields, "Strike", dataProficiencyInfo.Strike.ToString());
        MergeField(fields, "Thrust", dataProficiencyInfo.Thrust.ToString());
        MergeField(fields, "Throw", dataProficiencyInfo.Throw.ToString());
        MergeField(fields, "Shoot", dataProficiencyInfo.Shoot.ToString());
        MergeField(fields, "Cast", dataProficiencyInfo.Cast.ToString());
        MergeField(fields, "Project", dataProficiencyInfo.Project.ToString());

        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Dodge.ToString(),
            XUnitPt.FromInch(2.15),
            XUnitPt.FromInch(7.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Parry.ToString(),
            XUnitPt.FromInch(2.40),
            XUnitPt.FromInch(7.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Throw.ToString(),
            XUnitPt.FromInch(2.63),
            XUnitPt.FromInch(7.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.EvadeShoot.ToString(),
            XUnitPt.FromInch(2.87),
            XUnitPt.FromInch(7.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Ward.ToString(),
            XUnitPt.FromInch(3.10),
            XUnitPt.FromInch(7.60)
        );
        TextPrintUtilities.PrintStatInfo(
            page,
            dataProficiencyInfo.Deflect.ToString(),
            XUnitPt.FromInch(3.33),
            XUnitPt.FromInch(7.60)
        );

        MergeField(fields, "Dodge", dataProficiencyInfo.Dodge.ToString());
        MergeField(fields, "Parry", dataProficiencyInfo.Parry.ToString());
        MergeField(fields, "ThrowEvade", dataProficiencyInfo.EvadeThrow.ToString());
        MergeField(fields, "ShootEvade", dataProficiencyInfo.EvadeShoot.ToString());
        MergeField(fields, "Ward", dataProficiencyInfo.Ward.ToString());
        MergeField(fields, "Deflect", dataProficiencyInfo.Deflect.ToString());
    }

    private static void FillInBasicInfo(
        PdfAcroField.PdfAcroFieldCollection fields,
        BasicInfo basicInfo,
        PdfDocument document
    )
    {
        MergeField(fields, "PlayerNumber", basicInfo.PlayerNumber);
        MergeField(
            fields,
            "PlayerNumberAndName",
            $"{basicInfo.PlayerNumber} - {basicInfo.PlayerName}"
        );
        MergeField(
            fields,
            "EventNameAndTimeStamp",
            $"{basicInfo.EventName} - {DateTime.Now:MMM dd, yyyy}"
        );
        MergeField(fields, "CharacterName", basicInfo.CharacterName);
        MergeField(fields, "PlayerName", basicInfo.PlayerName);
        MergeField(fields, "Expression", basicInfo.Expression);
        MergeField(fields, "CharacterClass", basicInfo.Expression);
        MergeField(fields, "Subtype", basicInfo.ProgressionPath);
        MergeField(fields, "XL", basicInfo.CharacterLevel);

        var page = document.Pages[0];
        var anchor = document.AcroForm.Fields["QrAnchor"] as PdfTextField;

        if (anchor == null)
            throw new InvalidOperationException("QrAnchor field not found.");

        PdfDictionary? widget;

        // Try /Kids first
        var kids = anchor.Elements["/Kids"] as PdfArray;

        if (kids != null && kids.Elements.Count > 0)
        {
            widget = kids.Elements[0] as PdfDictionary;
        }
        else
        {
            // Field itself is the widget
            widget = anchor;
        }

        if (widget == null)
            throw new InvalidOperationException("QrAnchor field not found.");

        var rect = widget.Elements.GetRectangle("/Rect");

        var x = XUnitPt.FromPoint(rect.X1);
        var y = XUnitPt.FromPoint(page.Height.Point - rect.Y2);
        var width = XUnitPt.FromPoint(rect.Width);
        var height = XUnitPt.FromPoint(rect.Height);

        using var gfx = XGraphics.FromPdfPage(page);

        DrawQrCode(gfx, basicInfo.LookupId, x, y, width, height);
    }

    private static void DrawQrCode(
        XGraphics gfx,
        string content,
        XUnitPt x,
        XUnitPt y,
        XUnitPt width,
        XUnitPt height
    )
    {
        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);

        var matrix = data.ModuleMatrix;
        var modules = matrix.Count;

        double moduleSize = XUnitPt.FromPoint(Math.Min(width, height) / modules);

        // Center inside bounds
        var offsetX = x + (width - modules * moduleSize) / 2;
        var offsetY = y + (height - modules * moduleSize) / 2;

        for (int row = 0; row < modules; row++)
        {
            for (int col = 0; col < modules; col++)
            {
                if (matrix[row][col])
                {
                    gfx.DrawRectangle(
                        XBrushes.Black,
                        XUnitPt.FromPoint(offsetX + col * moduleSize),
                        XUnitPt.FromPoint(offsetY + row * moduleSize),
                        XUnitPt.FromPoint(moduleSize),
                        XUnitPt.FromPoint(moduleSize)
                    );
                }
            }
        }
    }

    private static void FillInTraits(PdfAcroField.PdfAcroFieldCollection fields, Traits traits)
    {
        int advantageCount = 0;
        foreach (var advantage in traits.Advantages)
        {
            MergeField(fields, $"Advantages{advantageCount.ToString()}", advantage.Name);
            MergeField(fields, $"AdvantagesCost{advantageCount.ToString()}", advantage.Cost);
            advantageCount++;
        }

        int disadvantageCount = 0;
        foreach (var advantage in traits.Disadvantages)
        {
            MergeField(fields, $"Disadvantages{disadvantageCount.ToString()}", advantage.Name);
            MergeField(fields, $"DisadvantagesCost{disadvantageCount.ToString()}", advantage.Cost);
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
        TextPrintUtilities.PrintSkills(page, skillInfo.MeleeOffense.ToString(), XUnitPt.FromInch(2.14), yPosition);
        TextPrintUtilities.PrintSkills(page, skillInfo.ThrownWeapons.ToString(), XUnitPt.FromInch(2.32), yPosition);
        TextPrintUtilities.PrintSkills(page, skillInfo.Marksmanship.ToString(), XUnitPt.FromInch(2.51), yPosition);
        TextPrintUtilities.PrintSkills(page, skillInfo.Spellcasting.ToString(), XUnitPt.FromInch(2.70), yPosition);
        TextPrintUtilities.PrintSkills(page, skillInfo.Projection.ToString(), XUnitPt.FromInch(2.89), yPosition);
        TextPrintUtilities.PrintSkills(
            page,
            skillInfo.HandToHandDefense.ToString(),
            XUnitPt.FromInch(3.09),
            yPosition
        );
        TextPrintUtilities.PrintSkills(page, skillInfo.MeleeDefense.ToString(), XUnitPt.FromInch(3.26), yPosition);
        TextPrintUtilities.PrintSkills(page, skillInfo.Acrobatics.ToString(), XUnitPt.FromInch(3.43), yPosition);
        TextPrintUtilities.PrintSkills(page, skillInfo.Spellwarding.ToString(), XUnitPt.FromInch(3.62), yPosition);
        TextPrintUtilities.PrintSkills(page, skillInfo.Deflection.ToString(), XUnitPt.FromInch(3.81), yPosition);

        MergeField(fields, "H2hOffenseLevel", skillInfo.HandToHandOffense.ToString());
        MergeField(fields, "MeleeOffenseLevel", skillInfo.MeleeOffense.ToString());
        MergeField(fields, "ThrownWeaponsLevel", skillInfo.ThrownWeapons.ToString());
        MergeField(fields, "MarksmanshipLevel", skillInfo.Marksmanship.ToString());
        MergeField(fields, "SpellcastingLevel", skillInfo.Spellcasting.ToString());
        MergeField(fields, "ProjectionLevel", skillInfo.Projection.ToString());
        MergeField(fields, "H2hDefenseLevel", skillInfo.HandToHandDefense.ToString());
        MergeField(fields, "MeleeDefenseLevel", skillInfo.MeleeDefense.ToString());
        MergeField(fields, "AcrobaticsLevel", skillInfo.Acrobatics.ToString());
        MergeField(fields, "SpellwardingLevel", skillInfo.Spellwarding.ToString());
        MergeField(fields, "DeflectionLevel", skillInfo.Deflection.ToString());
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
                if (i < dataPowers.Count)
                {
                    gfx.DrawString(
                        dataPowers[i].Name,
                        font,
                        XBrushes.Black,
                        XUnitPt.FromInch(1.90),
                        baselineY - 3
                    );
                    gfx.DrawString(
                        dataPowers[i].Specialization ?? "-",
                        font,
                        XBrushes.Black,
                        XUnitPt.FromInch(3.6),
                        baselineY - 3
                    );
                    gfx.DrawString(
                        dataPowers[i].Level.Substring(0, 1),
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
        double lineWidth = XUnitPt.FromInch(4.5);
        int lineCount = dataPowers.Count;

        double lineHeight = 12;
        double fontSize = lineHeight * 0.65;
        var font = new XFont(TextPrintUtilities.DefaultFontFace, fontSize, XFontStyleEx.Regular);
        using (var gfx = XGraphics.FromPdfPage(document.Pages[5]))
        {
            var linePen = new XPen(XColors.Black, 0.5);
            for (int i = 0; i < lineCount; i++)
            {
                // Can only show 15 knowledges in the admin area
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
                    XUnitPt.FromInch(0.30),
                    baselineY
                );
                gfx.DrawString(
                    dataPowers[i].Specialization ?? "-",
                    font,
                    XBrushes.Black,
                    XUnitPt.FromInch(2.45),
                    baselineY
                );
                gfx.DrawString(
                    dataPowers[i].Level.Substring(0, 1),
                    font,
                    XBrushes.Black,
                    XUnitPt.FromInch(4.35),
                    baselineY
                );
                gfx.DrawString(
                    dataPowers[i].XPCost,
                    font,
                    XBrushes.Black,
                    XUnitPt.FromInch(4.75),
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
            MergeField(fields, $"ContactName{count.ToString()}", model.Name);
            MergeField(fields, $"ContactKnowledge{count.ToString()}", model.KnowledgeName);
            MergeField(
                fields,
                $"ContactLevelUses{count.ToString()}",
                $"{model.KnowledgeLevel} ({model.NumberOfUses})"
            );
            count++;
        }
    }

    private static void MergeField(
        PdfAcroField.PdfAcroFieldCollection fields,
        string targetField,
        string data
    )
    {
        var indexes = fields
            .Names.Select((value, index) => new { value, index })
            .Where(x => x.value == targetField)
            .Select(x => x.index)
            .ToList();

        foreach (var index in indexes)
        {
            var pdfCompatibleData = data.Replace("\n", "\r\n").Replace("\r\r", "\r\n");

            if (fields[index] is PdfTextField textField)
            {
                textField.Text = pdfCompatibleData;
                textField.MultiLine = true;
            }
        }
    }
}

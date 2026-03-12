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

    private static void Print90DegreeMessage(XGraphics gfx, string stampText, double centerX, double centerY, XSolidBrush color)
    {
        var font = new XFont("Courier", 10, XFontStyleEx.Regular);
        var size = gfx.MeasureString(stampText, font);

        gfx.Save();
        gfx.TranslateTransform(centerX, centerY);
        gfx.RotateTransform(-90);
        gfx.DrawString(stampText, font, color, -size.Width / 2, font.GetHeight() / 2 -3);
        gfx.Restore();
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
            
            Print90DegreeMessage(page, $"{data.BasicInfo.EventName} - {DateTime.Now:MMM dd, yyyy}", centerX, XUnitPt.FromInch(7.5), XBrushes.DimGray);
            
            // Add Staple Markers
            Print90DegreeMessage(page, "— —", centerX, XUnitPt.FromInch(9.5), XBrushes.DimGray);
            Print90DegreeMessage(page, "— —", centerX, XUnitPt.FromInch(5.5), XBrushes.DimGray);
        }
        
        if (document.AcroForm != null)
        {
            var fields = document.AcroForm.Fields;

            FillInBasicInfo(fields, data.BasicInfo, document);
            FillInTraits(fields, data.Traits);
            FillInSkills(fields, data.SkillInfo);
            FillInPowers(fields, data.Powers);
            FillInProficiencies(fields, data.ProficiencyInfo, document);
            FillInStatInfo(fields, data.StatInfo, document);
            FillInContacts(fields, data.Contacts);
            
            FillInKnowledges(data.Knowledges, document);
            FillInAdminKnowledges(data.Knowledges, document);
        }

        document.Flatten();

        var finalStream = new MemoryStream();
        document.Save(finalStream, false);
        finalStream.Position = 0;
        return finalStream;
    }

    private static void Print90DegreeMessage(PdfPage page, string stampText, double centerX, double centerY)
    {
        using var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Courier", 10, XFontStyleEx.Regular);
        var size = gfx.MeasureString(stampText, font);

        gfx.Save();
        gfx.TranslateTransform(centerX, centerY);
        gfx.RotateTransform(-90);
        gfx.DrawString(stampText, font, XBrushes.Black, -size.Width / 2, font.GetHeight() / 2 -3);
        gfx.Restore();
    }
    
    private static void FillInStatInfo(
        PdfAcroField.PdfAcroFieldCollection fields,
        StatModifierInfo dataStatInfo,
        PdfDocument document
    )
    {
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Agility.Stat.ToString(), 
            XUnitPt.FromInch(2.20), XUnitPt.FromInch(10.15));
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Agility.Bonus.ToString(), 
            XUnitPt.FromInch(2.20), XUnitPt.FromInch(9.5));
        
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Constitution.Stat.ToString(), 
            XUnitPt.FromInch(2.50), XUnitPt.FromInch(10.15));
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Constitution.Bonus.ToString(), 
            XUnitPt.FromInch(2.50), XUnitPt.FromInch(9.5));
        
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Dexterity.Stat.ToString(), 
            XUnitPt.FromInch(2.80), XUnitPt.FromInch(10.15));
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Dexterity.Bonus.ToString(), 
            XUnitPt.FromInch(2.80), XUnitPt.FromInch(9.5));
        
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Intelligence.Stat.ToString(), 
            XUnitPt.FromInch(3.10), XUnitPt.FromInch(10.15));
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Intelligence.Bonus.ToString(), 
            XUnitPt.FromInch(3.10), XUnitPt.FromInch(9.5));
        
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Strength.Stat.ToString(), 
            XUnitPt.FromInch(3.40), XUnitPt.FromInch(10.15));
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Strength.Bonus.ToString(), 
            XUnitPt.FromInch(3.40), XUnitPt.FromInch(9.5));
        
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Willpower.Stat.ToString(), 
            XUnitPt.FromInch(3.70), XUnitPt.FromInch(10.15));
        Print90DegreeMessage(document.Pages[1], dataStatInfo.Willpower.Bonus.ToString(), 
            XUnitPt.FromInch(3.70), XUnitPt.FromInch(9.5));
        
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
        PdfDocument document
    )
    {
        
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Vitality.ToString(), 
            XUnitPt.FromInch(2.25), XUnitPt.FromInch(4.60));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Health.ToString(), 
            XUnitPt.FromInch(2.50), XUnitPt.FromInch(4.60));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Blood.ToString(), 
            XUnitPt.FromInch(2.75), XUnitPt.FromInch(4.60));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.RWP.ToString(), 
            XUnitPt.FromInch(3.00), XUnitPt.FromInch(4.60));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Reaction.ToString(), 
            XUnitPt.FromInch(3.26), XUnitPt.FromInch(4.60));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Psyche.ToString(), 
            XUnitPt.FromInch(3.53), XUnitPt.FromInch(4.60));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Mortis.ToString(), 
            XUnitPt.FromInch(3.80), XUnitPt.FromInch(4.60));
        
        MergeField(fields, "Vitality", dataProficiencyInfo.Vitality.ToString());
        MergeField(fields, "Health", dataProficiencyInfo.Health.ToString());
        MergeField(fields, "Blood", dataProficiencyInfo.Blood.ToString());
        MergeField(fields, "Reaction", dataProficiencyInfo.Reaction.ToString());
        MergeField(fields, "Psyche", dataProficiencyInfo.Psyche.ToString());
        MergeField(fields, "RWP", dataProficiencyInfo.RWP.ToString());
        MergeField(fields, "Mortis", dataProficiencyInfo.Mortis.ToString());

        var powerPoints = new List<int>()
        {
            dataProficiencyInfo.Chi,
            dataProficiencyInfo.Essence,
            dataProficiencyInfo.Mana,
            dataProficiencyInfo.Noumenon,
        };
        
        Print90DegreeMessage(document.Pages[1], powerPoints.Max().ToString(), 
            XUnitPt.FromInch(4.05), XUnitPt.FromInch(4.60));
        
        MergeField(fields, "PowerPoints", powerPoints.Max().ToString());

        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Strike.ToString(), 
            XUnitPt.FromInch(2.15), XUnitPt.FromInch(8.25));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Thrust.ToString(), 
            XUnitPt.FromInch(2.40), XUnitPt.FromInch(8.25));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Throw.ToString(), 
            XUnitPt.FromInch(2.63), XUnitPt.FromInch(8.25));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Shoot.ToString(), 
            XUnitPt.FromInch(2.87), XUnitPt.FromInch(8.25));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Cast.ToString(), 
            XUnitPt.FromInch(3.10), XUnitPt.FromInch(8.25));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Project.ToString(), 
            XUnitPt.FromInch(3.33), XUnitPt.FromInch(8.25));
        
        MergeField(fields, "Strike", dataProficiencyInfo.Strike.ToString());
        MergeField(fields, "Thrust", dataProficiencyInfo.Thrust.ToString());
        MergeField(fields, "Throw", dataProficiencyInfo.Throw.ToString());
        MergeField(fields, "Shoot", dataProficiencyInfo.Shoot.ToString());
        MergeField(fields, "Cast", dataProficiencyInfo.Cast.ToString());
        MergeField(fields, "Project", dataProficiencyInfo.Project.ToString());
        
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Dodge.ToString(), 
            XUnitPt.FromInch(2.15), XUnitPt.FromInch(6.45));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Parry.ToString(), 
            XUnitPt.FromInch(2.40), XUnitPt.FromInch(6.45));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Throw.ToString(), 
            XUnitPt.FromInch(2.63), XUnitPt.FromInch(6.45));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.EvadeShoot.ToString(), 
            XUnitPt.FromInch(2.87), XUnitPt.FromInch(6.45));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Ward.ToString(), 
            XUnitPt.FromInch(3.10), XUnitPt.FromInch(6.45));
        Print90DegreeMessage(document.Pages[1], dataProficiencyInfo.Deflect.ToString(), 
            XUnitPt.FromInch(3.33), XUnitPt.FromInch(6.45));
        
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
        MergeField(fields, "PlayerNumberAndName", $"{basicInfo.PlayerNumber} - {basicInfo.PlayerName}");
        MergeField(fields, "EventNameAndTimeStamp", $"{basicInfo.EventName} - {DateTime.Now:MMM dd, yyyy}");
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
        SkillInfo skillInfo
    )
    {
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

    private static void FillInPowers(
        PdfAcroField.PdfAcroFieldCollection fields,
        List<PowerInfo> dataPowers
    )
    {
        int count = 0;
        foreach (var powers in dataPowers)
        {
            MergeField(fields, $"PowerName{count.ToString()}", powers.Name);
            MergeField(fields, $"PowerLevel{count.ToString()}", powers.Level.Substring(0, 1));
            MergeField(fields, $"PowerExp{count.ToString()}", powers.XPCost);
            count++;
        }
    }
    
    private static void FillInKnowledges(
        List<KnowledgeInfo> dataPowers,
        PdfDocument document,
        bool showXP = false
    )
    {
        double totalHeight = XUnitPt.FromInch(6.15);
        double startY = XUnitPt.FromInch(4.60); // your starting Y position
        double startX = XUnitPt.FromInch(1.83);
        double lineWidth = XUnitPt.FromInch(2.90);
        int lineCount = 30;

        double lineHeight = totalHeight / lineCount;
        double fontSize = lineHeight * 0.65;
        var font = new XFont("Courier", fontSize, XFontStyleEx.Regular);
        using (var gfx = XGraphics.FromPdfPage(document.Pages[5]))
        {
            var linePen = new XPen(XColors.Black, 0.5);
            for (int i = 0; i < lineCount; i++)
            {
                double baselineY = startY + (i * lineHeight) + (lineHeight * 0.75);
                double lineY = baselineY + 1; // sit the rule just under the text baseline
                
                // Draw text (if any for this line)
                if (i < dataPowers.Count)
                {
                    gfx.DrawString(dataPowers[i].Name, font, XBrushes.Black, XUnitPt.FromInch(1.90), baselineY - 3);
                    gfx.DrawString(dataPowers[i].Specialization ?? string.Empty, font, XBrushes.Black, XUnitPt.FromInch(3.6), baselineY - 3);
                    gfx.DrawString(dataPowers[i].Level.Substring(0, 1), font, XBrushes.Black, XUnitPt.FromInch(4.55), baselineY - 3);
                    if (showXP)
                    {
                        gfx.DrawString(dataPowers[i].Level.Substring(0, 1), font, XBrushes.Black, XUnitPt.FromInch(4.55), baselineY - 3);

                    }
                }
                
                // Draw the underline rule
                gfx.DrawLine(linePen, startX, lineY, startX + lineWidth, lineY);
            }
        };
    }
    
    private static void FillInAdminKnowledges(
        List<KnowledgeInfo> dataPowers,
        PdfDocument document
    )
    {
        double startY = XUnitPt.FromInch(0.75); // your starting Y position
        double startX = XUnitPt.FromInch(0.3);
        double lineWidth = XUnitPt.FromInch(4.5);
        int lineCount = dataPowers.Count;

        double lineHeight = 12;
        double fontSize = lineHeight * 0.65;
        var font = new XFont("Courier", fontSize, XFontStyleEx.Regular);
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
                double baselineY = startY + (i * lineHeight) + (lineHeight * 0.75) -3;
                double lineY = baselineY + 1; // sit the rule just under the text baseline

                gfx.DrawString(dataPowers[i].Name, font, XBrushes.Black, XUnitPt.FromInch(0.30), baselineY);
                gfx.DrawString(dataPowers[i].Specialization ?? string.Empty, font, XBrushes.Black, XUnitPt.FromInch(2.45), baselineY );
                gfx.DrawString(dataPowers[i].Level.Substring(0, 1), font, XBrushes.Black, XUnitPt.FromInch(4.35), baselineY);
                gfx.DrawString(dataPowers[i].Level.Substring(0, 1), font, XBrushes.Black, XUnitPt.FromInch(4.75), baselineY);
                
                // Draw the underline rule
                gfx.DrawLine(linePen, startX, lineY, startX + lineWidth, lineY);
            }
        };
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

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

        if (document.AcroForm != null)
        {
            var fields = document.AcroForm.Fields;

            FillInBasicInfo(fields, data.BasicInfo, document);
            FillInTraits(fields, data.Traits);
            FillInSkills(fields, data.SkillInfo);
            FillInPowers(fields, data.Powers);
            FillInKnowledges(fields, data.Knowledges);
            FillInProficiencies(fields, data.ProficiencyInfo);
            FillInStatInfo(fields, data.StatInfo);
            FillInContacts(fields, data.Contacts);
        }

        document.Flatten();

        var finalStream = new MemoryStream();
        document.Save(finalStream, false);
        finalStream.Position = 0;
        return finalStream;
    }

    private static void FillInStatInfo(
        PdfAcroField.PdfAcroFieldCollection fields,
        StatModifierInfo dataStatInfo
    )
    {
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
        ProficiencyData dataProficiencyInfo
    )
    {
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

        MergeField(fields, "PowerPoints", powerPoints.Max().ToString());

        MergeField(fields, "Strike", dataProficiencyInfo.Strike.ToString());
        MergeField(fields, "Thrust", dataProficiencyInfo.Thrust.ToString());
        MergeField(fields, "Throw", dataProficiencyInfo.Throw.ToString());
        MergeField(fields, "Shoot", dataProficiencyInfo.Shoot.ToString());
        MergeField(fields, "Cast", dataProficiencyInfo.Cast.ToString());
        MergeField(fields, "Project", dataProficiencyInfo.Project.ToString());
        MergeField(fields, "Dodge", dataProficiencyInfo.Dodge.ToString());
        MergeField(fields, "Parry", dataProficiencyInfo.Parry.ToString());
        MergeField(fields, "ThrowEvade", dataProficiencyInfo.EvadeThrow.ToString());
        MergeField(fields, "ShootEvade", dataProficiencyInfo.EvadeShoot.ToString());
        MergeField(fields, "Ward", dataProficiencyInfo.Ward.ToString());
        MergeField(fields, "Deflect", dataProficiencyInfo.Deflect.ToString());
    }

    private static void FillInBasicInfo(PdfAcroField.PdfAcroFieldCollection fields,
        BasicInfo basicInfo, PdfDocument document)
    {
        MergeField(fields, "PlayerNumber", basicInfo.PlayerNumber);
        MergeField(fields, "LastName", basicInfo.PlayerName);
        MergeField(fields, "CharacterName", basicInfo.CharacterName);
        MergeField(fields, "PlayerName", basicInfo.PlayerName);
        MergeField(fields, "Expression", basicInfo.Expression);
        MergeField(fields, "CharacterClass", basicInfo.Expression);
        MergeField(fields, "Subtype", basicInfo.ProgressionPath);
        MergeField(fields, "XL", basicInfo.CharacterLevel);
        
        var page = document.Pages[2];
        var anchor = document.AcroForm.Fields["QrAnchor"] as PdfTextField;

        if (anchor == null)
            throw new Exception("QrAnchor field not found.");

        PdfDictionary widget;

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

        var rect = widget.Elements.GetRectangle("/Rect");
        
        double x = rect.X1;
        double y = page.Height - rect.Y2;
        double width = rect.Width;
        double height = rect.Height;
        
        using var gfx = XGraphics.FromPdfPage(page);

        DrawQrCode(gfx, basicInfo.LookupId, x, y, width, height);
    }
    
    private static void DrawQrCode(XGraphics gfx, string content, double x, double y, double width, double height)
    {
        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.H);

        var matrix = data.ModuleMatrix;
        int modules = matrix.Count;

        double moduleSize = Math.Min(width, height) / modules;

        // Center inside bounds
        double offsetX = x + (width - modules * moduleSize) / 2;
        double offsetY = y + (height - modules * moduleSize) / 2;

        for (int row = 0; row < modules; row++)
        {
            for (int col = 0; col < modules; col++)
            {
                if (matrix[row][col] == true)
                {
                    gfx.DrawRectangle(
                        XBrushes.Black,
                        offsetX + col * moduleSize,
                        offsetY + row * moduleSize,
                        moduleSize,
                        moduleSize);
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
        PdfAcroField.PdfAcroFieldCollection fields,
        List<KnowledgeInfo> dataPowers
    )
    {
        int count = 0;
        foreach (var model in dataPowers)
        {
            MergeField(fields, $"KnowledgeName{count.ToString()}", model.Name);
            MergeField(fields, $"KnowledgeLevel{count.ToString()}", model.Level.Substring(0, 1));
            MergeField(fields, $"Specialization{count.ToString()}", model.Specialization ?? "");
            MergeField(fields, $"KnowledgeExp{count.ToString()}", model.XPCost);
            count++;
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

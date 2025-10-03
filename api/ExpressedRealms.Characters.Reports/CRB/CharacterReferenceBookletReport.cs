using System.Text;
using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.IO;
using QuestPDF;
using QuestPDF.Infrastructure;

namespace ExpressedRealms.Characters.Reports.CRB;

public static class CharacterReferenceBookletReport
{

    public static MemoryStream GenerateReport(ReportData data)
    {
        Settings.License = LicenseType.Community;

        return MergeAllFields(data);
    }

    
    private static MemoryStream MergeAllFields(ReportData data)
    {
        using var document = PdfReader.Open("overallCRB.pdf", PdfDocumentOpenMode.Modify);

        if (document.AcroForm != null)
        {
            var fields = document.AcroForm.Fields;

            FillInBasicInfo(fields, data.BasicInfo);
            FillInTraits(fields, data.Traits);
            FillInSkills(fields, data.SkillInfo);
            FillInPowers(fields, data.Powers);
            FillInKnowledges(fields, data.Knowledges);
            FillInProficiencies(fields, data.ProficiencyInfo);
            FillInStatInfo(fields, data.StatInfo);
        }
        
        //FlattenAllFields(document);

        var finalStream = new MemoryStream();
        document.Save(finalStream, false);
        finalStream.Position = 0;
        return finalStream;
    }

    private static void FillInStatInfo(PdfAcroField.PdfAcroFieldCollection fields, StatModifierInfo dataStatInfo)
    {
        MergeField(fields, "AglStat",   dataStatInfo.Agility.Stat.ToString());
        MergeField(fields, "StrStat",   dataStatInfo.Strength.Stat.ToString());
        MergeField(fields, "ConStat",   dataStatInfo.Constitution.Stat.ToString());
        MergeField(fields, "DexStat",   dataStatInfo.Dexterity.Stat.ToString());
        MergeField(fields, "IntStat",   dataStatInfo.Intelligence.Stat.ToString());
        MergeField(fields, "WilStat",   dataStatInfo.Willpower.Stat.ToString());
        
        MergeField(fields, "AglBonus",   dataStatInfo.Agility.Bonus.ToString());
        MergeField(fields, "StrBonus",   dataStatInfo.Strength.Bonus.ToString());
        MergeField(fields, "ConBonus",   dataStatInfo.Constitution.Bonus.ToString());
        MergeField(fields, "DexBonus",   dataStatInfo.Dexterity.Bonus.ToString());
        MergeField(fields, "IntBonus",   dataStatInfo.Intelligence.Bonus.ToString());
        MergeField(fields, "WilBonus",   dataStatInfo.Willpower.Bonus.ToString());
    }

    private static void FillInProficiencies(PdfAcroField.PdfAcroFieldCollection fields, ProficiencyData dataProficiencyInfo)
    {
        MergeField(fields, "Vitality",   dataProficiencyInfo.Vitality.ToString());
        MergeField(fields, "Health",     dataProficiencyInfo.Health.ToString());
        MergeField(fields, "Blood",      dataProficiencyInfo.Blood.ToString());
        MergeField(fields, "Reaction",   dataProficiencyInfo.Reaction.ToString());
        MergeField(fields, "Psyche",     dataProficiencyInfo.Psyche.ToString());
        MergeField(fields, "RWP",        dataProficiencyInfo.RWP.ToString());
        MergeField(fields, "Mortis",     dataProficiencyInfo.Mortis.ToString());
        
        var powerPoints = new List<int>() { dataProficiencyInfo.Chi, dataProficiencyInfo.Essence, dataProficiencyInfo.Mana, dataProficiencyInfo.Noumenon };
        
        MergeField(fields, "PowerPoints" ,powerPoints.Max().ToString());
        
        MergeField(fields, "Strike",     dataProficiencyInfo.Strike.ToString());
        MergeField(fields, "Thrust",     dataProficiencyInfo.Thrust.ToString());
        MergeField(fields, "Throw",      dataProficiencyInfo.Throw.ToString());
        MergeField(fields, "Shoot",      dataProficiencyInfo.Shoot.ToString());
        MergeField(fields, "Cast",       dataProficiencyInfo.Cast.ToString());
        MergeField(fields, "Project",    dataProficiencyInfo.Project.ToString());
        MergeField(fields, "Dodge",      dataProficiencyInfo.Dodge.ToString());
        MergeField(fields, "Parry",      dataProficiencyInfo.Parry.ToString());
        MergeField(fields, "ThrowEvade", dataProficiencyInfo.EvadeThrow.ToString());
        MergeField(fields, "ShootEvade", dataProficiencyInfo.EvadeShoot.ToString());
        MergeField(fields, "Ward",       dataProficiencyInfo.Ward.ToString());
        MergeField(fields, "Deflect",    dataProficiencyInfo.Deflect.ToString());
    }

    private static void FillInBasicInfo(PdfAcroField.PdfAcroFieldCollection fields, BasicInfo basicInfo)
    {
        MergeField(fields,"PlayerNumber", basicInfo.PlayerNumber);
        MergeField(fields,"LastName", basicInfo.PlayerName);
        MergeField(fields,"CharacterName", basicInfo.CharacterName);
        MergeField(fields,"PlayerName", basicInfo.PlayerName);
        MergeField(fields,"Expression", basicInfo.Expression);
        MergeField(fields,"CharacterClass", basicInfo.Expression);
        MergeField(fields,"Subtype", basicInfo.ProgressionPath);
        MergeField(fields,"XL", basicInfo.CharacterLevel);
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

    private static void FillInSkills(PdfAcroField.PdfAcroFieldCollection fields, SkillInfo skillInfo)
    {
        MergeField(fields,"H2hOffenseLevel", skillInfo.HandToHandOffense.ToString());
        MergeField(fields,"MeleeOffenseLevel", skillInfo.MeleeOffense.ToString());
        MergeField(fields,"ThrownWeaponsLevel", skillInfo.ThrownWeapons.ToString());
        MergeField(fields,"MarksmanshipLevel", skillInfo.Marksmanship.ToString());
        MergeField(fields,"SpellcastingLevel", skillInfo.Spellcasting.ToString());
        MergeField(fields,"ProjectionLevel", skillInfo.Projection.ToString());
        MergeField(fields,"H2hDefenseLevel", skillInfo.HandToHandDefense.ToString());
        MergeField(fields,"MeleeDefenseLevel", skillInfo.MeleeDefense.ToString());
        MergeField(fields,"AcrobaticsLevel", skillInfo.Acrobatics.ToString());
        MergeField(fields,"SpellwardingLevel", skillInfo.Spellwarding.ToString());
        MergeField(fields,"DeflectionLevel", skillInfo.Deflection.ToString());
    }


    private static void FillInPowers(PdfAcroField.PdfAcroFieldCollection fields, List<PowerInfo> dataPowers)
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
    
    private static void FillInKnowledges(PdfAcroField.PdfAcroFieldCollection fields, List<KnowledgeInfo> dataPowers)
    {
        StringBuilder nameString = new();
        StringBuilder levelString = new();
        StringBuilder specializationString = new();
        StringBuilder xpString = new();
        foreach (var model in dataPowers)
        {
            nameString.AppendLine(model.Name);
            specializationString.AppendLine(model.Specialization);
            levelString.AppendLine(model.Level.Substring(0, 1));
            xpString.AppendLine(model.XPCost);
        }
        
        MergeField(fields,"KnowledgeName", nameString.ToString());
        MergeField(fields,"KnowledgeLevel", levelString.ToString());
        MergeField(fields,"Specialization", specializationString.ToString());
        MergeField(fields,"KnowledgeExp", xpString.ToString());
    }
    
        
    private static void MergeField(PdfAcroField.PdfAcroFieldCollection fields, string targetField, string data)
    {
        var indexes = fields.Names
            .Select((value, index) => new { value, index })
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

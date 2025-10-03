using System.Text;
using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using PdfSharpCore.Pdf;
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
        }

        var finalStream = new MemoryStream();
        document.Save(finalStream, false);
        finalStream.Position = 0;
        return finalStream;
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
        StringBuilder advantageString = new();
        StringBuilder xpSpent = new();
        foreach (var advantage in traits.Advantages)
        {
            advantageString.AppendLine(advantage.Name);
            xpSpent.AppendLine(advantage.Cost);
        }
        
        MergeField(fields,"Advantages", advantageString.ToString());
        MergeField(fields,"AdvantagesExp", xpSpent.ToString());
        
        StringBuilder disadvantageString = new();
        StringBuilder disXpSpent = new();
        foreach (var advantage in traits.Disadvantages)
        {
            disadvantageString.AppendLine(advantage.Name);
            disXpSpent.AppendLine(advantage.Cost);
        }
        
        MergeField(fields,"Disadvantages", disadvantageString.ToString());
        MergeField(fields,"DisadvantagesExp", disXpSpent.ToString());
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
        StringBuilder nameString = new();
        StringBuilder levelString = new();
        StringBuilder xpString = new();
        foreach (var model in dataPowers)
        {
            nameString.AppendLine(model.Name);
            levelString.AppendLine(model.Level.Substring(0, 1));
            xpString.AppendLine(model.XPCost);
        }
        
        MergeField(fields,"PowerName", nameString.ToString());
        MergeField(fields,"PowerLevel", levelString.ToString());
        MergeField(fields,"PowerExp", xpString.ToString());
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
            fields[index].Value = new PdfString(data);
        }
    }

}

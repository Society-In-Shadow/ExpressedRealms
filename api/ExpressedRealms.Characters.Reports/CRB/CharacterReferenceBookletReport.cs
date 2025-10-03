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
    
    private static void FillInBasicInfo(PdfAcroField.PdfAcroFieldCollection fields, BasicInfo basicInfo)
    {
        MergeField(fields,"PlayerNumber", basicInfo.PlayerNumber);
        MergeField(fields,"LastName", basicInfo.PlayerName);
        MergeField(fields,"CharacterName", basicInfo.CharacterName);
        MergeField(fields,"PlayerName", basicInfo.PlayerName);
        MergeField(fields,"Expression", basicInfo.Expression);
        MergeField(fields,"Subtype", basicInfo.ProgressionPath);
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

    private static void FillInSkills(PdfAcroField.PdfAcroFieldCollection fields, Skills skills)
    {
        MergeField(fields,"H2hOffenseLevel", skills.HandToHandOffense.ToString());
        MergeField(fields,"MeleeOffenseLevel", skills.MeleeOffense.ToString());
        MergeField(fields,"ThrownWeaponsLevel", skills.ThrownWeapons.ToString());
        MergeField(fields,"MarksmanshipLevel", skills.Marksmanship.ToString());
        MergeField(fields,"SpellcastingLevel", skills.Spellcasting.ToString());
        MergeField(fields,"ProjectionLevel", skills.Projection.ToString());
        MergeField(fields,"H2hDefenseLevel", skills.HandToHandDefense.ToString());
        MergeField(fields,"MeleeDefenseLevel", skills.MeleeDefense.ToString());
        MergeField(fields,"AcrobaticsLevel", skills.Acrobatics.ToString());
        MergeField(fields,"SpellwardingLevel", skills.Spellwarding.ToString());
        MergeField(fields,"DeflectionLevel", skills.Deflection.ToString());
    }

    private static MemoryStream MergeAllFields(ReportData data)
    {
        using var document = PdfReader.Open("overallCRB.pdf", PdfDocumentOpenMode.Modify);

        if (document.AcroForm != null)
        {
            var fields = document.AcroForm.Fields;

            FillInBasicInfo(fields, data.BasicInfo);
            //FillInTraits(fields, data.Traits);
            //FillInSkills(fields, data.Skills);
        }

        var finalStream = new MemoryStream();
        document.Save(finalStream, false);
        finalStream.Position = 0;
        return finalStream;
    }
    
    
}

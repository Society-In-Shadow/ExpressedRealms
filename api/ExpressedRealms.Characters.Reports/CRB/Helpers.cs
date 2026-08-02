using PdfSharp.Pdf.AcroForms;

namespace ExpressedRealms.Characters.Reports.CRB;


public static class Helpers
{
    internal static void MergeField(
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
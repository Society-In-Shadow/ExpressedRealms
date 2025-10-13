using ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet
{
    [UsedImplicitly]
    internal sealed class GetCharacterBookletUseCase(
        IGetCharacterPowerCardReportUseCase powerReport,
        IGetCharacterSheetReportUseCase crbReport,
        GetCharacterBookletModelValidator validator,
        CancellationToken cancellationToken
    ) : IGetCharacterBookletUseCase
    {
        public async Task<Result<MemoryStream>> ExecuteAsync(GetCharacterBookletModel model)
        {
            var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
                validator,
                model,
                cancellationToken
            );

            if (result.IsFailed)
                return Result.Fail(result.Errors);

            var crb = await crbReport.ExecuteAsync(
                new GetCharacterSheetReportModel() { CharacterId = model.CharacterId }
            );

            var powerCards = await powerReport.ExecuteAsync(
                new GetCharacterPowerCardReportModel()
                {
                    CharacterId = model.CharacterId,
                    IsFiveByThree = false,
                }
            );

            // Use PDFSharp to merge both PDFs
            using var finalDocument = new PdfDocument();

            // Add pages from the QuestPDF document
            using var questPdfDoc = PdfReader.Open(crb.Value, PdfDocumentOpenMode.Import);
            foreach (PdfPage page in questPdfDoc.Pages)
            {
                finalDocument.AddPage(page);
            }

            // Add pages from the other PDF document
            using var otherPdfDoc = PdfReader.Open(powerCards.Value, PdfDocumentOpenMode.Import);
            foreach (PdfPage page in otherPdfDoc.Pages)
            {
                finalDocument.AddPage(page);
                var blankPage = finalDocument.AddPage();
                blankPage.Orientation = PageOrientation.Landscape;
            }

            // Save the merged result to memory stream
            var finalStream = new MemoryStream();
            finalDocument.Save(finalStream, false);
            finalStream.Position = 0;

            return finalStream;
        }
    }
}

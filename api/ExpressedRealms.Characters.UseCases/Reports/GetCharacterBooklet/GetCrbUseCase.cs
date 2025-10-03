using ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet
{
    [UsedImplicitly]
    internal sealed class GetCharacterBookletUseCase(
        IGetCharacterPowerCardReportUseCase powerReport,
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

            /*backgroundReport.GenerateMemoryStream = false;
            await backgroundReport.ExecuteAsync(
                new GetExpressionCmsReportModel() { ExpressionId = model.ExpressionId }
            );*/
            
            var powerCards = await powerReport.ExecuteAsync(
                new GetCharacterPowerCardReportModel() { CharacterId = model.CharacterId, IsFiveByThree = false}
            );

            // Use PDFSharp to merge both PDFs
            using var finalDocument = new PdfDocument();
            
            /*// Add pages from the QuestPDF document
            using var questPdfDoc = PdfReader.Open(questPdfStream, PdfDocumentOpenMode.Import);
            foreach (PdfPage page in questPdfDoc.Pages)
            {
                finalDocument.AddPage(page);
            }*/
            
            // Add pages from the other PDF document  
            using var otherPdfDoc = PdfReader.Open(powerCards.Value, PdfDocumentOpenMode.Import);
            foreach (PdfPage page in otherPdfDoc.Pages)
            {
                finalDocument.AddPage(page);
            }

            // Save the merged result to memory stream
            var finalStream = new MemoryStream();
            finalDocument.Save(finalStream, false);
            finalStream.Position = 0;
            
            return finalStream;
        }
    }
}

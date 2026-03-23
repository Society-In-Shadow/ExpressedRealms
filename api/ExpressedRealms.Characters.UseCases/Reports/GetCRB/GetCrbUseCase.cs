using ExpressedRealms.Characters.Repository.Players;
using ExpressedRealms.Characters.Repository.Proficiencies;
using ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet;
using ExpressedRealms.DB.Models.Checkins.CheckinSecondaryStatsSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCRB
{
    [UsedImplicitly]
    internal sealed class GetCharacterBookletUseCase(
        IGetCharacterPowerCardReportUseCase powerReport,
        IGetCharacterSheetReportUseCase crbReport,
        IPlayerRepository playerRepository,
        IEventCheckinRepository checkinRepository,
        IProficiencyRepository profRepository,
        IApproveStageAndSendMessageUseCase sendMessageUseCase,
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

            var eventId = await checkinRepository.GetActiveEventId();
            if (eventId is not null)
            {
                var player = await playerRepository.GetPlayerByCharacterId(model.CharacterId);
                var checkin = await checkinRepository.GetCheckinAsync(eventId!.Value, player.Id);
                if (checkin is not null)
                {
                    var currentStage = await checkinRepository.GetCurrentStage(checkin.Id);
                    if (currentStage is not null && currentStage.Id == CheckinStageEnum.CrbCreation)
                    {
                        await sendMessageUseCase.ExecuteAsync(
                            new()
                            {
                                LookupId = player.LookupId,
                                StageId = CheckinStageEnum.PrintedCrb,
                            }
                        );
                        
                        var proficiencies = await profRepository.GetBasicProficiencies(model.CharacterId);

                        await checkinRepository.AddUpdateSecondaryStats(new CheckinSecondaryStat()
                        {
                            CheckinId = checkin.Id,
                            Vitality = proficiencies.Value.First(x => x.Id == 13).Value,
                            Health = proficiencies.Value.First(x => x.Id == 14).Value,
                            Blood = proficiencies.Value.First(x => x.Id == 15).Value,
                            Psyche = proficiencies.Value.First(x => x.Id == 17).Value,
                            Rwp = proficiencies.Value.First(x => x.Id == 22).Value,
                            Mortis = proficiencies.Value.First(x => x.Id == 23).Value,
                            Chi = proficiencies.Value.FirstOrDefault(x => x.Id == 18)?.Value ?? 0,
                            Essence = proficiencies.Value.FirstOrDefault(x => x.Id == 19)?.Value ?? 0,
                            Mana = proficiencies.Value.FirstOrDefault(x => x.Id == 20)?.Value ?? 0,
                            Noumenon = proficiencies.Value.FirstOrDefault(x => x.Id == 21)?.Value ?? 0,
                        });
                    }
                }
            }

            // Save the merged result to memory stream
            var finalStream = new MemoryStream();
            await finalDocument.SaveAsync(finalStream);
            finalStream.Position = 0;

            return finalStream;
        }
    }
}

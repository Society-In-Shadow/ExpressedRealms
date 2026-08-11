using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Characters.Reports.CRB;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using ExpressedRealms.Characters.Reports.CRB.DataCards.ContactsOverflowCards;
using ExpressedRealms.Characters.Reports.CRB.DataCards.KnowledgeOverflowCards;
using ExpressedRealms.Characters.Reports.CRB.DataCards.PowerOverflowCards;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Players;
using ExpressedRealms.Characters.Repository.Proficiencies;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet;
using ExpressedRealms.DB.Models.Checkins.CheckinSecondaryStatsSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.Powers.Reporting.powerCards.CardPluginSystem;
using ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;
using JetBrains.Annotations;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PopulatePowerOverflowCard = ExpressedRealms.Characters.Reports.CRB.DataCards.PowerOverflowCards.PopulatePowerOverflowCard;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCRB
{
    [UsedImplicitly]
    internal sealed class GetCharacterBookletUseCase(
        IGetCharacterPowerCardReportUseCase powerReport,
        IGetCharacterSheetDataUseCase crbDataUseCase,
        IPlayerRepository playerRepository,
        IEventCheckinRepository checkinRepository,
        IProficiencyRepository profRepository,
        ICharacterRepository characterRepository,
        IXpRepository xpRepository,
        IApproveStageAndSendMessageUseCase sendMessageUseCase,
        IUserContext userContext,
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

            var character = await characterRepository.FindCharacterAsync(model.CharacterId);

            var canDownloadAllCrbs = userContext.CurrentUserHasPermission(
                Permissions.CharacterManagement.DownloadAllCrbs
            );
            var canDownloadPrimaryCharacterCrbs =
                character!.IsPrimaryCharacter
                && userContext.CurrentUserHasPermission(
                    Permissions.CharacterManagement.ViewCharacterSheet
                );

            if (!canDownloadAllCrbs && !canDownloadPrimaryCharacterCrbs)
            {
                return Result.Fail(new UnauthorizedError());
            }

            var crbData = await crbDataUseCase.ExecuteAsync(
                new GetCharacterSheetDataModel() { CharacterId = model.CharacterId }
            );

            var reportStream = CharacterReferenceBookletReport.GenerateReport(crbData.Value);
            reportStream.Position = 0;

            var cardTiles = new List<ICardTile>();
            PopulateKnowledgeOverflowCardData(cardTiles, crbData.Value.Knowledges);
            PopulatePowersOverflowCardData(cardTiles, crbData.Value.Powers);
            PopulateContactsOverflowCardData(cardTiles, crbData.Value.Contacts);

            var powerCards = await powerReport.ExecuteAsync(
                new GetCharacterPowerCardReportModel()
                {
                    CharacterId = model.CharacterId,
                    IsFiveByThree = false,
                    IncludeWealthCard = true,
                    CardTiles = cardTiles,
                }
            );

            // Use PDFSharp to merge both PDFs
            using var finalDocument = new PdfDocument();

            // Add pages from the QuestPDF document
            using var questPdfDoc = PdfReader.Open(reportStream, PdfDocumentOpenMode.Import);
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

            await ProcessCheckinAndUpdateStats(model);

            // Save the merged result to memory stream
            var finalStream = new MemoryStream();
            await finalDocument.SaveAsync(finalStream);
            finalStream.Position = 0;

            return finalStream;
        }

        private static void PopulatePowersOverflowCardData(
            List<ICardTile> cardTiles,
            List<PowerInfo> powers
        )
        {
            if (powers.Count > 30)
            {
                cardTiles.Add(
                    new PopulatePowerOverflowCard(
                        new PowerOverflowCardData()
                        {
                            Powers = powers
                                .Take(new Range(30, powers.Count + 1))
                                .Select(x => new Power() { Name = x.Name, Level = x.Level.Substring(0, 1) })
                                .ToList(),
                        }
                    )
                );
            }
        }
        
        private static void PopulateKnowledgeOverflowCardData(
            List<ICardTile> cardTiles,
            List<KnowledgeInfo> knowledges
        )
        {
            if (knowledges.Count > 30)
            {
                cardTiles.Add(
                    new PopulateKnowledgeOverflowCard(
                        new KnowledgeOverflowCardData()
                        {
                            Knowledges = knowledges
                                .Take(new Range(30, knowledges.Count + 1))
                                .Select(x => new Knowledge() { Name = x.Name, Level = x.Level })
                                .ToList(),
                        }
                    )
                );
            }
        }
        
        private static void PopulateContactsOverflowCardData(
            List<ICardTile> cardTiles,
            List<ContactInfo> contacts
        )
        {
            if (contacts.Count > 6)
            {
                cardTiles.Add(
                    new PopulateContactOverflowCard(
                        new ContactsOverflowCardData()
                        {
                            Contacts = contacts
                                .Take(new Range(6, contacts.Count + 1))
                                .Select(x => new Contact()
                                {
                                    Name = x.Name, 
                                    KnowledgeName = x.KnowledgeName,
                                    KnowledgeLevel = x.KnowledgeLevel,
                                    NumberOfUses = x.NumberOfUses
                                })
                                .ToList(),
                        }
                    )
                );
            }
        }

        private async Task ProcessCheckinAndUpdateStats(GetCharacterBookletModel model)
        {
            var eventId = await checkinRepository.GetActiveEventId();
            if (eventId is null)
                return;

            var player = await playerRepository.GetPlayerByCharacterId(model.CharacterId);
            var checkin = await checkinRepository.GetCheckinAsync(eventId.Value, player.Id);
            if (checkin is null)
                return;

            var currentStage = await checkinRepository.GetCurrentStage(checkin.Id);
            if (currentStage is not null && currentStage.Id == CheckinStageEnum.CrbCreation)
            {
                await sendMessageUseCase.ExecuteAsync(
                    new() { LookupId = player.LookupId, StageId = CheckinStageEnum.PrintedCrb }
                );

                var proficiencies = await profRepository.GetBasicProficiencies(model.CharacterId);

                var character = await characterRepository.GetCharacterInfoForPickablePowers(
                    model.CharacterId
                );
                var characterLevel = await xpRepository.GetCharacterXpLevel(model.CharacterId);

                await checkinRepository.AddUpdateSecondaryStats(
                    new CheckinSecondaryStat()
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
                        ExpressionId = character.ExpressionSubTypeId,
                        PlayerLevel = characterLevel,
                    }
                );
            }
        }
    }
}

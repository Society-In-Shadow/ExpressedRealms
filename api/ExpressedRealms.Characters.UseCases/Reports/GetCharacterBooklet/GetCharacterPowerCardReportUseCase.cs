using ExpressedRealms.Characters.Reports.CRB;
using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Characters.Reports.CRB.Data.SupportingData;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet;

public class GetCharacterSheetReportUseCase(
    IPowerPathRepository repository,
    ICharacterRepository characterRepository,
    ICharacterPowerRepository mappingRepository,
    GetCharacterSheetReportModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterSheetReportUseCase
{
    public async Task<Result<MemoryStream>> ExecuteAsync(GetCharacterSheetReportModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var character = await characterRepository.GetCharacterInfoForCRB(model.CharacterId);

        var basicInfo = new BasicInfo()
        {
            CharacterName = character.CharacterName,
            Expression = character.Expression,
            ProgressionPath = string.IsNullOrWhiteSpace(character.SecondaryProgressionName) ? character.PrimaryProgressionName ?? "" : $"{character.PrimaryProgressionName} Secondary {character.SecondaryProgressionName}",
            PlayerNumber = character.PlayerNumber.ToString("D3"),
            PlayerName = character.PlayerName
        };


        var reportStream = CharacterReferenceBookletReport.GenerateReport(new ReportData()
            {
                BasicInfo = basicInfo
            });

        reportStream.Position = 0;
        return reportStream;
    }
}

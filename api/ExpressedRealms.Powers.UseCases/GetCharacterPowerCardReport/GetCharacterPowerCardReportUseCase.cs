using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Powers.Reporting.powerCards;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;

public class GetCharacterPowerCardReportUseCase(
    IPowerPathRepository repository,
    IExpressionRepository expressionRepository,
    ICharacterPowerRepository mappingRepository,
    GetCharacterPowerCardReportModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterPowerCardReportUseCase
{
    public async Task<Result<MemoryStream>> ExecuteAsync(GetCharacterPowerCardReportModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);
        
        var expression = await expressionRepository.GetExpression(model.ExpressionId);
        var selectedPowerInformation = await mappingRepository.GetCharacterPowerMappingInfo(model.CharacterId);
        var data = await repository.GetPowerPathAndPowers(selectedPowerInformation.Select(x => x.PowerId).ToList());

        var reportStream = PowerCardReport.GenerateSixUpPdf(
            data.Value.SelectMany(x =>
                    x.Powers.Select(y => new PowerCardData()
                        {
                            AreaOfEffect = y.AreaOfEffect.Name,
                            Name = y.Name,
                            Category = y.Category?.Select(z => z.Name).ToList(),
                            Description = y.Description,
                            PathName = x.Name,
                            GameMechanicEffect = y.GameMechanicEffect,
                            ExpressionName = expression.Value.Name,
                            PowerActivationType = y.PowerActivationType.Name,
                            PowerDuration = y.PowerDuration.Name,
                            PowerLevel = y.PowerLevel.Name,
                            Cost = y.Cost,
                            Id = y.Id,
                            IsPowerUse = y.IsPowerUse,
                            Limitation = y.Limitation,
                            Other = y.Other,
                            UserNotes = selectedPowerInformation.FirstOrDefault(x => x.PowerId == y.Id)?.UserNotes ?? null,
                            Prerequisites = y.Prerequisites is not null
                                ? new PrerequisiteData()
                                {
                                    Count = y.Prerequisites.RequiredAmount,
                                    PrerequisiteNames = y.Prerequisites.Powers,
                                }
                                : null,
                        })
                        .ToList()
                )
                .ToList(),
            model.IsFiveByThree
        );

        reportStream.Position = 0;
        return reportStream;
    }
}

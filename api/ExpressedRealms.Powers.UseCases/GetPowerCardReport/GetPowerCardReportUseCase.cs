using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Powers.Reporting.powerCards;
using ExpressedRealms.Powers.Repository.PowerPaths;

namespace ExpressedRealms.Powers.UseCases.GetPowerCardReport;

public class GetPowerCardReportUseCase(
    IPowerPathRepository repository,
    IExpressionRepository expressionRepository
) : IGetPowerCardReportUseCase
{
    public async Task<MemoryStream> ExecuteAsync(GetPowerCardReportModel model)
    {
        var data = await repository.GetPowerPathAndPowers(model.ExpressionId);
        var expression = await expressionRepository.GetExpression(model.ExpressionId);

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

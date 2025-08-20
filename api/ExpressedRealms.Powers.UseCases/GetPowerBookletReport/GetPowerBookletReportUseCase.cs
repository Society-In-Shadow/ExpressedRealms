using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Powers.Reporting.PowerBookletFormat;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using QuestPDF.Fluent;

namespace ExpressedRealms.Powers.UseCases.GetPowerBookletReport;

public class GetPowerBookletReportUseCase(
    IPowerPathRepository repository,
    IExpressionRepository expressionRepository,
    GetPowerBookletReportModelValidator validator,
    CancellationToken cancellationToken
) : IGetPowerBookletReportUseCase
{
    public Document? GeneratedReport { get; set; }
    public bool GenerateMemoryStream { get; set; } = true;

    public async Task<Result<MemoryStream>> ExecuteAsync(GetPowerBookletReportModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var data = await repository.GetPowerPathAndPowers(model.ExpressionId);
        var expression = await expressionRepository.GetExpression(model.ExpressionId);

        var report = PowerBookletReport.GenerateReport(
            new PowerBookletData()
            {
                ExpressionName = expression.Value.Name,
                PowerPaths = data
                    .Value.Select(x => new PowerPathData()
                    {
                        ExpressionName = expression.Value.Name,
                        Name = x.Name,
                        Description = x.Description,
                        Powers = x
                            .Powers.Select(y => new PowerData()
                            {
                                AreaOfEffect = y.AreaOfEffect.Name,
                                Name = y.Name,
                                Category = y.Category?.Select(z => z.Name).ToList(),
                                Description = y.Description,
                                GameMechanicEffect = y.GameMechanicEffect,
                                PowerActivationType = y.PowerActivationType.Name,
                                PowerDuration = y.PowerDuration.Name,
                                PowerLevel = y.PowerLevel.Name,
                                Cost = y.Cost,
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
                            .ToList(),
                    })
                    .ToList(),
            }
        );

        if (!GenerateMemoryStream)
        {
            GeneratedReport = report;
            return Result.Fail("Stream Option Was Disabled");
        }

        var stream = new MemoryStream();
        report.GeneratePdf(stream);

        stream.Position = 0;
        return stream;
    }
}

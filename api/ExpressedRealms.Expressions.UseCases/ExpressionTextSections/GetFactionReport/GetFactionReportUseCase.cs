using ExpressedRealms.Expressions.Reports.FactionReport;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFactions;
using ExpressedRealms.Powers.Reporting.PowerBookletFormat;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;
using QuestPDF.Fluent;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetFactionReport;

[UsedImplicitly]
internal sealed class GetFactionReportUseCase(
    IExpressionRepository expressionRepository,
    IGetFactionsUseCase factionsUseCase,
    GetFactionReportModelValidator validator,
    CancellationToken cancellationToken
) : IGetFactionReportUseCase
{
    public Document? GeneratedReport { get; set; }
    public bool GenerateMemoryStream { get; set; } = true;

    public async Task<Result<MemoryStream>> ExecuteAsync(GetFactionReportModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var factions = await factionsUseCase.ExecuteAsync(
            new GetFactionsModel() { ExpressionId = model.ExpressionId }
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var reportData = factions
            .Value.Factions.Select(x => new FactionData()
            {
                Name = x.Name,
                Background = x.Background,
                FactionLevels = x
                    .FactionLevels.Select(y => new FactionLevelData()
                    {
                        RankName = y.RankName,
                        KnowledgeLevel = y.KnowledgeLevel,
                        KnowledgeName = y.Knowledge,
                        KnowledgeSpecialization = y.Specialization,
                        Power = y.Power is null
                            ? null
                            : new PowerData()
                            {
                                AreaOfEffect = y.Power.AreaOfEffect.Name,
                                Name = y.Power.Name,
                                Category = y.Power.Category?.Select(z => z.Name).ToList(),
                                Description = y.Power.Description,
                                GameMechanicEffect = y.Power.GameMechanicEffect,
                                PowerActivationType = y.Power.PowerActivationType.Name,
                                PowerDuration = y.Power.PowerDuration.Name,
                                PowerLevel = y.Power.PowerLevel.Name,
                                PowerLevelId = y.Power.PowerLevel.Id,
                                Cost = y.Power.Cost,
                                IsPowerUse = y.Power.IsPowerUse,
                                Limitation = y.Power.Limitation,
                                Other = y.Power.Other,
                                Prerequisites = null,
                            },
                    })
                    .ToList(),
            })
            .ToList();

        var expression = await expressionRepository.GetExpression(model.ExpressionId);

        var report = FactionReport.GenerateReport(
            new FactionReportData()
            {
                ExpressionName = expression.Value.Name,
                Factions = reportData,
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

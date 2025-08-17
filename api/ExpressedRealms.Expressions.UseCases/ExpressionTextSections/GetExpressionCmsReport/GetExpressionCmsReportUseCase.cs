using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Expressions.Reports.ExpressionCMSReport;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Expressions.Repository.ExpressionTextSections.DTOs;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;
using QuestPDF.Fluent;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionCmsReport;

[UsedImplicitly]
internal sealed class GetExpressionCmsReportUseCase(
    IExpressionTextSectionRepository repository,
    IExpressionRepository expressionRepository,
    IKnowledgeRepository knowledgesRepository,
    IBlessingRepository blessingsRepository,
    GetExpressionCmsReportModelValidator validator,
    CancellationToken cancellationToken
) : IGetExpressionCmsReportUseCase
{
    public Document? GeneratedReport { get; set; }
    public bool GenerateMemoryStream { get; set; } = true;

    public async Task<Result<MemoryStream>> ExecuteAsync(GetExpressionCmsReportModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var sections = await repository.GetExpressionTextSections(model.ExpressionId);
        var expression = await expressionRepository.GetExpression(model.ExpressionId);

        int sortOrder = await repository.GetKnowledgesSectionId();
        var flattenedSections = FlattenHierarchy(sections, ref sortOrder);

        var knowledgeSectionId = await repository.GetKnowledgesSectionId();
        var knowledgeSection = flattenedSections.FirstOrDefault(x =>
            x.SectionTypeId == knowledgeSectionId
        );
        if (knowledgeSection is not null)
        {
            var knowledges = await knowledgesRepository.GetKnowledges();

            knowledgeSection.Knowledges = knowledges
                .Select(x => new KnowledgeData()
                {
                    Name = x.Name,
                    Type = x.KnowledgeType.Name,
                    Description = x.Description,
                })
                .ToList();
        }

        var blessingId = await repository.GetBlessingSectionId();
        var blessingSection = flattenedSections.FirstOrDefault(x => x.SectionTypeId == blessingId);
        if (blessingSection is not null)
        {
            var blessings = await blessingsRepository.GetAllBlessingsAndBlessingLevels();

            blessingSection.Blessings = blessings
                .Select(x => new BlessingData()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type,
                    SubType = x.SubCategory,
                    Levels = x
                        .BlessingLevels.Select(y => new BlessingLevelData()
                        {
                            Level = y.Level,
                            Description = y.Description,
                        })
                        .ToList(),
                })
                .ToList();
        }

        var report = ExpressionCmsReport.GenerateReport(
            new ExpressionCmsReportData()
            {
                IsExpression = await repository.IsExpression(model.ExpressionId),
                ExpressionName = expression.Value.Name,
                Sections = flattenedSections,
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

    private static List<SectionData> FlattenHierarchy(
        List<ExpressionSectionDto> request,
        ref int sortOrder,
        int level = 0
    )
    {
        var flatList = new List<SectionData>();

        foreach (var item in request)
        {
            flatList.Add(
                new SectionData()
                {
                    SectionTypeId = item.SectionTypeId,
                    Name = item.Name,
                    Content = item.Content,
                    SortOrder = sortOrder,
                    Level = level,
                }
            );

            sortOrder++;
            if (item.SubSections.Count == 0)
                continue;
            flatList.AddRange(FlattenHierarchy(item.SubSections, ref sortOrder, level + 1));
        }
        return flatList;
    }
}

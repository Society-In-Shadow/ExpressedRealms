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
    GetExpressionCmsReportModelValidator validator,
    CancellationToken cancellationToken
) : IGetExpressionCmsReportUseCase
{
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
        var knowledgeSection = flattenedSections.FirstOrDefault(x => x.SectionTypeId == knowledgeSectionId);
        if (knowledgeSection is not null)
        {
            var knowledges = await knowledgesRepository.GetKnowledges();

            knowledgeSection.Knowledges = knowledges.Select(x => new KnowledgeData()
            {
                Name = x.Name,
                Type = x.KnowledgeType.Name,
                Description = x.Description
            }).ToList();
        }
        
        var report = ExpressionCmsReport.GenerateReport(
            new ExpressionCmsReportData()
            {
                ExpressionName = expression.Value.Name,
                Sections = flattenedSections
            }
        );

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
                    Level = level
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

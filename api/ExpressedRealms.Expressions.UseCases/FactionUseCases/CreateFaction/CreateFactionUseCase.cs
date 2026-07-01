using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.UseCases.FactionLevelUseCases.CreateFactionLevel;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.CreateFaction;

internal sealed class CreateFactionUseCase(
    IFactionRepository factionRepository,
    IExpressionRepository expressionRepository,
    CreateFactionModelValidator validator,
    ICreateFactionLevelUseCase createFactionLevelUseCase,
    CancellationToken cancellationToken
) : ICreateFactionUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateFactionModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var isExpressionType = await expressionRepository.ExpressionIsExpressionType(
            model.ExpressionId
        );
        if (!isExpressionType)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.ExpressionId),
                "Expression Id is not of an expression type."
            );

        var isDuplicateName = await factionRepository.HasDuplicateName(model.Name);
        if (isDuplicateName)
            return ValidationHelper.AddSingleValidationFailure(
                nameof(model.Name),
                "This name already exists."
            );

        var factionId = await factionRepository.CreateFactionAsync(
            new Faction()
            {
                Name = model.Name,
                Background = model.Background,
                ExpressionId = model.ExpressionId,
            }
        );

        var results = await createFactionLevelUseCase.ExecuteAsync(
            new CreateFactionLevelModel()
            {
                FactionId = factionId,
                Specialization = model.Specialization,
                KnowledgeId = model.KnowledgeId,
            }
        );

        if (results.IsFailed)
            return results;

        return Result.Ok(factionId);
    }
}

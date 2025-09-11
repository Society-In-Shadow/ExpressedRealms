using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Get;

internal sealed class GetAssignedBlessingsUseCase(
    ICharacterBlessingRepository repository,
    GetAssignedBlessingsModelValidator validator,
    CancellationToken cancellationToken
) : IGetAssignedBlessingsUseCase
{
    public async Task<Result<List<CharacterBlessingReturnModel>>> ExecuteAsync(
        GetAssignedBlessingsModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var blessings = await repository.GetBlessingsForCharacter(model.CharacterId);

        return Result.Ok(
            blessings
                .Select(x => new CharacterBlessingReturnModel()
                {
                    Description = x.Description,
                    LevelDescription = x.LevelDescription,
                    LevelName = x.LevelName,
                    Name = x.Name,
                    BlessingLevelId = x.BlessingLevelId,
                    BlessingId = x.BlessingId,
                })
                .ToList()
        );
    }
}

using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.LeaveFaction;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.CharacterFaction;

public class LeaveFactionUseCaseTests
{
    private readonly LeaveFactionUseCase _useCase;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterFactionRepository _characterFactionRepository;
    private readonly LeaveFactionModel _model;
    private readonly Character _character;

    public LeaveFactionUseCaseTests()
    {
        _model = new LeaveFactionModel() { CharacterId = 1 };

        _character = new Character() { Id = _model.CharacterId, ExpressionId = 3 };

        _characterRepository = A.Fake<ICharacterRepository>();
        _characterFactionRepository = A.Fake<ICharacterFactionRepository>();

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(_character);

        A.CallTo(() =>
                _characterFactionRepository.GetFactionLevelsForBulkEditing(_model.CharacterId)
            )
            .Returns(new List<CharacterFactionMapping>());

        var validator = new LeaveFactionModelValidator();

        _useCase = new LeaveFactionUseCase(
            _characterRepository,
            _characterFactionRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterId_IsEmpty()
    {
        _model.CharacterId = 0;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(LeaveFactionModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterDoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))!
            .Returns(Task.FromResult((Character)null!));

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(LeaveFactionModel.CharacterId),
            "Character Id does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillSoftDelete_AndBulkEdit_AllFactionMappings()
    {
        var factionMappings = new List<CharacterFactionMapping>()
        {
            new() { Id = 7, CharacterId = _model.CharacterId },
            new() { Id = 8, CharacterId = _model.CharacterId },
        };

        A.CallTo(() =>
                _characterFactionRepository.GetFactionLevelsForBulkEditing(_model.CharacterId)
            )
            .Returns(factionMappings);

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _characterFactionRepository.BulkEditCharacterFactionAsync(
                    A<List<CharacterFactionMapping>>.That.Matches(k =>
                        k.Count == 2 && k.All(x => x.IsDeleted) && k.All(x => x.DeletedAt != null)
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_Success_IfSuccessful()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
    }
}

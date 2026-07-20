using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.JoinFaction;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.CharacterFaction;

public class JoinFactionUseCaseTests
{
    private readonly JoinFactionUseCase _useCase;
    private readonly IFactionRepository _factionRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterFactionRepository _characterFactionRepository;
    private readonly JoinFactionModel _model;
    private readonly Character _character;

    private const int FactionRankId = 7;
    private const int CharacterFactionMappingId = 11;

    public JoinFactionUseCaseTests()
    {
        _model = new JoinFactionModel()
        {
            CharacterId = 1,
            FactionId = 2,
        };

        _character = new Character()
        {
            Id = _model.CharacterId,
            ExpressionId = 3,
        };

        _factionRepository = A.Fake<IFactionRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _characterFactionRepository = A.Fake<ICharacterFactionRepository>();

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId)).Returns(_character);
        A.CallTo(() =>
                _factionRepository.GetBasicFactionRankId(_model.FactionId, _character.ExpressionId)
            )
            .Returns(FactionRankId);
        A.CallTo(() =>
                _characterFactionRepository.JoinFaction(A<DB.Models.Factions.CharacterFactionMappingModels.CharacterFactionMapping>._)
            )
            .Returns(CharacterFactionMappingId);

        var validator = new JoinFactionModelValidator();

        _useCase = new JoinFactionUseCase(
            _factionRepository,
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
            nameof(JoinFactionModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionId_WillFail_WhenFactionId_IsEmpty()
    {
        _model.FactionId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(JoinFactionModel.FactionId),
            "Faction Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterDoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))!
            .Returns(Task.FromResult((Character)null!));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(JoinFactionModel.CharacterId),
            "Character Id is not of an expression type."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionId_WillFail_WhenFactionDoesNotExistForCharactersExpression()
    {
        A.CallTo(() =>
                _factionRepository.GetBasicFactionRankId(_model.FactionId, _character.ExpressionId)
            )
            .Returns(Task.FromResult((int?)null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(JoinFactionModel.FactionId),
            "This faction does not exist for the character's expression."
        );
    }

    [Fact]
    public async Task UseCase_WillJoin_TheFaction()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _characterFactionRepository.JoinFaction(
                    A<DB.Models.Factions.CharacterFactionMappingModels.CharacterFactionMapping>.That.Matches(k =>
                        k.CharacterId == _model.CharacterId
                        && k.FactionRankId == FactionRankId
                        && !k.RequestPromotion
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillSet_ApprovalDate()
    {
        var before = DateTimeOffset.UtcNow;

        await _useCase.ExecuteAsync(_model);

        var after = DateTimeOffset.UtcNow;

        A.CallTo(() =>
                _characterFactionRepository.JoinFaction(
                    A<DB.Models.Factions.CharacterFactionMappingModels.CharacterFactionMapping>.That.Matches(k =>
                        k.ApprovalDate >= before && k.ApprovalDate <= after
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_CharacterFactionMappingId_IfSuccessful()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(CharacterFactionMappingId, result.Value);
    }
}

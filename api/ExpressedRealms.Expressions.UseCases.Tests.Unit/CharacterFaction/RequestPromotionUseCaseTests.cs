using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;
using ExpressedRealms.DB.Models.Factions.FactionLevelModels;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.RequestPromotion;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.CharacterFaction;

public class RequestPromotionUseCaseTests
{
    private readonly RequestPromotionUseCase _useCase;
    private readonly IFactionRepository _factionRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterFactionRepository _characterFactionRepository;
    private readonly ICharacterKnowledgeRepository _knowledgeRepository;
    private readonly RequestPromotionModel _model;
    private readonly FactionLevel _factionLevel;
    private readonly PlayerFactionInfoDto _characterFactionInfo;

    public RequestPromotionUseCaseTests()
    {
        _model = new RequestPromotionModel()
        {
            CharacterId = 1,
            FactionLevelId = 2,
            RequestReason = "Please promote me.",
        };

        var character = new Character() { Id = _model.CharacterId, ExpressionId = 3 };

        _factionLevel = new FactionLevel()
        {
            Id = _model.FactionLevelId,
            FactionId = 4,
            FactionRankId = FactionRankEnum.Intermediate.Value,
            KnowledgeId = 5,
            KnowledgeLevelId = 6,
            Specialization = "Alchemy",
        };

        _characterFactionInfo = new PlayerFactionInfoDto()
        {
            FactionId = _factionLevel.FactionId,
            FactionLevelId = 7,
            FactionRankId = FactionRankEnum.Basic.Value,
        };

        _factionRepository = A.Fake<IFactionRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _characterFactionRepository = A.Fake<ICharacterFactionRepository>();
        _knowledgeRepository = A.Fake<ICharacterKnowledgeRepository>();

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(character);

        A.CallTo(() => _factionRepository.GetFactionLevelAsync(_model.FactionLevelId))
            .Returns(_factionLevel);

        A.CallTo(() => _characterFactionRepository.GetPlayerFactionInfo(_model.CharacterId))
            .Returns(_characterFactionInfo);

        A.CallTo(() =>
                _knowledgeRepository.HasFactionPrerequisites(
                    _factionLevel.KnowledgeId.Value,
                    _factionLevel.KnowledgeLevelId.Value,
                    _factionLevel.Specialization
                )
            )
            .Returns(true);

        var validator = new RequestPromotionModelValidator();

        _useCase = new RequestPromotionUseCase(
            _factionRepository,
            _characterRepository,
            _characterFactionRepository,
            _knowledgeRepository,
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
            nameof(RequestPromotionModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenFactionLevelId_IsEmpty()
    {
        _model.FactionLevelId = 0;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.FactionLevelId),
            "Faction Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_RequestReason_WillFail_WhenRequestReason_IsTooLong()
    {
        _model.RequestReason = new string('a', 20_001);

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.RequestReason),
            "The length of 'Request Reason' must be 20000 characters or fewer. You entered 20001 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterDoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))!
            .Returns(Task.FromResult((Character)null!));

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.CharacterId),
            "Character Id does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenFactionLevelDoesNotExist()
    {
        A.CallTo(() => _factionRepository.GetFactionLevelAsync(_model.FactionLevelId))!
            .Returns(Task.FromResult((FactionLevel)null!));

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.FactionLevelId),
            "This faction level does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterDoesNotHaveFaction()
    {
        A.CallTo(() => _characterFactionRepository.GetPlayerFactionInfo(_model.CharacterId))!
            .Returns(Task.FromResult((PlayerFactionInfoDto)null!));

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.CharacterId),
            "Character does not have a faction."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenFactionLevelDoesNotBelongToCharactersFaction()
    {
        _factionLevel.FactionId = 999;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.FactionLevelId),
            "This faction level does not belong to the character's faction."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenFactionLevelIsBasicRank()
    {
        _factionLevel.FactionRankId = FactionRankEnum.Basic.Value;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.FactionLevelId),
            "Basic faction levels are automatically approved upon joining."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenGoAlreadyApprovedRank()
    {
        _characterFactionInfo.FactionRankId = FactionRankEnum.Intermediate.Value;
        _factionLevel.FactionRankId = FactionRankEnum.Intermediate.Value;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.FactionLevelId),
            "GO already approved this rank."
        );
    }
    
    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenGoAlreadyApprovedRank_OnceRemoved()
    {
        _characterFactionInfo.FactionRankId = FactionRankEnum.Advance.Value;
        _factionLevel.FactionRankId = FactionRankEnum.Intermediate.Value;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.FactionLevelId),
            "GO already approved this rank."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenCharacterDoesNotHavePreviousRankApproved()
    {
        _characterFactionInfo.FactionRankId = FactionRankEnum.Basic.Value;
        _factionLevel.FactionRankId = FactionRankEnum.Supreme.Value;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.FactionLevelId),
            "Character does not have a previous rank approved."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenCharacterDoesNotHaveFactionPrerequisites()
    {
        A.CallTo(() =>
                _knowledgeRepository.HasFactionPrerequisites(
                    _factionLevel.KnowledgeId!.Value,
                    _factionLevel.KnowledgeLevelId!.Value,
                    _factionLevel.Specialization!
                )
            )
            .Returns(false);

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(RequestPromotionModel.FactionLevelId),
            "Character does not have one or more of the required knowledge, knowledge level, or specialization for this faction level."
        );
    }

    [Fact]
    public async Task UseCase_WillRequestPromotion()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _characterFactionRepository.AddCharacterFactionMapping(
                    A<CharacterFactionMapping>.That.Matches(k =>
                        k.CharacterId == _model.CharacterId
                        && k.FactionLevelId == _model.FactionLevelId
                        && k.RequestPromotion
                        && k.RequestReason == _model.RequestReason
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
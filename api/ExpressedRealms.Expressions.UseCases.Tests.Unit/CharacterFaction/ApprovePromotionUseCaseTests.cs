using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;
using ExpressedRealms.DB.Models.Factions.FactionLevelModels;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.ApprovePromotion;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.CharacterFaction;

public class ApprovePromotionUseCaseTests
{
    private readonly ApprovePromotionUseCase _useCase;
    private readonly IFactionRepository _factionRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterFactionRepository _characterFactionRepository;
    private readonly ICharacterKnowledgeRepository _knowledgeRepository;
    private readonly TimeProvider _timeProvider;
    private readonly IUserContext _userContext;
    private readonly ApprovePromotionModel _model;
    private readonly FactionLevel _factionLevel;
    private readonly PlayerFactionInfoDto _characterFactionInfo;
    private readonly DateTimeOffset _approvalDate;
    private readonly string _userId;

    public ApprovePromotionUseCaseTests()
    {
        _model = new ApprovePromotionModel()
        {
            CharacterId = 1,
            FactionLevelId = 2,
            ApprovalReason = "The character completed the required faction trial.",
        };

        var character = new Character()
        {
            Id = _model.CharacterId,
            ExpressionId = 3,
        };

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

        _approvalDate = new DateTimeOffset(2026, 7, 31, 12, 30, 0, TimeSpan.Zero);
        _userId = Guid.NewGuid().ToString();

        _factionRepository = A.Fake<IFactionRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _characterFactionRepository = A.Fake<ICharacterFactionRepository>();
        _knowledgeRepository = A.Fake<ICharacterKnowledgeRepository>();
        _timeProvider = A.Fake<TimeProvider>();
        _userContext = A.Fake<IUserContext>();

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(character);

        A.CallTo(() => _factionRepository.GetFactionLevelAsync(_model.FactionLevelId))
            .Returns(_factionLevel);

        A.CallTo(() => _characterFactionRepository.GetPlayerFactionInfo(_model.CharacterId))
            .Returns(_characterFactionInfo);

        A.CallTo(() =>
                _knowledgeRepository.HasFactionPrerequisites(
                    _model.CharacterId,
                    _factionLevel.KnowledgeId.Value,
                    _factionLevel.KnowledgeLevelId.Value,
                    _factionLevel.Specialization
                )
            )
            .Returns(true);

        A.CallTo(() =>
                _characterFactionRepository.GetCharacterFactionMapping(
                    _model.CharacterId,
                    _model.FactionLevelId
                )
            )
            .Returns(Task.FromResult<CharacterFactionMapping?>(null));

        A.CallTo(() => _timeProvider.GetUtcNow()).Returns(_approvalDate);
        A.CallTo(() => _userContext.CurrentUserId()).Returns(_userId);

        var validator = new ApprovePromotionModelValidator();

        _useCase = new ApprovePromotionUseCase(
            _factionRepository,
            _characterRepository,
            _characterFactionRepository,
            _knowledgeRepository,
            _timeProvider,
            _userContext,
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
            nameof(ApprovePromotionModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenFactionLevelId_IsEmpty()
    {
        _model.FactionLevelId = 0;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(ApprovePromotionModel.FactionLevelId),
            "Faction Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_ApprovalReason_WillFail_WhenApprovalReason_IsEmpty()
    {
        _model.ApprovalReason = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(ApprovePromotionModel.ApprovalReason),
            "Approval Reason is required."
        );
    }

    [Fact]
    public async Task ValidationFor_ApprovalReason_WillFail_WhenApprovalReason_IsTooShort()
    {
        _model.ApprovalReason = new string('a', 19);

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(ApprovePromotionModel.ApprovalReason),
            "The length of 'Approval Reason' must be at least 20 characters. You entered 19 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_ApprovalReason_WillFail_WhenApprovalReason_IsTooLong()
    {
        _model.ApprovalReason = new string('a', 20_001);

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(ApprovePromotionModel.ApprovalReason),
            "The length of 'Approval Reason' must be 20000 characters or fewer. You entered 20001 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterDoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))!
            .Returns(Task.FromResult((Character)null!));

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(ApprovePromotionModel.CharacterId),
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
            nameof(ApprovePromotionModel.FactionLevelId),
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
            nameof(ApprovePromotionModel.CharacterId),
            "Character does not have a faction."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenFactionLevelDoesNotBelongToCharactersFaction()
    {
        _factionLevel.FactionId = 999;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(ApprovePromotionModel.FactionLevelId),
            "This faction level does not belong to the character's faction."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenFactionLevelIsBasicRank()
    {
        _factionLevel.FactionRankId = FactionRankEnum.Basic.Value;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(ApprovePromotionModel.FactionLevelId),
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
            nameof(ApprovePromotionModel.FactionLevelId),
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
            nameof(ApprovePromotionModel.FactionLevelId),
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
            nameof(ApprovePromotionModel.FactionLevelId),
            "Character does not have a previous rank approved."
        );
    }

    [Fact]
    public async Task ValidationFor_FactionLevelId_WillFail_WhenCharacterDoesNotHaveFactionPrerequisites()
    {
        A.CallTo(() =>
                _knowledgeRepository.HasFactionPrerequisites(
                    _model.CharacterId,
                    _factionLevel.KnowledgeId!.Value,
                    _factionLevel.KnowledgeLevelId!.Value,
                    _factionLevel.Specialization!
                )
            )
            .Returns(false);

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(ApprovePromotionModel.FactionLevelId),
            "Character does not have one or more of the required knowledge, knowledge level, or specialization for this faction level."
        );
    }

    [Fact]
    public async Task UseCase_WillApprovePromotion()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _characterFactionRepository.AddCharacterFactionMapping(
                    A<CharacterFactionMapping>.That.Matches(mapping =>
                        mapping.CharacterId == _model.CharacterId
                        && mapping.FactionLevelId == _model.FactionLevelId
                        && mapping.ApprovalDate == _approvalDate
                        && mapping.ApprovedByUserId == _userId
                        && mapping.ApprovalReason == _model.ApprovalReason
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillEditExistingPromotionRequest()
    {
        var existingMapping = new CharacterFactionMapping()
        {
            CharacterId = _model.CharacterId,
            FactionLevelId = _model.FactionLevelId,
            RequestPromotion = true,
            RequestReason = "Please promote me.",
        };

        A.CallTo(() =>
                _characterFactionRepository.GetCharacterFactionMapping(
                    _model.CharacterId,
                    _model.FactionLevelId
                )
            )
            .Returns(existingMapping);

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _characterFactionRepository.EditAsync(
                    A<CharacterFactionMapping>.That.Matches(mapping =>
                        mapping.CharacterId == _model.CharacterId
                        && mapping.FactionLevelId == _model.FactionLevelId
                        && mapping.ApprovalDate == _approvalDate
                        && mapping.ApprovedByUserId == _userId
                        && mapping.ApprovalReason == _model.ApprovalReason
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillNotAddPromotion_WhenExistingPromotionRequestIsEdited()
    {
        var existingMapping = new CharacterFactionMapping()
        {
            CharacterId = _model.CharacterId,
            FactionLevelId = _model.FactionLevelId,
            RequestPromotion = true,
            RequestReason = "Please promote me.",
        };

        A.CallTo(() =>
                _characterFactionRepository.GetCharacterFactionMapping(
                    _model.CharacterId,
                    _model.FactionLevelId
                )
            )
            .Returns(existingMapping);

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _characterFactionRepository.AddCharacterFactionMapping(
                    A<CharacterFactionMapping>._
                )
            )
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillReturn_Success_IfSuccessful()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
    }
}
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;
using ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.GetFactions;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.CharacterFaction;

public class GetCharacterFactionLevelsUseCaseTests
{
    private readonly GetCharacterFactionLevelsUseCase _useCase;
    private readonly ICharacterFactionRepository _characterFactionRepository;
    private readonly ICharacterKnowledgeRepository _knowledgeRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly GetCharacterFactionLevelsModel _model;
    private readonly Character _character;

    public GetCharacterFactionLevelsUseCaseTests()
    {
        _model = new GetCharacterFactionLevelsModel() { CharacterId = 1 };

        _character = new Character() { Id = _model.CharacterId, ExpressionId = 3 };

        _characterFactionRepository = A.Fake<ICharacterFactionRepository>();
        _knowledgeRepository = A.Fake<ICharacterKnowledgeRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(_character);

        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns(new List<CharacterFactionDto>());

        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(new List<BasicFactionLevelProjection>());

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(new List<SimpleCharacterKnowledgeProjection>());

        var validator = new GetCharacterFactionLevelsModelValidator();

        _useCase = new GetCharacterFactionLevelsUseCase(
            _characterFactionRepository,
            _knowledgeRepository,
            _characterRepository,
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
            nameof(GetCharacterFactionLevelsModel.CharacterId),
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
            nameof(GetCharacterFactionLevelsModel.CharacterId),
            "Character Id does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillReturn_FactionLevels()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new() { Id = 7, FactionRankId = FactionRankEnum.Basic.Value },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.Equal(7, factionLevel.FactionLevelId);
    }

    [Fact]
    public async Task UseCase_WillReturn_PlayerFactionLevelDetails_WhenCharacterHasFactionLevel()
    {
        var approvalDate = DateTimeOffset.UtcNow;

        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new() { Id = 7, FactionRankId = FactionRankEnum.Basic.Value },
                }
            );

        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns(
                new List<CharacterFactionDto>()
                {
                    new()
                    {
                        FactionLevelId = 7,
                        Approver = "Approver Name",
                        ApprovalReason = "Approved because reasons.",
                        RequestedPromotion = true,
                        RequestedPromotionReason = "Please promote me.",
                        ApprovalDate = approvalDate,
                        CharacterNotes = "Character notes.",
                    },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.Equal(7, factionLevel.FactionLevelId);
        Assert.Equal("Approver Name", factionLevel.Approver);
        Assert.Equal("Approved because reasons.", factionLevel.ApprovalReason);
        Assert.True(factionLevel.RequestedPromotion);
        Assert.Equal("Please promote me.", factionLevel.RequestedPromotionReason);
        Assert.Equal(approvalDate, factionLevel.ApprovalDate);
        Assert.Equal("Character notes.", factionLevel.CharacterNotes);
    }

    [Fact]
    public async Task UseCase_WillNotReturn_PlayerFactionLevelDetails_WhenCharacterDoesNotHaveFactionLevel()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new() { Id = 7, FactionRankId = FactionRankEnum.Basic.Value },
                }
            );

        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns(
                new List<CharacterFactionDto>()
                {
                    new()
                    {
                        FactionLevelId = 8,
                        Approver = "Approver Name",
                        ApprovalReason = "Approved because reasons.",
                        RequestedPromotion = true,
                        RequestedPromotionReason = "Please promote me.",
                        ApprovalDate = DateTimeOffset.UtcNow,
                        CharacterNotes = "Character notes.",
                    },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.Equal(7, factionLevel.FactionLevelId);
        Assert.Null(factionLevel.Approver);
        Assert.Null(factionLevel.ApprovalReason);
        Assert.False(factionLevel.RequestedPromotion);
        Assert.Null(factionLevel.RequestedPromotionReason);
        Assert.Null(factionLevel.ApprovalDate);
        Assert.Null(factionLevel.CharacterNotes);
    }

    [Fact]
    public async Task UseCase_WillReturn_HasKnowledge_AsFalse_WhenCharacterDoesNotHaveKnowledge()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new()
                    {
                        Id = 7,
                        KnowledgeId = 3,
                        FactionRankId = FactionRankEnum.Intermediate.Value,
                    },
                }
            );

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(new List<SimpleCharacterKnowledgeProjection>() { new() { Id = 4 } });

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.False(factionLevel.HasKnowledge);
    }

    [Fact]
    public async Task UseCase_WillReturn_HasKnowledge_AsTrue_WhenCharacterHasKnowledge()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new()
                    {
                        Id = 7,
                        KnowledgeId = 3,
                        FactionRankId = FactionRankEnum.Intermediate.Value,
                    },
                }
            );

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(new List<SimpleCharacterKnowledgeProjection>() { new() { Id = 3 } });

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.True(factionLevel.HasKnowledge);
    }

    [Fact]
    public async Task UseCase_WillReturn_HasKnowledgeLevel_AsFalse_WhenCharacterDoesNotHaveRequiredKnowledgeLevel()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new()
                    {
                        Id = 7,
                        KnowledgeId = 3,
                        KnowledgeLevel = 2,
                        FactionRankId = FactionRankEnum.Intermediate.Value,
                    },
                }
            );

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(
                new List<SimpleCharacterKnowledgeProjection>()
                {
                    new() { Id = 3, Level = 1 },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.False(factionLevel.HasKnowledgeLevel);
    }

    [Fact]
    public async Task UseCase_WillReturn_HasKnowledgeLevel_AsTrue_WhenCharacterHasRequiredKnowledgeLevel()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new()
                    {
                        Id = 7,
                        KnowledgeId = 3,
                        KnowledgeLevel = 2,
                        FactionRankId = FactionRankEnum.Intermediate.Value,
                    },
                }
            );

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(
                new List<SimpleCharacterKnowledgeProjection>()
                {
                    new() { Id = 3, Level = 2 },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.True(factionLevel.HasKnowledgeLevel);
    }

    [Fact]
    public async Task UseCase_WillReturn_HasKnowledgeLevel_AsTrue_WhenCharacterHasHigherThanRequiredKnowledgeLevel()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new()
                    {
                        Id = 7,
                        KnowledgeId = 3,
                        KnowledgeLevel = 2,
                        FactionRankId = FactionRankEnum.Intermediate.Value,
                    },
                }
            );

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(
                new List<SimpleCharacterKnowledgeProjection>()
                {
                    new() { Id = 3, Level = 3 },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.True(factionLevel.HasKnowledgeLevel);
    }

    [Fact]
    public async Task UseCase_WillReturn_HasKnowledgeLevel_AsFalse_ForBasicFactionRank()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new()
                    {
                        Id = 7,
                        KnowledgeId = 3,
                        KnowledgeLevel = 1,
                        FactionRankId = FactionRankEnum.Basic.Value,
                    },
                }
            );

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(
                new List<SimpleCharacterKnowledgeProjection>()
                {
                    new() { Id = 3, Level = 3 },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.False(factionLevel.HasKnowledgeLevel);
    }

    [Fact]
    public async Task UseCase_WillReturn_HasSpecialization_AsFalse_WhenFactionLevelDoesNotRequireSpecialization()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new()
                    {
                        Id = 7,
                        KnowledgeId = 3,
                        Specialization = null,
                        FactionRankId = FactionRankEnum.Intermediate.Value,
                    },
                }
            );

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(
                new List<SimpleCharacterKnowledgeProjection>()
                {
                    new()
                    {
                        Id = 3,
                        Specializations = new List<string>() { "Alchemy" },
                    },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.False(factionLevel.HasSpecialization);
    }

    [Fact]
    public async Task UseCase_WillReturn_HasSpecialization_AsFalse_WhenCharacterDoesNotHaveRequiredSpecialization()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new()
                    {
                        Id = 7,
                        KnowledgeId = 3,
                        Specialization = "Cooking",
                        FactionRankId = FactionRankEnum.Intermediate.Value,
                    },
                }
            );

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(
                new List<SimpleCharacterKnowledgeProjection>()
                {
                    new()
                    {
                        Id = 3,
                        Specializations = new List<string>() { "Alchemy" },
                    },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.False(factionLevel.HasSpecialization);
    }

    [Fact]
    public async Task UseCase_WillReturn_HasSpecialization_AsTrue_WhenCharacterHasRequiredSpecialization()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new()
                    {
                        Id = 7,
                        KnowledgeId = 3,
                        Specialization = "Cooking",
                        FactionRankId = FactionRankEnum.Intermediate.Value,
                    },
                }
            );

        A.CallTo(() => _knowledgeRepository.GetSimpleKnowledgesForCharacter(_model.CharacterId))
            .Returns(
                new List<SimpleCharacterKnowledgeProjection>()
                {
                    new()
                    {
                        Id = 3,
                        Specializations = new List<string>() { "Alchemy", "Cooking" },
                    },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        var factionLevel = Assert.Single(result.Value.FactionLevels);
        Assert.True(factionLevel.HasSpecialization);
    }

    [Fact]
    public async Task UseCase_WillReturn_AllFactionLevels()
    {
        A.CallTo(() => _characterFactionRepository.GetFactionLevels(_model.CharacterId))
            .Returns(
                new List<BasicFactionLevelProjection>()
                {
                    new() { Id = 7, FactionRankId = FactionRankEnum.Basic.Value },
                    new() { Id = 8, FactionRankId = FactionRankEnum.Intermediate.Value },
                }
            );

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(2, result.Value.FactionLevels.Count);
        Assert.Contains(result.Value.FactionLevels, x => x.FactionLevelId == 7);
        Assert.Contains(result.Value.FactionLevels, x => x.FactionLevelId == 8);
    }
}

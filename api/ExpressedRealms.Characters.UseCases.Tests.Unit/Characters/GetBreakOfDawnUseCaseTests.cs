using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Proficiencies;
using ExpressedRealms.Characters.Repository.Proficiencies.DTOs;
using ExpressedRealms.Characters.Repository.Proficiencies.Enums;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.UseCases.Characters.GetBreakOfDawnInfo;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Characters;

public class GetBreakOfDawnUseCaseTests
{
    private readonly GetBreakOfDawnInfoUseCase _useCaseTests;
    private readonly ICharacterRepository _characterRepository;
    private readonly IProficiencyRepository _proficiencyRepository;
    private readonly IXpRepository _xpRepository;
    private readonly GetBreakOfDawnInfoModel _model;

    public GetBreakOfDawnUseCaseTests()
    {
        _model = new GetBreakOfDawnInfoModel { Id = 4 };

        _characterRepository = A.Fake<ICharacterRepository>();
        _proficiencyRepository = A.Fake<IProficiencyRepository>();
        _xpRepository = A.Fake<IXpRepository>();

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id)).Returns(new Character());

        A.CallTo(() => _characterRepository.GetCharacterExpressionId(_model.Id)).Returns(3);

        A.CallTo(() => _xpRepository.GetCharacterXpLevel(_model.Id)).Returns(5);

        A.CallTo(() => _proficiencyRepository.GetBasicProficiencies(_model.Id))
            .Returns(
                new List<ProficiencyDto>
                {
                    new()
                    {
                        Id = 13,
                        Type = "Type",
                        AppliedModifiers = new List<ModifierDescription>
                        {
                            new()
                            {
                                Message = "test",
                                Value = 11,
                                Type = ModifierType.Vitality,
                                Name = "test",
                            },
                        },
                    },
                    new()
                    {
                        Id = 14,
                        Type = "Type",
                        AppliedModifiers = new List<ModifierDescription>
                        {
                            new()
                            {
                                Message = "test",
                                Value = 12,
                                Type = ModifierType.Vitality,
                                Name = "test",
                            },
                        },
                    },
                    new()
                    {
                        Id = 15,
                        Type = "Type",
                        AppliedModifiers = new List<ModifierDescription>
                        {
                            new()
                            {
                                Message = "test",
                                Value = 13,
                                Type = ModifierType.Vitality,
                                Name = "test",
                            },
                        },
                    },
                    new()
                    {
                        Id = 17,
                        Type = "Type",
                        AppliedModifiers = new List<ModifierDescription>
                        {
                            new()
                            {
                                Message = "test",
                                Value = 14,
                                Type = ModifierType.Vitality,
                                Name = "test",
                            },
                        },
                    },
                    new()
                    {
                        Id = 22,
                        Type = "Type",
                        AppliedModifiers = new List<ModifierDescription>
                        {
                            new()
                            {
                                Message = "test",
                                Value = 15,
                                Type = ModifierType.Vitality,
                                Name = "test",
                            },
                        },
                    },
                    new()
                    {
                        Id = 23,
                        Type = "Type",
                        AppliedModifiers = new List<ModifierDescription>
                        {
                            new()
                            {
                                Message = "test",
                                Value = 16,
                                Type = ModifierType.Vitality,
                                Name = "test",
                            },
                        },
                    },
                }
            );

        var validator = new GetBreakOfDawnInfoModelValidator(_characterRepository);

        _useCaseTests = new GetBreakOfDawnInfoUseCase(
            _characterRepository,
            _proficiencyRepository,
            _xpRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.Id = 0;

        var results = await _useCaseTests.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(GetBreakOfDawnInfoModel.Id),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenIt_DoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.Id))
            .Returns(Task.FromResult<Character?>(null));

        var results = await _useCaseTests.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(GetBreakOfDawnInfoModel.Id),
            "The Character Id does not exist."
        );
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(3, 1)]
    [InlineData(4, 3)]
    [InlineData(7, 4)]
    [InlineData(8, 5)]
    [InlineData(9, 6)]
    public async Task UseCase_Maps_All_ExpressionIds_Correctly(
        int repositoryExpressionId,
        int expectedExpressionId
    )
    {
        A.CallTo(() => _characterRepository.GetCharacterExpressionId(_model.Id))
            .Returns(repositoryExpressionId);

        var results = await _useCaseTests.ExecuteAsync(_model);

        Assert.True(results.IsSuccess);
        Assert.Equal(expectedExpressionId, results.Value.ExpressionId);
    }

    [Fact]
    public async Task UseCase_Returns_BreakOfDawnInfo_With_Mapped_Proficiencies()
    {
        var results = await _useCaseTests.ExecuteAsync(_model);

        Assert.True(results.IsSuccess);
        Assert.Equal(11, results.Value.Vitality);
        Assert.Equal(12, results.Value.Health);
        Assert.Equal(13, results.Value.Blood);
        Assert.Equal(15, results.Value.Rwp);
        Assert.Equal(14, results.Value.Psyche);
        Assert.Equal(16, results.Value.Mortis);
        Assert.Equal(5, results.Value.CharacterLevel);
    }
}

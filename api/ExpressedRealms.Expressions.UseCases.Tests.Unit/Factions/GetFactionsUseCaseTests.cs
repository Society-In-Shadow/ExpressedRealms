using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.Repository.Factions.Dtos;
using ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFactions;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.Factions;

public class GetFactionsUseCaseTests
{
    private readonly GetFactionsUseCase _useCase;
    private readonly IFactionRepository _factionRepository;
    private readonly IPowerRepository _powerRepository;

    private readonly GetFactionsModel _model;

    public GetFactionsUseCaseTests()
    {
        _factionRepository = A.Fake<IFactionRepository>();
        _powerRepository = A.Fake<IPowerRepository>();

        _model = new GetFactionsModel() { ExpressionId = 1 };

        var validator = new GetFactionsModelValidator();

        _useCase = new GetFactionsUseCase(
            _factionRepository,
            _powerRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_ExpressionId_WillFail_WhenItIsEmpty()
    {
        _model.ExpressionId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetFactionsModel.ExpressionId),
            "Expression Id is required."
        );
    }

    [Fact]
    public async Task UseCase_Grabs_TheFactions()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _factionRepository.GetFactions(_model.ExpressionId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_Grabs_ThePowers_ForFactionLevelsWithPowerIds()
    {
        var listTest = new List<FactionDto>()
        {
            new()
            {
                Id = 1,
                Name = "Washington",
                Background = "Money Market Account",
                Levels = new List<FactionLevelListDto>()
                {
                    new()
                    {
                        RankName = "Basic",
                        KnowledgeLevel = "Foo",
                        Knowledge = "Goo",
                        KnowledgeLevelId = 1,
                        Id = 5,
                        Specialization = "Bar",
                        KnowledgeId = 3,
                        PowerId = 8,
                    },
                },
            },
        };

        A.CallTo(() => _factionRepository.GetFactions(_model.ExpressionId)).Returns(listTest);

        await _useCase.ExecuteAsync(_model);
        var powerIds = new List<int>() { 8 };
        A.CallTo(() => _powerRepository.GetPowers(powerIds)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_Returns_ListOfFactions()
    {
        var power = new PowerInformation()
        {
            Id = 8,
            Name = "Power Name",
            AreaOfEffect = new DetailedInformation(1, "Foo", "Bar"),
            PowerDuration = new DetailedInformation(2, "Foo1", "Bar1"),
            PowerLevel = new DetailedInformation(4, "Foo2", "Bar2"),
            PowerActivationType = new DetailedInformation(3, "Foo3", "Bar3"),
            Description = "Power Description",
            GameMechanicEffect = "Power Effect",
            Limitation = "Power Limitation",
            Other = "Other Power Info",
            IsPowerUse = true,
            Cost = "1",
        };

        var listTest = new List<FactionDto>()
        {
            new()
            {
                Id = 1,
                Name = "Washington",
                Background = "Money Market Account",
                Levels = new List<FactionLevelListDto>()
                {
                    new()
                    {
                        RankName = "Basic",
                        KnowledgeLevel = "Foo",
                        Knowledge = "Goo",
                        KnowledgeLevelId = 1,
                        Id = 5,
                        Specialization = "Bar",
                        KnowledgeId = 3,
                        PowerId = 8,
                    },
                },
            },
            new()
            {
                Id = 2,
                Name = "Human",
                Background = "Norwegian Krone",
            },
        };

        var expectedReturn = new FactionsReturnModel()
        {
            Factions = new List<FactionModel>()
            {
                new()
                {
                    Id = 1,
                    Name = "Washington",
                    Background = "Money Market Account",
                    FactionLevels = new List<FactionLevelModel>()
                    {
                        new()
                        {
                            RankName = "Basic",
                            KnowledgeLevel = "Foo",
                            Knowledge = "Goo",
                            KnowledgeLevelId = 1,
                            Id = 5,
                            Specialization = "Bar",
                            KnowledgeId = 3,
                            Power = power,
                        },
                    },
                },
                new()
                {
                    Id = 2,
                    Name = "Human",
                    Background = "Norwegian Krone",
                },
            },
        };

        var powerIds = new List<int>() { 8 };
        A.CallTo(() => _factionRepository.GetFactions(_model.ExpressionId)).Returns(listTest);
        A.CallTo(() => _powerRepository.GetPowers(powerIds)).Returns([power]);

        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(expectedReturn.Factions, results.Value.Factions);
    }
}

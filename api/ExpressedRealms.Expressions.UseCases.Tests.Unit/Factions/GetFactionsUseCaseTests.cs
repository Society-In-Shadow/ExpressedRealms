using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFactions;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.Factions;

public class GetFactionsUseCaseTests
{
    private readonly GetFactionsUseCase _useCase;
    private readonly IFactionRepository _factionRepository;

    private readonly GetFactionsModel _model;

    public GetFactionsUseCaseTests()
    {
        _factionRepository = A.Fake<IFactionRepository>();

        _model = new GetFactionsModel() { ExpressionId = 1 };

        var validator = new GetFactionsModelValidator();

        _useCase = new GetFactionsUseCase(_factionRepository, validator, CancellationToken.None);
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
    public async Task UseCase_Returns_ListOfFactions()
    {
        var listTest = new List<Faction>()
        {
            new()
            {
                Id = 1,
                Name = "Washington",
                Background = "Money Market Account",
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
                },
                new()
                {
                    Id = 2,
                    Name = "Human",
                    Background = "Norwegian Krone",
                },
            },
        };

        A.CallTo(() => _factionRepository.GetFactions(_model.ExpressionId)).Returns(listTest);

        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(expectedReturn.Factions, results.Value.Factions);
    }
}

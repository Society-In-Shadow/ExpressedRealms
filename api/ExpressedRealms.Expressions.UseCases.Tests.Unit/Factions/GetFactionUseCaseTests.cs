using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFaction;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.Factions;

public class GetFactionUseCaseTests
{
    private readonly GetFactionUseCase _useCase;
    private readonly IFactionRepository _factionRepository;
    private readonly GetFactionModel _model;

    private Faction FactionDbModel { get; set; }

    public GetFactionUseCaseTests()
    {
        _model = new GetFactionModel() { Id = 4 };
        FactionDbModel = new Faction()
        {
            Name = "parse",
            Background = "View",
            ExpressionId = 1,
        };

        _factionRepository = A.Fake<IFactionRepository>();

        A.CallTo(() => _factionRepository.GetFactionAsync(_model.Id)).Returns(FactionDbModel);

        var validator = new GetFactionModelValidator(_factionRepository);

        _useCase = new GetFactionUseCase(_factionRepository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(GetFactionModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenFactionDoesNotExist()
    {
        A.CallTo(() => _factionRepository.GetFactionAsync(_model.Id))!
            .Returns(Task.FromResult((Faction)null!));
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(GetFactionModel.Id), "This Faction was not found.");
    }

    [Fact]
    public async Task UseCase_Grabs_TheFaction()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _factionRepository.GetFactionAsync(_model.Id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_Returns_AllPropertiesForTheFaction()
    {
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equal(FactionDbModel.Name, results.Value.Name);
        Assert.Equal(FactionDbModel.Background, results.Value.Background);
    }
}

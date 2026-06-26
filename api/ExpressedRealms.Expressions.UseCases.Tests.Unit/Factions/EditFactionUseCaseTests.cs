using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.UseCases.FactionUseCases.CreateFaction;
using ExpressedRealms.Expressions.UseCases.FactionUseCases.EditFaction;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.Factions;

public class EditFactionUseCaseTests
{
    private readonly EditFactionUseCase _useCase;
    private readonly IFactionRepository _factionRepository;
    private readonly EditFactionModel _model;
    private readonly Faction _dbModel;

    public EditFactionUseCaseTests()
    {
        _model = new EditFactionModel()
        {
            Id = 1,
            Name = "parse",
            Background = "View",
        };

        _dbModel = new Faction()
        {
            Name = "Intelligent Metal Towels",
            Background = "Engineer",
            ExpressionId = 1,
        };

        _factionRepository = A.Fake<IFactionRepository>();

        A.CallTo(() => _factionRepository.GetFactionForEditingAsync(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _factionRepository.HasDuplicateName(_model.Name)).Returns(false);

        var validator = new EditFactionModelValidator();

        _useCase = new EditFactionUseCase(_factionRepository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditFactionModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenFactionDoesNotExist()
    {
        A.CallTo(() => _factionRepository.GetFactionForEditingAsync(_model.Id))!
            .Returns(Task.FromResult((Faction)null!));
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(EditFactionModel.Id), "This Faction was not found.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsEmpty()
    {
        _model.Name = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditFactionModel.Name), "Name is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsOver250Characters()
    {
        _model.Name = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditFactionModel.Name),
            "Name cannot exceed 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsADuplicateName()
    {
        A.CallTo(() => _factionRepository.HasDuplicateName(_model.Name, _model.Id)).Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateFactionModel.Name),
            "This name already exists."
        );
    }

    [Fact]
    public async Task ValidationFor_Background_WillFail_WhenBackground_IsEmpty()
    {
        _model.Background = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditFactionModel.Background),
            "Background is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Background_WillFail_WhenBackground_IsOver20000Characters()
    {
        _model.Background = new string('x', 20001);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditFactionModel.Background),
            "Background cannot exceed 20000 characters."
        );
    }

    [Fact]
    public async Task UseCase_WillGrab_TheFaction()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _factionRepository.GetFactionForEditingAsync(_model.Id))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_PassesThrough_TheDbFaction()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _factionRepository.EditFactionAsync(A<Faction>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillEditTheFaction()
    {
        var faction = new Faction() { Name = _model.Name, Background = _model.Background };

        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _factionRepository.EditFactionAsync(
                    A<Faction>.That.Matches(k =>
                        k.Name == faction.Name && k.Background == faction.Background
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

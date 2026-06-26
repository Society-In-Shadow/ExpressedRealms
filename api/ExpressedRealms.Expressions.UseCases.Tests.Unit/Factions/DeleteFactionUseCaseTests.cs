using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.Expressions.Repository.Factions;
using ExpressedRealms.Expressions.UseCases.FactionUseCases.DeleteFaction;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.Factions;

public class DeleteFactionUseCaseTests
{
    private readonly DeleteFactionUseCase _useCase;
    private readonly IFactionRepository _repository;
    private readonly DeleteFactionModel _model;

    public DeleteFactionUseCaseTests()
    {
        _model = new DeleteFactionModel() { Id = 4 };

        _repository = A.Fake<IFactionRepository>();

        A.CallTo(() => _repository.GetFactionForEditingAsync(_model.Id))
            .Returns(new Faction() { Id = _model.Id });

        var validator = new DeleteFactionModelValidator();

        _useCase = new DeleteFactionUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteFactionModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenFactionDoesNotExist()
    {
        A.CallTo(() => _repository.GetFactionForEditingAsync(_model.Id))!
            .Returns(Task.FromResult((Faction)null!));
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(DeleteFactionModel.Id), "This Faction was not found.");
    }

    [Fact]
    public async Task UseCase_WillGrab_TheFaction()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetFactionForEditingAsync(_model.Id))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillSoftDelete_TheFaction()
    {
        var faction = new Faction() { Id = _model.Id, IsDeleted = true };

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _repository.EditFactionAsync(
                    A<Faction>.That.Matches(k =>
                        k.Id == faction.Id && k.IsDeleted == faction.IsDeleted
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.UseCases.Blessings.DeleteBlessing;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit;

public class DeleteBlessingUseCaseTests
{
    private readonly DeleteBlessingUseCase _useCase;
    private readonly IBlessingRepository _repository;
    private readonly DeleteBlessingModel _model;
    private readonly Blessing _dbModel;

    public DeleteBlessingUseCaseTests()
    {
        _model = new DeleteBlessingModel() { Id = 3 };

        _dbModel = new Blessing()
        {
            Name = "asdf",
            Description = "qwer",
            SubCategory = "jkl;",
            Type = "yuio",
        };

        _repository = A.Fake<IBlessingRepository>();

        A.CallTo(() => _repository.GetBlessingForEditing(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _repository.IsExistingBlessing(_model.Id)).Returns(true);

        var validator = new DeleteBlessingModelValidator(_repository);

        _useCase = new DeleteBlessingUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItIsEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteBlessingModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingBlessing(_model.Id)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(DeleteBlessingModel.Id),
            "This blessing was not found."
        );
    }

    [Fact]
    public async Task UseCase_WillGrab_TheBlessing()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.GetBlessingForEditing(_model.Id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_PassesThrough_TheDbBlessing()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.EditBlessingAsync(A<Blessing>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillDeleteTheBlessing()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditBlessingAsync(
                    A<Blessing>.That.Matches(k =>
                        k.Id == _dbModel.Id && k.IsDeleted == _dbModel.IsDeleted
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

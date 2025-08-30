using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.UseCases.Blessings.EditBlessings;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit;

public class EditBlessingUseCaseTests
{
    private readonly EditBlessingUseCase _useCase;
    private readonly IBlessingRepository _repository;
    private readonly EditBlessingModel _model;
    private readonly Blessing _dbModel;

    public EditBlessingUseCaseTests()
    {
        _model = new EditBlessingModel()
        {
            Id = 3,
            Name = "Test Blessing",
            Description = "Test Description",
            SubCategory = "Mental",
            Type = "Advantage",
        };

        _dbModel = new Blessing()
        {
            Name = "asdf",
            Description = "qwer",
            SubCategory = "jkl;",
            Type = "yuio",
        };

        _repository = A.Fake<IBlessingRepository>();

        A.CallTo(() => _repository.GetBlessing(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _repository.IsExistingBlessing(_model.Id)).Returns(true);
        A.CallTo(() => _repository.HasDuplicateName(_model.Name)).Returns(false);

        var validator = new EditBlessingModelValidator(_repository);

        _useCase = new EditBlessingUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItIsEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditBlessingModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingBlessing(_model.Id)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditBlessingModel.Id), "Blessing does not exist.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsEmpty()
    {
        _model.Name = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditBlessingModel.Name), "Name is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsOver150Characters()
    {
        _model.Name = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingModel.Name),
            "Name must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_AlreadyExists()
    {
        A.CallTo(() => _repository.HasDuplicateName(_model.Name)).Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingModel.Name),
            "Blessing with this name already exists."
        );
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItsEmpty()
    {
        _model.Description = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingModel.Description),
            "Description is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Type_WillFail_WhenItIs_IsEmpty()
    {
        _model.Type = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditBlessingModel.Type), "Type is required.");
    }

    [Fact]
    public async Task ValidationFor_Type_WillFail_WhenItIs_IsOver50Characters()
    {
        _model.Type = new string('x', 51);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingModel.Type),
            "Type must be between 1 and 50 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_SubCategory_WillFail_WhenItIs_IsEmpty()
    {
        _model.SubCategory = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingModel.SubCategory),
            "Sub Category is required."
        );
    }

    [Fact]
    public async Task ValidationFor_SubCategory_WillFail_WhenItIs_IsOver75Characters()
    {
        _model.SubCategory = new string('x', 76);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingModel.SubCategory),
            "Sub Category must be between 1 and 75 characters."
        );
    }

    [Fact]
    public async Task UseCase_WillGrab_TheBlessing()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.GetBlessing(_model.Id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_PassesThrough_TheDbBlessing()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.EditBlessingAsync(A<Blessing>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillEditTheBlessing()
    {
        var blessing = new Blessing()
        {
            Name = _model.Name,
            Description = _model.Description,
            SubCategory = _model.SubCategory,
            Type = _model.Type,
        };

        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditBlessingAsync(
                    A<Blessing>.That.Matches(k =>
                        k.Name == blessing.Name
                        && k.Description == blessing.Description
                        && k.Type == blessing.Type
                        && k.SubCategory == blessing.SubCategory
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

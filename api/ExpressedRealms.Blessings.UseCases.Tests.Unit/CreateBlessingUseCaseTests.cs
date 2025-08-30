using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.UseCases.Blessings.CreateBlessings;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit;

public class CreateBlessingUseCaseTests
{
    private readonly CreateBlessingUseCase _useCase;
    private readonly IBlessingRepository _repository;
    private readonly CreateBlessingModel _model;

    public CreateBlessingUseCaseTests()
    {
        _model = new CreateBlessingModel()
        {
            Name = "Test Blessing",
            Description = "Test Description",
            SubCategory = "Mental",
            Type = "Advantage",
        };

        _repository = A.Fake<IBlessingRepository>();

        A.CallTo(() => _repository.HasDuplicateName(_model.Name)).Returns(false);

        var validator = new CreateBlessingModelValidator(_repository);

        _useCase = new CreateBlessingUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsEmpty()
    {
        _model.Name = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateBlessingModel.Name), "Name is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsOver150Characters()
    {
        _model.Name = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingModel.Name),
            "Name must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_AlreadyExists()
    {
        A.CallTo(() => _repository.HasDuplicateName(_model.Name)).Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingModel.Name),
            "Blessing with this name already exists."
        );
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItsEmpty()
    {
        _model.Description = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingModel.Description),
            "Description is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Type_WillFail_WhenItIs_IsEmpty()
    {
        _model.Type = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateBlessingModel.Type), "Type is required.");
    }

    [Fact]
    public async Task ValidationFor_Type_WillFail_WhenItIs_IsOver50Characters()
    {
        _model.Type = new string('x', 51);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingModel.Type),
            "Type must be between 1 and 50 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_SubCategory_WillFail_WhenItIs_IsEmpty()
    {
        _model.SubCategory = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingModel.SubCategory),
            "Sub Category is required."
        );
    }

    [Fact]
    public async Task ValidationFor_SubCategory_WillFail_WhenItIs_IsOver75Characters()
    {
        _model.SubCategory = new string('x', 76);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingModel.SubCategory),
            "Sub Category must be between 1 and 75 characters."
        );
    }

    [Fact]
    public async Task UseCase_WillCreateTheBlessing()
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
                _repository.CreateBlessingAsync(
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

    [Fact]
    public async Task UseCase_WillReturn_BlessingId_IfSuccessful()
    {
        A.CallTo(() => _repository.CreateBlessingAsync(A<Blessing>._)).Returns(5);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.Equal(5, result.Value);
    }
}

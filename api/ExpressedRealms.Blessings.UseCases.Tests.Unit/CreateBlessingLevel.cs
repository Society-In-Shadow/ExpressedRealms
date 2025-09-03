using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.UseCases.BlessingLevels.CreateBlessingLevel;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit;

public class CreateBlessingLevelUseCaseTests
{
    private readonly CreateBlessingLevelUseCase _useCase;
    private readonly IBlessingRepository _repository;
    private readonly CreateBlessingLevelModel _model;

    public CreateBlessingLevelUseCaseTests()
    {
        _model = new CreateBlessingLevelModel()
        {
            Description = "Description",
            Level = "4 pt.",
            BlessingId = 2,
            XpCost = 4,
            XpGain = 0,
        };

        _repository = A.Fake<IBlessingRepository>();

        A.CallTo(() => _repository.HasDuplicateLevelName(_model.BlessingId, _model.Level))
            .Returns(false);
        A.CallTo(() => _repository.IsExistingBlessing(_model.BlessingId)).Returns(true);

        var validator = new CreateBlessingLevelModelValidator(_repository);

        _useCase = new CreateBlessingLevelUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Level_WillFail_WhenName_IsEmpty()
    {
        _model.Level = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingLevelModel.Level),
            "Level is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsOver25Characters()
    {
        _model.Level = new string('x', 26);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingLevelModel.Level),
            "Name must be between 1 and 25 characters."
        );
    }

    [Theory]
    [InlineData("123 pst.")]
    [InlineData("5 ptss. ")]
    [InlineData("asdf.")]
    [InlineData("pts.")]
    [InlineData("pt.")]
    public async Task ValidationFor_Name_WillFail_WhenName_IsInIncorrectFormat(string levelName)
    {
        _model.Level = levelName;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingLevelModel.Level),
            "Level must be in the format of '123 pts.' or '1 pt.'"
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_AlreadyExists()
    {
        A.CallTo(() => _repository.HasDuplicateLevelName(_model.BlessingId, _model.Level))
            .Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingLevelModel.Level),
            "Blessing already has a level with this name."
        );
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItsEmpty()
    {
        _model.Description = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingLevelModel.Description),
            "Description is required."
        );
    }

    [Fact]
    public async Task ValidationFor_XpCost_WillFail_ItIsLessThen0()
    {
        _model.XpCost = -1;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingLevelModel.XpCost),
            "Xp Cost must be greater than or equal to 0."
        );
    }

    [Fact]
    public async Task ValidationFor_XpGain_WillFail_ItIsLessThen0()
    {
        _model.XpGain = -1;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingLevelModel.XpGain),
            "Xp Gain must be greater than or equal to 0."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingId_WillFail_WhenItIs_IsEmpty()
    {
        _model.BlessingId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingLevelModel.BlessingId),
            "Blessing Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingBlessing(_model.BlessingId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateBlessingLevelModel.BlessingId),
            "Blessing Id does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillCreateTheBlessing()
    {
        var blessing = new BlessingLevel()
        {
            Level = _model.Level,
            Description = _model.Description,
            XpCost = _model.XpCost,
            XpGain = _model.XpGain,
        };

        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.CreateBlessingLevelAsync(
                    A<BlessingLevel>.That.Matches(k =>
                        k.Level == blessing.Level
                        && k.XpCost == blessing.XpCost
                        && k.XpGain == blessing.XpGain
                        && k.Description == blessing.Description
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_BlessingId_IfSuccessful()
    {
        A.CallTo(() => _repository.CreateBlessingLevelAsync(A<BlessingLevel>._)).Returns(5);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.Equal(5, result.Value);
    }
}

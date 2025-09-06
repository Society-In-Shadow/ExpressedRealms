using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.UseCases.BlessingLevels.EditBlessingLevel;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit;

public class EditBlessingLevelUseCaseTests
{
    private readonly EditBlessingLevelUseCase _useCase;
    private readonly IBlessingRepository _repository;
    private readonly EditBlessingLevelModel _model;

    private readonly BlessingLevel _dbModel;

    public EditBlessingLevelUseCaseTests()
    {
        _model = new EditBlessingLevelModel()
        {
            Description = "Description",
            Level = "4pts",
            BlessingId = 2,
            LevelId = 3,
            XpCost = 4,
            XpGain = 0,
        };

        _dbModel = new BlessingLevel()
        {
            BlessingId = 1,
            Id = 1,
            Level = "1pt",
            Description = "Description",
            XpCost = 3,
            XpGain = 2,
        };

        _repository = A.Fake<IBlessingRepository>();

        A.CallTo(() =>
                _repository.HasDuplicateLevelName(_model.BlessingId, _model.Level, _model.LevelId)
            )
            .Returns(false);
        A.CallTo(() => _repository.IsExistingBlessing(_model.BlessingId)).Returns(true);
        A.CallTo(() => _repository.GetBlessingLevelForEditing(_model.BlessingId, _model.LevelId))
            .Returns(_dbModel);
        A.CallTo(() => _repository.IsExistingBlessingLevel(_model.BlessingId, _model.LevelId))
            .Returns(true);

        var validator = new EditBlessingLevelModelValidator(_repository);

        _useCase = new EditBlessingLevelUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Level_WillFail_WhenName_IsEmpty()
    {
        _model.Level = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditBlessingLevelModel.Level), "Level is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsOver25Characters()
    {
        _model.Level = new string('x', 26);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.Level),
            "Name must be between 1 and 25 characters."
        );
    }

    [Theory]
    [InlineData("123 pst")]
    [InlineData("5 ptss ")]
    [InlineData("asdf.")]
    [InlineData("pts.")]
    [InlineData("pt.")]
    public async Task ValidationFor_Name_WillFail_WhenName_IsInIncorrectFormat(string levelName)
    {
        _model.Level = levelName;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.Level),
            "Level must be in the format of '123pts' or '1pt'"
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_AlreadyExists()
    {
        A.CallTo(() =>
                _repository.HasDuplicateLevelName(_model.BlessingId, _model.Level, _model.LevelId)
            )
            .Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.Level),
            "Blessing already has a level with this name."
        );
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItsEmpty()
    {
        _model.Description = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.Description),
            "Description is required."
        );
    }

    [Fact]
    public async Task ValidationFor_XpCost_WillFail_ItIsLessThen0()
    {
        _model.XpCost = -1;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.XpCost),
            "Xp Cost must be greater than or equal to 0."
        );
    }

    [Fact]
    public async Task ValidationFor_XpGain_WillFail_ItIsLessThen0()
    {
        _model.XpGain = -1;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.XpGain),
            "Xp Gain must be greater than or equal to 0."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingId_WillFail_WhenItIs_IsEmpty()
    {
        _model.BlessingId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.BlessingId),
            "Blessing Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_BlessingId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingBlessing(_model.BlessingId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.BlessingId),
            "Blessing Id does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_LevelId_WillFail_WhenItIs_IsEmpty()
    {
        _model.LevelId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.LevelId),
            "Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_LevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingBlessingLevel(_model.BlessingId, _model.LevelId))
            .Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditBlessingLevelModel.LevelId),
            "Level Id does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillPassThrough_TheEditObject()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.EditBlessingLevelAsync(A<BlessingLevel>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillEditTheBlessing()
    {
        var blessing = new BlessingLevel()
        {
            Level = _model.Level,
            Description = _model.Description,
            XpCost = _model.XpCost,
            XpGain = _model.XpGain,
        };

        var result = await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditBlessingLevelAsync(
                    A<BlessingLevel>.That.Matches(k =>
                        k.Level == blessing.Level
                        && k.XpCost == blessing.XpCost
                        && k.XpGain == blessing.XpGain
                        && k.Description == blessing.Description
                        && k.BlessingId == _dbModel.BlessingId
                        && k.Id == _dbModel.Id
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

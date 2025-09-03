using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.UseCases.BlessingLevels.GetBlessingLevel;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit;

public class GetBlessingLevelUseCaseTests
{
    private readonly GetBlessingLevelUseCase _useCase;
    private readonly IBlessingRepository _repository;
    private readonly GetBlessingLevelModel _model;
    
    private readonly BlessingLevel _dbModel;

    public GetBlessingLevelUseCaseTests()
    {
        _model = new GetBlessingLevelModel()
        {
            LevelId = 3
        };
        
        _dbModel = new BlessingLevel()
        {
            BlessingId = 1,
            Id = 6,
            Level = "1 pt.",
            Description = "Description",
            XpCost = 3,
            XpGain = 2,
        };

        _repository = A.Fake<IBlessingRepository>();

        A.CallTo(() => _repository.IsExistingBlessingLevel(_model.LevelId)).Returns(true);
        A.CallTo(() => _repository.GetBlessingLevelForEditing(_model.LevelId)).Returns(_dbModel);
        
        var validator = new GetBlessingLevelModelValidator(_repository);

        _useCase = new GetBlessingLevelUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_LevelId_WillFail_WhenItIs_IsEmpty()
    {
        _model.LevelId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetBlessingLevelModel.LevelId),
            "Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_LevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingBlessingLevel(_model.LevelId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetBlessingLevelModel.LevelId),
            "Level Id does not exist."
        );
    }


    [Fact]
    public async Task UseCase_WillCreateTheBlessing()
    {
        var level = new GetBlessingLevelReturnModel()
        {
            Level = _dbModel.Level,
            Description = _dbModel.Description,
            XpCost = _dbModel.XpCost,
            XpGain = _dbModel.XpGain,
            BlessingId = _dbModel.BlessingId,
            Id = _dbModel.Id,
        };

        var results = await _useCase.ExecuteAsync(_model);
        Assert.Equivalent(level, results.Value);
    }
}
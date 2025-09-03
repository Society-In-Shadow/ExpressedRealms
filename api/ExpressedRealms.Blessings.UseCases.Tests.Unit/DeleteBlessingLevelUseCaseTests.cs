using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.UseCases.BlessingLevels.DeleteBlessingLevel;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit;

public class DeleteBlessingLevelUseCaseTests
{
    private readonly DeleteBlessingLevelUseCase _useCase;
    private readonly IBlessingRepository _repository;
    private readonly DeleteBlessingLevelModel _model;

    private readonly BlessingLevel _dbModel;

    public DeleteBlessingLevelUseCaseTests()
    {
        _model = new DeleteBlessingLevelModel()
        {
            LevelId = 3,
        };

        _dbModel = new BlessingLevel()
        {
            BlessingId = 2,
            Id = 1,
        };

        _repository = A.Fake<IBlessingRepository>();

        A.CallTo(() => _repository.GetBlessingLevelForEditing(_model.LevelId)).Returns(_dbModel);
        A.CallTo(() => _repository.IsExistingBlessingLevel(_model.LevelId)).Returns(true);

        var validator = new DeleteBlessingLevelModelValidator(_repository);

        _useCase = new DeleteBlessingLevelUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_LevelId_WillFail_WhenItIs_IsEmpty()
    {
        _model.LevelId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(DeleteBlessingLevelModel.LevelId),
            "Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_LevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingBlessingLevel(_model.LevelId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(DeleteBlessingLevelModel.LevelId),
            "Level Id does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillPassThrough_TheDeleteObject()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.EditBlessingLevelAsync(A<BlessingLevel>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillDeleteTheBlessing()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditBlessingLevelAsync(
                    A<BlessingLevel>.That.Matches(k =>
                        k.IsDeleted == true
                        && k.BlessingId == _dbModel.BlessingId
                        && k.Id == _dbModel.Id
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

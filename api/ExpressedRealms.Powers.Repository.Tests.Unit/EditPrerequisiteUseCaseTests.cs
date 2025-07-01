using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.Powers.Repository.PowerPrerequisites;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPrerequisiteUseCase;
using ExpressedRealms.Powers.Repository.Powers;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Powers.Repository.Tests.Unit;

public class EditPrerequisiteUseCaseTests
{
    private readonly EditPrerequisiteUseCase _useCase;
    private readonly IPowerPrerequisitesRepository _repository;
    private readonly IPowerRepository _powerRepository;
    private readonly EditPrerequisiteModel _model;
    private const int ReturnedPrerequisiteId = 6;

    public EditPrerequisiteUseCaseTests()
    {
        _model = new EditPrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = 1,
            PrerequisitePowerIds = [1, 2, 3],
        };

        var dbModel = new PowerPrerequisite() { Id = ReturnedPrerequisiteId, RequiredAmount = 1 };

        _repository = A.Fake<IPowerPrerequisitesRepository>();
        _powerRepository = A.Fake<IPowerRepository>();

        A.CallTo(() => _powerRepository.IsValidRequirement(A<int>.Ignored)).Returns(true);
        A.CallTo(() => _powerRepository.AreValidPowers(A<List<int>>.Ignored)).Returns(true);
        A.CallTo(() => _powerRepository.RequirementAlreadyExists(A<int>.Ignored)).Returns(false);
        A.CallTo(() => _repository.GetPrerequisiteForEditingAsync(A<int>.Ignored)).Returns(dbModel);
        A.CallTo(() => _repository.AddPrerequisite(A<PowerPrerequisite>.Ignored))
            .Returns(ReturnedPrerequisiteId);

        var validator = new EditPrerequisiteModelValidator(_powerRepository);

        _useCase = new EditPrerequisiteUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_IfPrerequisiteIdDoesNotExist()
    {
        A.CallTo(() => _powerRepository.IsValidRequirement(_model.Id)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(
            nameof(EditPrerequisiteModel.Id),
            "This is not a valid prerequisite id."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_IfPrerequisiteIdIsEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(nameof(EditPrerequisiteModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_PrerequisitePowerIds_WillFail_IfAnyPrerequisitePowersDoesNotExist()
    {
        A.CallTo(() => _powerRepository.AreValidPowers(A<List<int>>.Ignored)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);

        results.HasValidationError(
            nameof(EditPrerequisiteModel.PrerequisitePowerIds),
            "One or more prerequisite powers are invalid."
        );
    }

    [Fact]
    public async Task ValidationFor_PrerequisitePowerIds_WillFail_IfPrerequisitePowersAreEmpty()
    {
        _model.PrerequisitePowerIds = [];

        var results = await _useCase.ExecuteAsync(_model);

        results.HasValidationError(
            nameof(EditPrerequisiteModel.PrerequisitePowerIds),
            "Prerequisite Power Ids are required."
        );
    }

    [Theory]
    [InlineData(-3)]
    [InlineData(0)]
    [InlineData(-7)]
    public async Task ValidationFor_RequiredAmount_WillFail_IfRequiredAmountIsLessThenNegativeTwoOrZero(
        int requiredAmount
    )
    {
        _model.RequiredAmount = requiredAmount;

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(
            nameof(EditPrerequisiteModel.RequiredAmount),
            "Required Amount can only be a value greater then 0, or -1 (All) or -2 (Any)"
        );
    }

    [Fact]
    public async Task ValidationFor_RequiredAmount_WillSucceed_IfRequiredAmountIsGreaterThen0()
    {
        _model.RequiredAmount = 1;

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_RequiredAmount_WillSucceed_IfSetToAnyPower()
    {
        _model.RequiredAmount = -1;

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_RequiredAmount_WillSucceed_IfSetToAllPowers()
    {
        _model.RequiredAmount = -2;

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task UseCase_WillGrab_TheCorrespondingPrerequisite()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetPrerequisiteForEditingAsync(_model.Id))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillUpdate_TheRequiredAmount()
    {
        _model.RequiredAmount = 5;
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _repository.UpdatePrerequisite(
                    A<PowerPrerequisite>.That.Matches(x =>
                        x.RequiredAmount == _model.RequiredAmount
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillRemove_AllPrerequisitePowers()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.RemovePrerequisitePowers(_model.Id))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillAdd_UpdatedPrerequisitePowers()
    {
        await _useCase.ExecuteAsync(_model);

        var requisitePowers = _model
            .PrerequisitePowerIds.Select(x => new PowerPrerequisitePower()
            {
                PrerequisiteId = ReturnedPrerequisiteId,
                PowerId = x,
            })
            .ToList();

        A.CallTo(() =>
                _repository.AddPrerequisitePowers(
                    A<List<PowerPrerequisitePower>>.That.Matches(list =>
                        list.Count == requisitePowers.Count
                        && list.All(item =>
                            requisitePowers.Any(req =>
                                req.PrerequisiteId == item.PrerequisiteId
                                && req.PowerId == item.PowerId
                            )
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

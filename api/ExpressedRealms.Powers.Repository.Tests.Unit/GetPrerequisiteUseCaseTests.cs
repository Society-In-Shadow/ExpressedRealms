using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.Powers.Repository.PowerPrerequisites;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.GetPrerequisiteUseCase;
using ExpressedRealms.Powers.Repository.Powers;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Powers.Repository.Tests.Unit;

public class GetPrerequisiteUseCaseTests
{
    private readonly GetPrerequisiteUseCase _useCase;
    private readonly IPowerPrerequisitesRepository _repository;
    private readonly IPowerRepository _powerRepository;
    private readonly GetPrerequisiteModel _model;
    private const int ReturnedPrerequisiteId = 6;

    public GetPrerequisiteUseCaseTests()
    {
        _model = new GetPrerequisiteModel() { PowerId = 3 };

        _repository = A.Fake<IPowerPrerequisitesRepository>();
        _powerRepository = A.Fake<IPowerRepository>();

        A.CallTo(() => _powerRepository.IsValidPower(A<int>.Ignored)).Returns(true);
        A.CallTo(() => _repository.GetPrerequisiteAndPowersForEditingAsync(A<int>.Ignored))
            .Returns(
                new PowerPrerequisite()
                {
                    Id = ReturnedPrerequisiteId,
                    RequiredAmount = 3,
                    PrerequisitePowers = new List<PowerPrerequisitePower>()
                    {
                        new PowerPrerequisitePower() { PowerId = 1 },
                        new PowerPrerequisitePower() { PowerId = 2 },
                    },
                }
            );

        var validator = new GetPrerequisiteModelValidator(_powerRepository);

        _useCase = new GetPrerequisiteUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_IfPowerIdDoesNotExist()
    {
        A.CallTo(() => _powerRepository.IsValidPower(_model.PowerId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(
            nameof(GetPrerequisiteModel.PowerId),
            "This is not a valid power id."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_IfPrerequisiteIdIsEmpty()
    {
        _model.PowerId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(nameof(GetPrerequisiteModel.PowerId), "Power Id is required.");
    }

    [Fact]
    public async Task UseCase_WillReturnNull_IfThereIsNoPrerequisite()
    {
        A.CallTo(() => _repository.GetPrerequisiteAndPowersForEditingAsync(_model.PowerId))
            .Returns(Task.FromResult<PowerPrerequisite?>(null));

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Null(result.Value);
    }

    [Fact]
    public async Task UseCase_WillReturn_PrerequisiteId_IfNotNull()
    {
        await _useCase.ExecuteAsync(_model);

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(ReturnedPrerequisiteId, result.Value!.Id);
    }

    [Fact]
    public async Task UseCase_WillReturn_RequiredAmount_IfNotNull()
    {
        await _useCase.ExecuteAsync(_model);

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(3, result.Value!.RequiredAmount);
    }

    [Fact]
    public async Task UseCase_WillReturn_PowerIds_IfNotNull()
    {
        await _useCase.ExecuteAsync(_model);

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal([1, 2], result.Value!.PowerIds);
    }
}

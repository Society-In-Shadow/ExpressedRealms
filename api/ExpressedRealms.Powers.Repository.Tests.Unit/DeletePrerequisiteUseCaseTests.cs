using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.Powers.Repository.PowerPrerequisites;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.DeletePrerequisiteUseCase;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.EditPrerequisiteUseCase;
using ExpressedRealms.Powers.Repository.Powers;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Powers.Repository.Tests.Unit;

public class DeletePrerequisiteUseCaseTests
{
    private readonly DeletePrerequisiteUseCase _useCase;
    private readonly IPowerPrerequisitesRepository _repository;
    private readonly IPowerRepository _powerRepository;
    private readonly DeletePrerequisiteModel _model;
    private const int ReturnedPrerequisiteId = 6;

    public DeletePrerequisiteUseCaseTests()
    {
        _model = new DeletePrerequisiteModel() { Id = 3 };

        _repository = A.Fake<IPowerPrerequisitesRepository>();
        _powerRepository = A.Fake<IPowerRepository>();

        A.CallTo(() => _powerRepository.IsValidRequirement(A<int>.Ignored)).Returns(true);
        A.CallTo(() => _repository.AddPrerequisite(A<PowerPrerequisite>.Ignored))
            .Returns(ReturnedPrerequisiteId);

        var validator = new DeletePrerequisiteModelValidator(_powerRepository);

        _useCase = new DeletePrerequisiteUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_IfPrerequisiteIdDoesNotExist()
    {
        A.CallTo(() => _powerRepository.IsValidRequirement(_model.Id)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(
            nameof(DeletePrerequisiteModel.Id),
            "This is not a valid prerequisite id."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_IfPrerequisiteIdIsEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(nameof(DeletePrerequisiteModel.Id), "Id is required.");
    }

    [Fact]
    public async Task UseCase_WillDelete_PrerequisitePowers()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.RemovePrerequisitePowers(_model.Id))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillDelete_Prerequisite()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.DeletePrerequisite(_model.Id)).MustHaveHappenedOnceExactly();
    }
}

using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.Delete;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.Events;

public class DeleteEventUseCaseTests
{
    private readonly DeleteEventUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly DeleteEventModel _model;
    private readonly Event _dbModel;

    public DeleteEventUseCaseTests()
    {
        _model = new DeleteEventModel() { Id = 2 };

        _dbModel = new Event()
        {
            Id = 2,
            Name = "Test Event",
            Location = "Location",
            StartDate = DateOnly.Parse("10/31/2025"),
            EndDate = DateOnly.Parse("11/02/2025"),
            WebsiteName = "Website Name",
            AdditionalNotes = "Additional Notes",
            WebsiteUrl = "https://societyinshadows.org",
            TimeZoneId = "UTC",
            ConExperience = 20,
        };

        _repository = A.Fake<IEventRepository>();

        A.CallTo(() => _repository.GetEventAsync(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _repository.IsExistingEvent(_model.Id)).Returns(true);

        var validator = new DeleteEventModelValidator(_repository);

        _useCase = new DeleteEventUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteEventModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillReturnNotFound_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.Id)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<NotFoundFailure>(
            nameof(DeleteEventModel.Id),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillDeleteTheEvent()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.EditAsync(A<Event>.That.Matches(k => k.IsDeleted == true)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillPassThroughTheDbModel()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.EditAsync(A<Event>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }
}

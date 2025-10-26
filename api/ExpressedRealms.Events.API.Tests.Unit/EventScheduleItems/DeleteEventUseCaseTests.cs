using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventScheduleItems.Delete;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventScheduleItems;

public class DeleteEventScheduleItemUseCaseTests
{
    private readonly DeleteEventScheduleItemUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly DeleteEventScheduleItemModel _model;
    private readonly Event _dbEventModel;
    private readonly EventScheduleItem _dbModel;

    public DeleteEventScheduleItemUseCaseTests()
    {
        _dbEventModel = new Event
        {
            StartDate = DateOnly.Parse("10/28/2025"),
            EndDate = DateOnly.Parse("10/31/2025"),
            Name = "Name",
            Location = "Location",
            WebsiteName = "website",
            WebsiteUrl = "url",
            AdditionalNotes = "additional notes",
            TimeZoneId = "TimeZoneId",
        };

        _dbModel = new EventScheduleItem()
        {
            Description = "My Scheduled Event",
            StartTime = TimeOnly.Parse("12:00"),
            EndTime = TimeOnly.Parse("13:00"),
            Date = DateOnly.Parse("10/31/2025"),
            EventId = 1,
        };

        _model = new DeleteEventScheduleItemModel() { Id = 2, EventId = 1 };
        _repository = A.Fake<IEventRepository>();

        A.CallTo(() => _repository.GetEventScheduleItem(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _repository.GetEventAsync(_model.EventId)).Returns(_dbEventModel);
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(true);

        var validator = new DeleteEventScheduleItemModelValidator(_repository);

        _useCase = new DeleteEventScheduleItemUseCase(
            _repository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteEventScheduleItemModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillReturnNotFound_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.GetEventScheduleItem(_model.Id))
            .Returns(Task.FromResult<EventScheduleItem?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<NotFoundFailure>(
            nameof(DeleteEventScheduleItemModel.Id),
            "Event Schedule Item does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(DeleteEventScheduleItemModel.EventId),
            "Event Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEventDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(DeleteEventScheduleItemModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillDeleteTheEventScheduleItem()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditAsync(A<EventScheduleItem>.That.Matches(k => k.IsDeleted == true))
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillPassThroughTheDbModel()
    {
        var foo = await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.EditAsync(A<EventScheduleItem>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }
}

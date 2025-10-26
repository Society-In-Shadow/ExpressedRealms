using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventScheduleItems.Edit;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventScheduleItems;

public class EditEventScheduleItemUseCaseTests
{
    private readonly EditEventScheduleItemUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly EditEventScheduleItemModel _model;
    private readonly Event _dbEventModel;
    private readonly EventScheduleItem _dbModel;

    public EditEventScheduleItemUseCaseTests()
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
        
        _model = new EditEventScheduleItemModel()
        {
            Id = 2,
            Description = "My Scheduled Event",
            StartTime = TimeOnly.Parse("12:00"),
            EndTime = TimeOnly.Parse("13:00"),
            Date = DateOnly.Parse("10/31/2025"),
            EventId = 1,
        };
        _repository = A.Fake<IEventRepository>();

        A.CallTo(() => _repository.GetEventScheduleItem(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _repository.GetEventAsync(_model.EventId)).Returns(_dbEventModel);
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(true);

        var validator = new EditEventScheduleItemModelValidator(_repository);

        _useCase = new EditEventScheduleItemUseCase(
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
        results.MustHaveValidationError(nameof(EditEventScheduleItemModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillReturnNotFound_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.GetEventScheduleItem(_model.Id)).Returns(Task.FromResult<EventScheduleItem?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<NotFoundFailure>(
            nameof(EditEventScheduleItemModel.Id),
            "Event Schedule Item does not exist."
        );
    }
    
    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenEmpty()
    {
        _model.Description = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventScheduleItemModel.Description),
            "Description is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenOver250Characters()
    {
        _model.Description = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventScheduleItemModel.Description),
            "Description must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_StartTime_WillFail_WhenEmpty()
    {
        _model.StartTime = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventScheduleItemModel.StartTime),
            "Start Date is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EndTime_WillFail_WhenEmpty()
    {
        _model.EndTime = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventScheduleItemModel.EndTime),
            "End Date is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Date_WillFail_WhenEmpty()
    {
        _model.Date = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventScheduleItemModel.Date),
            "Date is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Date_WillFail_WhenOutsideEventDates()
    {
        A.CallTo(() => _repository.GetEventAsync(_model.EventId)).Returns(_dbEventModel);
        _model.Date = _dbEventModel.EndDate.AddDays(1); // outside range

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventScheduleItemModel.Date),
            "Date must be within the event dates."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventScheduleItemModel.EventId),
            "Event Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEventDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventScheduleItemModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillEditTheEventScheduleItem()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditAsync(
                    A<EventScheduleItem>.That.Matches(k =>
                        k.Description == _model.Description
                        && k.StartTime == _model.StartTime
                        && k.EndTime == _model.EndTime
                        && k.EventId == _model.EventId
                        && k.Date == _model.Date
                    )
                )
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

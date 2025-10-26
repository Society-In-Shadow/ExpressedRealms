using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventScheduleItems.Create;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventScheduleItems;

public class CreateEventScheduleItemUseCaseTests
{
    private readonly CreateEventScheduleItemUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly CreateEventScheduleItemModel _model;
    private readonly Event _dbEventModel;

    public CreateEventScheduleItemUseCaseTests()
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
            TimeZoneId = "TimeZoneId"
        };
        
        _model = new CreateEventScheduleItemModel()
        {
            Description = "My Scheduled Event",
            StartTime = TimeOnly.Parse("12:00"),
            EndTime = TimeOnly.Parse("13:00"),
            Date = DateOnly.Parse("10/31/2025"),
            EventId = 1,
        };
        _repository = A.Fake<IEventRepository>();

        A.CallTo(() => _repository.GetEventAsync(_model.EventId)).Returns(_dbEventModel);
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(true);

        var validator = new CreateEventScheduleItemModelValidator(_repository);

        _useCase = new CreateEventScheduleItemUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenEmpty()
    {
        _model.Description = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateEventScheduleItemModel.Description), "Description is required.");
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenOver250Characters()
    {
        _model.Description = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventScheduleItemModel.Description),
            "Description must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_StartTime_WillFail_WhenEmpty()
    {
        _model.StartTime = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateEventScheduleItemModel.StartTime), "Start Date is required.");
    }

    [Fact]
    public async Task ValidationFor_EndTime_WillFail_WhenEmpty()
    {
        _model.EndTime = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateEventScheduleItemModel.EndTime), "End Date is required.");
    }

    [Fact]
    public async Task ValidationFor_Date_WillFail_WhenEmpty()
    {
        _model.Date = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateEventScheduleItemModel.Date), "Date is required.");
    }

    [Fact]
    public async Task ValidationFor_Date_WillFail_WhenOutsideEventDates()
    {
        A.CallTo(() => _repository.GetEventAsync(_model.EventId)).Returns(_dbEventModel);
        _model.Date = _dbEventModel.EndDate.AddDays(1); // outside range

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateEventScheduleItemModel.Date), "Date must be within the event dates.");
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateEventScheduleItemModel.EventId), "Event Id is required.");
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEventDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventScheduleItemModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillCreateTheEventScheduleItem()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.CreateEventScheduleItemAsync(
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
    public async Task UseCase_WillReturn_EventId_IfSuccessful()
    {
        A.CallTo(() => _repository.CreateEventScheduleItemAsync(A<EventScheduleItem>._)).Returns(5);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.Equal(5, result.Value);
    }
}

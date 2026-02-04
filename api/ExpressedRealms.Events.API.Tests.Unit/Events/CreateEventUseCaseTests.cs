using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventQuestions.PopulateDefaults;
using ExpressedRealms.Events.API.UseCases.Events.Create;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.Events;

public class CreateEventUseCaseTests
{
    private readonly CreateEventUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly CreateEventModel _model;
    private readonly IPopulateDefaultQuestionsUseCase _populateDefaultQuestionsUseCase;
    private readonly List<EventScheduleItem> _defaultScheduledEvents;

    public CreateEventUseCaseTests()
    {
        _model = new CreateEventModel()
        {
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

        _defaultScheduledEvents = new List<EventScheduleItem>()
        {
            new()
            {
                Id = 1,
                Date = DateOnly.Parse("10/24/2025"),
                EventId = 1,
                Description = "Friday Event",
                StartTime = TimeOnly.Parse("12:00"),
                EndTime = TimeOnly.Parse("13:00"),
            },
            new()
            {
                Id = 2,
                Date = DateOnly.Parse("10/25/2025"),
                EventId = 1,
                Description = "Saturday Event",
                StartTime = TimeOnly.Parse("14:00"),
                EndTime = TimeOnly.Parse("15:00"),
            },
            new()
            {
                Id = 3,
                Date = DateOnly.Parse("10/26/2025"),
                EventId = 1,
                Description = "Sunday Event",
                StartTime = TimeOnly.Parse("14:00"),
                EndTime = TimeOnly.Parse("15:00"),
            },
        };

        _repository = A.Fake<IEventRepository>();
        _populateDefaultQuestionsUseCase = A.Fake<IPopulateDefaultQuestionsUseCase>();

        A.CallTo(() => _repository.CreateEventAsync(A<Event>._)).Returns(1);
        A.CallTo(() => _repository.GetDefaultScheduleItems()).Returns(_defaultScheduledEvents);

        var validator = new CreateEventModelValidator(_repository);

        _useCase = new CreateEventUseCase(_repository, _populateDefaultQuestionsUseCase, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenEmpty()
    {
        _model.Name = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateEventModel.Name), "Name is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenOver250Characters()
    {
        _model.Name = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.Name),
            "Name must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_StartDate_WillFail_WhenEmpty()
    {
        _model.StartDate = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.StartDate),
            "Start Date is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EndDate_WillFail_WhenEmpty()
    {
        _model.EndDate = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateEventModel.EndDate), "End Date is required.");
    }

    [Fact]
    public async Task ValidationFor_Location_WillFail_WhenEmpty()
    {
        _model.Location = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(CreateEventModel.Location), "Location is required.");
    }

    [Fact]
    public async Task ValidationFor_Location_WillFail_WhenOver1000Characters()
    {
        _model.Location = new string('x', 1001);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.Location),
            "Location must be between 1 and 1000 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteName_WillFail_WhenEmpty()
    {
        _model.WebsiteName = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.WebsiteName),
            "Website Name is required."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteName_WillFail_WhenOver250Characters()
    {
        _model.WebsiteName = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.WebsiteName),
            "Website Name must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillFail_WhenEmpty()
    {
        _model.WebsiteUrl = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.WebsiteUrl),
            "Website Url is required."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillFail_WhenOver500Characters()
    {
        _model.WebsiteUrl = "https://" + new string('x', 495);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.WebsiteUrl),
            "Website Url must be between 1 and 500 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillFail_WhenInvalidUrl()
    {
        _model.WebsiteUrl = "not a url";

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.WebsiteUrl),
            "Website Url must be a valid URL."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillFail_WhenUnsupportedScheme()
    {
        _model.WebsiteUrl = "ftp://example.com";

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.WebsiteUrl),
            "Website Url must be a valid URL."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillPass_WhenValidHttpUrl()
    {
        _model.WebsiteUrl = "http://example.com";

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillPass_WhenValidHttpsUrl()
    {
        _model.WebsiteUrl = "https://example.com";

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_AdditionalNotes_WillFail_WhenOver5000Characters()
    {
        _model.AdditionalNotes = new string('x', 5001);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.AdditionalNotes),
            "Additional Notes must be between 1 and 5000 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_TimeZoneId_WillFail_WhenEmpty()
    {
        _model.TimeZoneId = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.TimeZoneId),
            "Time Zone Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_TimeZoneId_WillFail_WhenOver250Characters()
    {
        _model.TimeZoneId = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.TimeZoneId),
            "Time Zone Id must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_TimeZoneId_WillFail_WhenItIsNotAValidTimeZoneId()
    {
        _model.TimeZoneId = "Not Time Zone";

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.TimeZoneId),
            "Time Zone Id is not a valid time zone."
        );
    }

    [Fact]
    public async Task ValidationFor_ConExperience_WillFail_WhenEmpty()
    {
        _model.ConExperience = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventModel.ConExperience),
            "Con Experience is required."
        );
    }

    [Fact]
    public async Task UseCase_WillCreateTheEvent()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.CreateEventAsync(
                    A<Event>.That.Matches(k =>
                        k.Name == _model.Name
                        && k.Location == _model.Location
                        && k.StartDate == _model.StartDate
                        && k.EndDate == _model.EndDate
                        && k.WebsiteName == _model.WebsiteName
                        && k.WebsiteUrl == _model.WebsiteUrl
                        && k.AdditionalNotes == _model.AdditionalNotes
                        && k.ConExperience == _model.ConExperience
                        && k.TimeZoneId == _model.TimeZoneId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillPassThroughTheDefaultScheduleItems()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.BulkAddEventScheduleItems(
                    A<List<EventScheduleItem>>.That.IsSameAs(_defaultScheduledEvents)
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ScheduleItems_WillZeroOutIds_ToAllowDuplication()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.BulkAddEventScheduleItems(
                    A<List<EventScheduleItem>>.That.Matches(k => k.All(i => i.Id == 0))
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ScheduleItems_WillAssign_NewEventId()
    {
        A.CallTo(() => _repository.CreateEventAsync(A<Event>._)).Returns(5);
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.BulkAddEventScheduleItems(
                    A<List<EventScheduleItem>>.That.Matches(k => k.All(i => i.EventId == 5))
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillPopulateDefaultQuestions()
    {
        A.CallTo(() => _repository.CreateEventAsync(A<Event>._)).Returns(7);
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _populateDefaultQuestionsUseCase.ExecuteAsync(A<PopulateDefaultQuestionsModel>.That
                .Matches(k => k.EventId == 7)))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task UseCase_ScheduleItems_AdjustsToCorrectFridayDate()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.BulkAddEventScheduleItems(
                    A<List<EventScheduleItem>>.That.Matches(k =>
                        k.Exists(x =>
                            x.Description == "Friday Event"
                            && x.Date == DateOnly.Parse("10/31/2025")
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ScheduleItems_AdjustsToCorrectSaturdayDate()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.BulkAddEventScheduleItems(
                    A<List<EventScheduleItem>>.That.Matches(k =>
                        k.Exists(x =>
                            x.Description == "Saturday Event"
                            && x.Date == DateOnly.Parse("11/01/2025")
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ScheduleItems_AdjustsToCorrectSundayDate()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.BulkAddEventScheduleItems(
                    A<List<EventScheduleItem>>.That.Matches(k =>
                        k.Exists(x =>
                            x.Description == "Sunday Event"
                            && x.Date == DateOnly.Parse("11/02/2025")
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ScheduleItems_AdjustsToCorrectFridayDate_EvenWhenEventStartsDayEarly()
    {
        _model.StartDate = DateOnly.Parse("10/29/2025");
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.BulkAddEventScheduleItems(
                    A<List<EventScheduleItem>>.That.Matches(k =>
                        k.Exists(x =>
                            x.Description == "Friday Event"
                            && x.Date == DateOnly.Parse("10/31/2025")
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ScheduleItems_AdjustsToCorrectSaturdayDate_EvenWhenEventStartsDayEarly()
    {
        _model.StartDate = DateOnly.Parse("10/29/2025");
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.BulkAddEventScheduleItems(
                    A<List<EventScheduleItem>>.That.Matches(k =>
                        k.Exists(x =>
                            x.Description == "Saturday Event"
                            && x.Date == DateOnly.Parse("11/01/2025")
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ScheduleItems_AdjustsToCorrectSundayDate_EvenWhenEventStartsDayEarly()
    {
        _model.StartDate = DateOnly.Parse("10/29/2025");
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.BulkAddEventScheduleItems(
                    A<List<EventScheduleItem>>.That.Matches(k =>
                        k.Exists(x =>
                            x.Description == "Sunday Event"
                            && x.Date == DateOnly.Parse("11/02/2025")
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_EventId_IfSuccessful()
    {
        A.CallTo(() => _repository.CreateEventAsync(A<Event>._)).Returns(5);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.Equal(5, result.Value);
    }
}

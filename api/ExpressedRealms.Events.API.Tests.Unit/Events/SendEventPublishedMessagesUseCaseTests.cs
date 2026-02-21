using System.Globalization;
using System.Net;
using Discord;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.SendEventPublishedMessages;
using ExpressedRealms.Shared;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.Events;

public class SendEventPublishedMessagesUseCaseTests
{
    private readonly SendEventPublishedMessagesUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly SendEventPublishedMessagesModel _model;
    private readonly List<EventScheduleItem> dbEventItems;
    private readonly Event _dbModel;
    private readonly IDiscordService _discordService;
    private readonly TimeProvider _systemClock;

    public SendEventPublishedMessagesUseCaseTests()
    {
        _model = new SendEventPublishedMessagesModel() { Id = 2 };

        _dbModel = new Event()
        {
            Id = 2,
            Name = "Sioux City Geek Con",
            Location = "Sioux City Convention Center Sioux City, Iowa",
            StartDate = DateOnly.Parse("08/22/2025", CultureInfo.InvariantCulture),
            EndDate = DateOnly.Parse("08/24/2025", CultureInfo.InvariantCulture),
            WebsiteName = "Website Name",
            AdditionalNotes = "Additional Notes",
            WebsiteUrl = "https://societyinshadows.org",
            TimeZoneId = "America/Chicago",
            ConExperience = 32,
        };

        dbEventItems = new List<EventScheduleItem>()
        {
            new()
            {
                Id = 1,
                Description = "Test Event 1",
                StartTime = TimeOnly.Parse("10:00", CultureInfo.InvariantCulture),
                EndTime = TimeOnly.Parse("10:30", CultureInfo.InvariantCulture),
                Date = DateOnly.Parse("08/22/2025", CultureInfo.InvariantCulture),
                EventId = 1,
            },
            new()
            {
                Id = 2,
                Description = "Test Event 2",
                StartTime = TimeOnly.Parse("11:00", CultureInfo.InvariantCulture),
                EndTime = TimeOnly.Parse("11:30", CultureInfo.InvariantCulture),
                Date = DateOnly.Parse("08/23/2025", CultureInfo.InvariantCulture),
                EventId = 1,
            },
            new()
            {
                Id = 3,
                Description = "Test Event 3",
                StartTime = TimeOnly.Parse("12:00", CultureInfo.InvariantCulture),
                EndTime = TimeOnly.Parse("12:30", CultureInfo.InvariantCulture),
                Date = DateOnly.Parse("08/24/2025", CultureInfo.InvariantCulture),
                EventId = 1,
            },
        };

        _repository = A.Fake<IEventRepository>();
        _discordService = A.Fake<IDiscordService>();
        _systemClock = A.Fake<TimeProvider>();

        A.CallTo(() => _repository.FindEventAsync(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _repository.GetEventScheduleItems(_model.Id)).Returns(dbEventItems);
        A.CallTo(() => _systemClock.GetUtcNow()).Returns(new DateTime(2025, 08, 22, 12, 0, 0, DateTimeKind.Utc));

        var validator = new SendEventPublishedMessagesModelValidator(_repository);

        _useCase = new SendEventPublishedMessagesUseCase(
            _repository,
            validator,
            _discordService,
            _systemClock,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(SendEventPublishedMessagesModel.Id),
            "Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillReturnNotFound_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.FindEventAsync(_model.Id))
            .Returns(Task.FromResult<Event?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<NotFoundFailure>(
            nameof(SendEventPublishedMessagesModel.Id),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task UseCase_MessageContains_EventTitle_WithFormatting()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains("# Sioux City Geek Con Has Been Confirmed!"),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_MessageContains_LocationMessage_WithFormatting()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "We are happy to announce that we will be attending **Sioux City Geek Con** out in the **Sioux City Convention Center Sioux City, Iowa**!"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_MessageContains_ConDatesMessage_WithFormatting()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "Their scheduled dates are **Friday August 22, 2025** to **Sunday August 24, 2025**."
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ContainsMessage_ThatWeWillAttendAllDays_WithFormatting()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains("# Society in Shadows will be there every day!"),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ContainsMessage_ThatWeWillAttendASubSetOfDays_WithFormatting()
    {
        _dbModel.StartDate = DateOnly.Parse("08/21/2025", CultureInfo.InvariantCulture);
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "# Society in Shadows will be there on Friday, Saturday and Sunday!"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(PublishType.OneMonthReminder)]
    [InlineData(PublishType.OneWeekReminder)]
    public async Task UseCase_ContainsMessage_OfHowMuchXPTheyWillEarn_WithFormatting_WhenItsNotInitialAnnouncement(
        PublishType type
    )
    {
        _model.PublishType = type;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        $"Players now have access to an additional **32 XP** for this event on their Primary Characters!"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(PublishType.OneMonthReminder)]
    [InlineData(PublishType.OneWeekReminder)]
    [InlineData(PublishType.DayOfReminder)]
    public async Task UseCase_ContainsMessage_StatingTheTimezone_WithFormatting_WhenItIsNotAnInitialAnnouncement(
        PublishType type
    )
    {
        _model.PublishType = type;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "Sioux City Geek Con is in the **Central Standard Time** and our schedule below reflects that."
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_DoesNotContainMessage_StatingTheTimezone_WithFormatting_WhenItIsAnInitialAnnouncement()
    {
        _model.PublishType = PublishType.InitialAnnouncement;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "Sioux City Geek Con is in the **Central Standard Time** and our schedule below reflects that."
                    ),
                    A<Embed[]>._
                )
            )
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_HeaderStatesMonthOut_WhenItIsAOneMonthReminder()
    {
        _model.PublishType = PublishType.OneMonthReminder;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains("# Sioux City Geek Con is about a month out!"),
                    A<Embed[]>._
                )
            )
            .MustHaveHappened();
    }

    [Fact]
    public async Task UseCase_HeaderStatesMonthOut_WhenItIsAOneWeekReminder()
    {
        _model.PublishType = PublishType.OneWeekReminder;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains("# Sioux City Geek Con is about a week away!"),
                    A<Embed[]>._
                )
            )
            .MustHaveHappened();
    }

    [Theory]
    [InlineData(PublishType.OneMonthReminder)]
    [InlineData(PublishType.OneWeekReminder)]
    [InlineData(PublishType.DayOfReminder)]
    public async Task UseCase_ContainsMessage_FridaysScheduledEvent_WithFormatting_WhenItIsNotAnInitialAnnouncement(
        PublishType type
    )
    {
        _model.PublishType = type;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains("Friday\n\n**10:00 AM** - 10:30 AM - **Test Event 1**"),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(PublishType.OneMonthReminder)]
    [InlineData(PublishType.OneWeekReminder)]
    [InlineData(PublishType.DayOfReminder)]
    public async Task UseCase_ContainsMessage_SaturdaysScheduledEvent_WithFormatting_WhenItIsNotAnInitialAnnouncement(
        PublishType type
    )
    {
        _model.PublishType = type;
        A.CallTo(() => _systemClock.GetUtcNow()).Returns(new DateTime(2025, 08, 23, 12, 0, 0, DateTimeKind.Utc));
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "Saturday\n\n**11:00 AM** - 11:30 AM - **Test Event 2**"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(PublishType.OneMonthReminder)]
    [InlineData(PublishType.OneWeekReminder)]
    [InlineData(PublishType.DayOfReminder)]
    public async Task UseCase_ContainsMessage_SundaysScheduledEvent_WithFormatting_WhenItIsNotAnInitialAnnouncement(
        PublishType type
    )
    {
        _model.PublishType = type;
        A.CallTo(() => _systemClock.GetUtcNow()).Returns(new DateTime(2025, 08, 24, 12, 0, 0, DateTimeKind.Utc));
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains("Sunday\n\n**12:00 PM** - 12:30 PM - **Test Event 3**"),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ContainsMessage_CheckInWithBooth_OnDayOfReminder()
    {
        _model.PublishType = PublishType.DayOfReminder;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "** We do need you to check in at our booth (SHQ) for the following reasons:**"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_OnDayOfReminder_WillOnlyShow_TheDatesEvents()
    {
        _model.PublishType = PublishType.DayOfReminder;
        A.CallTo(() => _systemClock.GetUtcNow()).Returns(new DateTime(2025, 08, 24, 12, 0, 0, DateTimeKind.Utc));
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "## Sunday"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
        
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Not.Contains(
                        "## Friday"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();        
        
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Not.Contains(
                        "## Saturday"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }
    
    [Theory]
    [InlineData(PublishType.OneMonthReminder)]
    [InlineData(PublishType.OneWeekReminder)]
    [InlineData(PublishType.InitialAnnouncement)]
    public async Task UseCase_OnlyContainsMessage_CheckInWithBooth_OnDayOfReminder(PublishType type)
    {
        _model.PublishType = type;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Not.Contains(
                        "** We do need you to check in at our booth (SHQ) for the following reasons:**"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ContainsMessage_DailyScheduleAndXPWillBeReleasedLater_ForInitialAnnouncement()
    {
        _model.PublishType = PublishType.InitialAnnouncement;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "The daily schedule and assigned XP will be provided one month out from the event!"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(PublishType.OneMonthReminder)]
    [InlineData(PublishType.OneWeekReminder)]
    [InlineData(PublishType.DayOfReminder)]
    public async Task UseCase_OnlyContainsMessage_DailyScheduleWillBeReleasedLater_OnDayOfReminder(
        PublishType type
    )
    {
        _model.PublishType = type;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Not.Contains(
                        "The daily schedule will be provided roughly a month out from the event"
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_PassesThrough_WebsiteLinkAsEmbed()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>._,
                    A<Embed[]>.That.Matches(embeds =>
                        embeds.Any(k =>
                            k.Url == _dbModel.WebsiteUrl
                            && k.Title == _dbModel.WebsiteName
                            && k.Description == $"{_dbModel.Name} Website!"
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_PassesThrough_ConLocationAsEmbed()
    {
        var mapUrl =
            $"https://www.google.com/maps/search/?api=1&query={WebUtility.UrlEncode(_dbModel.Location)}";
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>._,
                    A<Embed[]>.That.Matches(embeds =>
                        embeds.Any(k =>
                            k.Url == mapUrl
                            && k.Title == _dbModel.Location
                            && k.Description == $"{_dbModel.Name} Location!"
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ForInternalReminder_WillUseDevGeneralChannel()
    {
        _model.PublishType = PublishType.InternalReminder;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.DevGeneralChannel,
                    A<string>._,
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ForInternalReminder_HasCorrectHeader()
    {
        _model.PublishType = PublishType.InternalReminder;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.DevGeneralChannel,
                    A<string>.That.Contains("# Reminder to Review Schedule!"),
                    A<Embed[]>._
                )
            )
            .MustHaveHappened();
    }

    [Fact]
    public async Task UseCase_ForInternalReminder_HasCorrectMessage()
    {
        _model.PublishType = PublishType.InternalReminder;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.DevGeneralChannel,
                    A<string>.That.Contains("The following message will be sent out in a week."),
                    A<Embed[]>._
                )
            )
            .MustHaveHappened();
    }

    [Fact]
    public async Task UseCase_CreatesEvent_OnInitialAnnouncement()
    {
        _model.PublishType = PublishType.InitialAnnouncement;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.CreateEventAsync(
                    A<DiscordEvent>.That.Matches(x =>
                        x.Name == _dbModel.Name
                        && x.Location == _dbModel.Location
                        && x.StartDate == _dbModel.StartDate.ToUtc(_dbModel.TimeZoneId)
                        && x.EndDate == _dbModel.EndDate.ToUtc(_dbModel.TimeZoneId)
                    )
                )
            )
            .MustHaveHappened();
    }

    [Theory]
    [InlineData(PublishType.OneMonthReminder)]
    [InlineData(PublishType.OneWeekReminder)]
    [InlineData(PublishType.DayOfReminder)]
    public async Task UseCase_DoesNotCreateEvent_OnAnyOtherPublishType(PublishType type)
    {
        _model.PublishType = type;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _discordService.CreateEventAsync(A<DiscordEvent>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_ForInternalReminder_WillShowMonthOutMessage()
    {
        _model.PublishType = PublishType.InternalReminder;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.DevGeneralChannel,
                    A<string>.That.Contains($"# Sioux City Geek Con is about a month out!"),
                    A<Embed[]>._
                )
            )
            .MustHaveHappened();
    }

    /// <summary>
    /// This is needed specifically because the cron job will not check if there are any scheduled
    /// events for the day
    /// </summary>
    [Fact]
    public async Task UseCase_DailyReminder_WillSkipDay_IfNoDetectedEventsForDay()
    {
        _model.PublishType = PublishType.DayOfReminder;
        dbEventItems.Clear();
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    A<DiscordChannel>._,
                    A<string>._,
                    A<Embed[]>._
                )
            )
            .MustNotHaveHappened();
    }
}

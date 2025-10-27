using System.Net;
using Discord;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Discord;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.SendEventPublishedMessages;
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
    private readonly Event _dbModel;
    private readonly List<EventScheduleItem> _dbEventItems;
    private readonly IDiscordService _discordService;

    public SendEventPublishedMessagesUseCaseTests()
    {
        _model = new SendEventPublishedMessagesModel() { Id = 2 };

        _dbModel = new Event()
        {
            Id = 2,
            Name = "Sioux City Geek Con",
            Location = "Sioux City Convention Center Sioux City, Iowa",
            StartDate = DateOnly.Parse("08/22/2025"),
            EndDate = DateOnly.Parse("08/24/2025"),
            WebsiteName = "Website Name",
            AdditionalNotes = "Additional Notes",
            WebsiteUrl = "https://societyinshadows.org",
            TimeZoneId = "America/Chicago",
            ConExperience = 32,
        };

        _dbEventItems = new List<EventScheduleItem>()
        {
            new()
            {
                Id = 1,
                Description = "Test Event 1",
                StartTime = TimeOnly.Parse("10:00"),
                EndTime = TimeOnly.Parse("10:30"),
                Date = DateOnly.Parse("08/22/2025"),
                EventId = 1,
            },
            new()
            {
                Id = 2,
                Description = "Test Event 2",
                StartTime = TimeOnly.Parse("11:00"),
                EndTime = TimeOnly.Parse("11:30"),
                Date = DateOnly.Parse("08/23/2025"),
                EventId = 1,
            },
            new()
            {
                Id = 3,
                Description = "Test Event 3",
                StartTime = TimeOnly.Parse("12:00"),
                EndTime = TimeOnly.Parse("12:30"),
                Date = DateOnly.Parse("08/24/2025"),
                EventId = 1,
            },
        };

        _repository = A.Fake<IEventRepository>();
        _discordService = A.Fake<IDiscordService>();

        A.CallTo(() => _repository.FindEventAsync(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _repository.GetEventScheduleItems(_model.Id)).Returns(_dbEventItems);

        var validator = new SendEventPublishedMessagesModelValidator(_repository);

        _useCase = new SendEventPublishedMessagesUseCase(
            _repository,
            validator,
            _discordService,
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
        _dbModel.StartDate = DateOnly.Parse("08/21/2025");
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

    [Fact]
    public async Task UseCase_ContainsMessage_OfHowMuchXPTheyWillEarn_WithFormatting()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>.That.Contains(
                        "Once the event starts, everyone will have access to an additional **32 XP** for this event."
                    ),
                    A<Embed[]>._
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ContainsMessage_StatingTheTimezone_WithFormatting()
    {
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
    public async Task UseCase_ContainsMessage_FridaysScheduledEvent_WithFormatting()
    {
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

    [Fact]
    public async Task UseCase_ContainsMessage_SaturdaysScheduledEvent_WithFormatting()
    {
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

    [Fact]
    public async Task UseCase_ContainsMessage_SundaysScheduledEvent_WithFormatting()
    {
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
    public async Task UseCase_PassesThrough_WebsiteLinkAsEmbed()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _discordService.SendMessageToChannelAsync(
                    DiscordChannel.PublicAnnouncements,
                    A<string>._,
                    A<Embed[]>.That.Matches(embeds =>
                        embeds.Any(k =>
                            k.Url == _dbModel.WebsiteUrl && k.Title == _dbModel.WebsiteName
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
                        embeds.Any(k => k.Url == mapUrl && k.Title == _dbModel.Location)
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

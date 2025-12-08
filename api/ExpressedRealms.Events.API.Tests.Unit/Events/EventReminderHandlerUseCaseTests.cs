using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.SendEventPublishedMessages;
using ExpressedRealms.Events.API.UseCases.Events.TriggerEventReminder;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.Events;

public class EventReminderHandlerUseCaseTests
{
    private readonly EventReminderHandlerUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly List<Event> _dbModel;
    private readonly ISendEventPublishedMessagesUseCase _sendMessageUseCase;
    private readonly TimeProvider _timeProvider;

    public EventReminderHandlerUseCaseTests()
    {
        _dbModel = new List<Event>()
        {
            new()
            {
                Id = 1,
                Name = "Test Event 1",
                Location = "Location 1",
                StartDate = DateOnly.Parse("10/31/2025"),
                EndDate = DateOnly.Parse("11/02/2025"),
                WebsiteName = "Website Name 1",
                AdditionalNotes = "Additional Notes 1",
                WebsiteUrl = "https://societyinshadows.org",
                TimeZoneId = "UTC",
                ConExperience = 20,
                IsPublished = true,
            },
            new()
            {
                Id = 2,
                Name = "Test Event 2",
                Location = "Location 2",
                StartDate = DateOnly.Parse("10/30/2025"),
                EndDate = DateOnly.Parse("11/04/2025"),
                WebsiteName = "Website Name 2",
                AdditionalNotes = "Additional Notes 2",
                WebsiteUrl = "https://societyinshadows.org",
                TimeZoneId = "UTC",
                ConExperience = 25,
                IsPublished = false,
            },
        };

        _repository = A.Fake<IEventRepository>();
        _sendMessageUseCase = A.Fake<ISendEventPublishedMessagesUseCase>();
        _timeProvider = A.Fake<TimeProvider>();

        A.CallTo(() => _timeProvider.GetUtcNow()).Returns(new DateTime(2025, 10, 30, 12, 0, 0));
        A.CallTo(() => _repository.GetCurrenOrFutureEvents()).Returns(_dbModel);

        _useCase = new EventReminderHandlerUseCase(_repository, _sendMessageUseCase, _timeProvider);
    }

    [Fact]
    public async Task UseCase_WillProcessInternalReminder_5WeeksOut()
    {
        _dbModel[0].StartDate = DateOnly
            .FromDateTime(_timeProvider.GetUtcNow().DateTime)
            .AddMonths(1)
            .AddDays(7);
        await _useCase.ExecuteAsync();

        A.CallTo(() =>
                _sendMessageUseCase.ExecuteAsync(
                    A<SendEventPublishedMessagesModel>.That.Matches(k =>
                        k.Id == 1 && k.PublishType == PublishType.InternalReminder
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillProcessOneWeekReminder_1WeekOut()
    {
        _dbModel[0].StartDate = DateOnly
            .FromDateTime(_timeProvider.GetUtcNow().DateTime)
            .AddDays(7);
        await _useCase.ExecuteAsync();

        A.CallTo(() =>
                _sendMessageUseCase.ExecuteAsync(
                    A<SendEventPublishedMessagesModel>.That.Matches(k =>
                        k.Id == 1 && k.PublishType == PublishType.OneWeekReminder
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillProcessOneMonthReminder_1MonthOut()
    {
        _dbModel[0].StartDate = DateOnly
            .FromDateTime(_timeProvider.GetUtcNow().DateTime)
            .AddMonths(1);
        await _useCase.ExecuteAsync();

        A.CallTo(() =>
                _sendMessageUseCase.ExecuteAsync(
                    A<SendEventPublishedMessagesModel>.That.Matches(k =>
                        k.Id == 1 && k.PublishType == PublishType.OneMonthReminder
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("10/29/2025")]
    [InlineData("10/30/2025")]
    [InlineData("10/31/2025")]
    public async Task UseCase_WillProcessDayOfReminder_BetweenStartAndEndDatesInclusive(
        string dayOfString
    )
    {
        _dbModel[0].StartDate = DateOnly.Parse("10/29/2025");
        _dbModel[0].EndDate = DateOnly.Parse("10/31/2025");

        var testDate = DateTimeOffset.Parse(dayOfString).Date;
        A.CallTo(() => _timeProvider.GetUtcNow()).Returns(testDate);

        await _useCase.ExecuteAsync();

        A.CallTo(() =>
                _sendMessageUseCase.ExecuteAsync(
                    A<SendEventPublishedMessagesModel>.That.Matches(k =>
                        k.Id == 1 && k.PublishType == PublishType.DayOfReminder
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

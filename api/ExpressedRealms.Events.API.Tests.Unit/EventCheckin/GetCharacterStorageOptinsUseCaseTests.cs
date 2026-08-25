using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventCheckin.GetCharacterStorageOptins;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class GetCharacterStorageOptinsUseCaseTests
{
    private readonly GetCharacterStorageOptinsUseCase _useCase;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private readonly IEventRepository _eventRepository;
    private readonly GetCharacterStorageOptinsModel _model;

    private readonly int _eventId = 123;
    private readonly int _activeEventId = 456;
    private readonly DateTimeOffset _timestamp = DateTimeOffset.UtcNow;

    public GetCharacterStorageOptinsUseCaseTests()
    {
        _model = new GetCharacterStorageOptinsModel { EventId = _eventId };

        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();
        _eventRepository = A.Fake<IEventRepository>();

        A.CallTo(() => _eventRepository.IsExistingEvent(_eventId)).Returns(true);

        A.CallTo(() => _eventCheckinRepository.GetActiveEventInfoOrDefaultAsync())
            .Returns(
                new Event()
                {
                    Id = _activeEventId,
                    Name = "Test Event",
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    EndDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    TimeZoneId = "America/New_York",
                    Location = string.Empty,
                    WebsiteName = string.Empty,
                    WebsiteUrl = string.Empty
                }
            );

        A.CallTo(() => _eventCheckinRepository.GetCharacterStorageUsersForEvent(_activeEventId))
            .Returns(
                [
                    new ()
                    {
                        Id = 1,
                        Timestamp = _timestamp,
                        ApproverName = "Approver One (001)",
                        PlayerName = "Player One (002)",
                        Amount = 20,
                    },
                    new ()
                    {
                        Id = 2,
                        Timestamp = _timestamp.AddMinutes(5),
                        ApproverName = "Approver Two (003)",
                        PlayerName = "Player Two (004)",
                        Amount = 25,
                    },
                ]
            );

        var validator = new GetCharacterStorageOptinsModelValidator(_eventRepository);

        _useCase = new GetCharacterStorageOptinsUseCase(
            _eventCheckinRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(nameof(_model.EventId), "Event Id is required.");
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenDoesNotExistInRepository()
    {
        A.CallTo(() => _eventRepository.IsExistingEvent(_eventId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveNotFoundError(nameof(_model.EventId), "Event Id does not exist.");
    }

    [Fact]
    public async Task UseCase_WillReturn_CharacterStorageOptins()
    {
        var results = await _useCase.ExecuteAsync(_model);

        Assert.True(results.IsSuccess);
        Assert.Equal(2, results.Value.CharacterStorageOptins.Count);

        Assert.Equal(1, results.Value.CharacterStorageOptins[0].Id);
        Assert.Equal(_timestamp, results.Value.CharacterStorageOptins[0].Timestamp);
        Assert.Equal("Approver One (001)", results.Value.CharacterStorageOptins[0].ApproverName);
        Assert.Equal("Player One (002)", results.Value.CharacterStorageOptins[0].PlayerName);
        Assert.Equal(20, results.Value.CharacterStorageOptins[0].Amount);

        Assert.Equal(2, results.Value.CharacterStorageOptins[1].Id);
        Assert.Equal(_timestamp.AddMinutes(5), results.Value.CharacterStorageOptins[1].Timestamp);
        Assert.Equal("Approver Two (003)", results.Value.CharacterStorageOptins[1].ApproverName);
        Assert.Equal("Player Two (004)", results.Value.CharacterStorageOptins[1].PlayerName);
        Assert.Equal(25, results.Value.CharacterStorageOptins[1].Amount);
    }

    [Fact]
    public async Task UseCase_WillNotGetCharacterStorageOptins_WhenValidationFails()
    {
        A.CallTo(() => _eventRepository.IsExistingEvent(_eventId)).Returns(false);

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _eventCheckinRepository.GetActiveEventInfoOrDefaultAsync())
            .MustNotHaveHappened();

        A.CallTo(() => _eventCheckinRepository.GetCharacterStorageUsersForEvent(A<int>._))
            .MustNotHaveHappened();
    }
}
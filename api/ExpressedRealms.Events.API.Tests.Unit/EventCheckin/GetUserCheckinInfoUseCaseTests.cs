using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class GetUserCheckinInfoUseCaseTests
{
    private readonly GetUserCheckinInfoUseCase _useCase;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private readonly Guid _playerId = Guid.NewGuid();

    public GetUserCheckinInfoUseCaseTests()
    {
        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();

        
        A.CallTo(() => _eventCheckinRepository.GetPlayerLookupId()).Returns("123456AB");
        A.CallTo(() => _eventCheckinRepository.GetActiveEventId()).Returns(2);
        A.CallTo(() => _eventCheckinRepository.GetCurrentPlayerId()).Returns(_playerId);
        A.CallTo(() => _eventCheckinRepository.GetCheckinAsync(2, _playerId)).Returns(new Checkin()
        {
            Id = 4
        });
        A.CallTo(() => _eventCheckinRepository.GetCurrentStage(4)).Returns(new BasicInfo()
        {
            Name = "Test",
            Id = 3
        });

        _useCase = new GetUserCheckinInfoUseCase(_eventCheckinRepository);
    }

    [Fact]
    public async Task UseCase_WillFail_IfThereIsNoActiveEvent()
    {
        A.CallTo(() => _eventCheckinRepository.GetActiveEventId())
            .Returns(Task.FromResult<int?>(null));

        var results = await _useCase.ExecuteAsync();
        Assert.False(results.IsSuccess);
        Assert.Equal("No Active Event Found", results.Errors.First().Message);
    }

    [Fact]
    public async Task UseCase_WillReturn_PlayersLookupId()
    {
        var results = await _useCase.ExecuteAsync();
        Assert.Equal("123456AB", results.Value.LookupId);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheActiveEventId()
    {
        var results = await _useCase.ExecuteAsync();
        Assert.Equal(2, results.Value.EventId);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCurrentStage()
    {
        var results = await _useCase.ExecuteAsync();
        Assert.Equal("Test", results.Value.CheckinStage!.Name);
        Assert.Equal(3, results.Value.CheckinStage.Id);
    }
    
    [Fact]
    public async Task UseCase_CanHandleNull_CurrentStage()
    {
        A.CallTo(() => _eventCheckinRepository.GetCurrentStage(4)).Returns(Task.FromResult<BasicInfo?>(null));

        var results = await _useCase.ExecuteAsync();
        Assert.Null(results.Value.CheckinStage);
    }
    
    [Fact]
    public async Task UseCase_CanHandleNull_Checkin()
    {
        A.CallTo(() => _eventCheckinRepository.GetCheckinAsync(2, _playerId)).Returns(Task.FromResult<Checkin?>(null));

        var results = await _useCase.ExecuteAsync();
        Assert.Null(results.Value.CheckinStage);
        A.CallTo(() => _eventCheckinRepository.GetCurrentStage(A<int>._)).MustNotHaveHappened();
    }
}

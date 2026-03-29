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
    private readonly int _checkinId = 4;
    
    public GetUserCheckinInfoUseCaseTests()
    {
        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();
        
        A.CallTo(() => _eventCheckinRepository.GetPlayerInfoForPlayerCheckinPage())
            .Returns(new UserCheckinPageDto()
            {
                LookupId = "123456AB",
                CheckinId = _checkinId,
                SendPickupCrbEmail = true,
                EventName = "Test Event"
            });
        A.CallTo(() => _eventCheckinRepository.GetCurrentStage(4))
            .Returns(new BasicInfo() { Name = "Test", Id = 3 });

        _useCase = new GetUserCheckinInfoUseCase(_eventCheckinRepository);
    }

    [Fact]
    public async Task UseCase_WillFail_IfThereIsNoActiveEvent()
    {
        A.CallTo(() => _eventCheckinRepository.GetPlayerInfoForPlayerCheckinPage())
            .Returns(new UserCheckinPageDto()
            {
                LookupId = "123456AB",
                CheckinId = _checkinId,
                SendPickupCrbEmail = true,
                EventName = null
            });

        var results = await _useCase.ExecuteAsync();
        Assert.False(results.IsSuccess);
        Assert.Equal("No Active Event Found", results.Errors[0].Message);
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
        Assert.Equal("Test Event", results.Value.EventName);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCurrentStage()
    {
        var results = await _useCase.ExecuteAsync();
        Assert.Equal("Test", results.Value.CheckinStage!.Name);
        Assert.Equal(3, results.Value.CheckinStage.Id);
        Assert.True(results.Value.SendPickupCrbEmail);
    }

    [Fact]
    public async Task UseCase_CanHandleNull_CurrentStage()
    {
        A.CallTo(() => _eventCheckinRepository.GetCurrentStage(4))
            .Returns(Task.FromResult<BasicInfo?>(null));

        var results = await _useCase.ExecuteAsync();
        Assert.Null(results.Value.CheckinStage);
    }

    [Fact]
    public async Task UseCase_CanHandleNull_Checkin()
    {
        A.CallTo(() => _eventCheckinRepository.GetPlayerInfoForPlayerCheckinPage())
            .Returns(new UserCheckinPageDto()
            {
                LookupId = "123456AB",
                CheckinId = null,
                SendPickupCrbEmail = false,
                EventName = "Test Event"
            });

        var results = await _useCase.ExecuteAsync();
        Assert.Null(results.Value.CheckinStage);
        A.CallTo(() => _eventCheckinRepository.GetCurrentStage(A<int>._)).MustNotHaveHappened();
    }
}

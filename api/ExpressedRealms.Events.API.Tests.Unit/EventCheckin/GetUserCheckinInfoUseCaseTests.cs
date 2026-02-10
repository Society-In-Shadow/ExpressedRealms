using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.GetUserCheckinInfo;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class GetUserCheckinInfoUseCaseTests
{
    private readonly GetUserCheckinInfoUseCase _useCase;
    private readonly IEventCheckinRepository _eventCheckinRepository;

    public GetUserCheckinInfoUseCaseTests()
    {
        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();
        
        A.CallTo(() => _eventCheckinRepository.GetPlayerLookupId())
            .Returns("123456AB");
        
        A.CallTo(() => _eventCheckinRepository.GetActiveEventId())
            .Returns(2);
        
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
}

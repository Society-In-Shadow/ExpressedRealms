using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.Get;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit;

public class GetEventsUseCaseTests
{
    private readonly GetEventsUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly List<Event> _dbModel;

    public GetEventsUseCaseTests()
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
            }
        };

        _repository = A.Fake<IEventRepository>();

        A.CallTo(() => _repository.GetEventsAsync()).Returns(_dbModel);

        _useCase = new GetEventsUseCase(_repository);
    }

    [Fact]
    public async Task UseCase_WillReturnAllItems()
    {
        
        var returnList = new List<EventModel>()
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
            }
        };
        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(returnList, results.Value.Events);
    }
}

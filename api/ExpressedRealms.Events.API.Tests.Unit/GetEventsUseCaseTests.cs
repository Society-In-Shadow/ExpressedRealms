using ExpressedRealms.Authentication;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.Get;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit;

public class GetEventsUseCaseTests
{
    private readonly GetEventsUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly List<Event> _dbModel;
    private readonly IUserContext _userContext;

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
                IsPublished = true
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
                IsPublished = false
            },
        };

        _repository = A.Fake<IEventRepository>();
        _userContext = A.Fake<IUserContext>();

        A.CallTo(() => _repository.GetEventsAsync()).Returns(_dbModel);
        A.CallTo(() => _userContext.CurrentUserHasPolicy(Policies.ManageEvents)).Returns(true);

        _useCase = new GetEventsUseCase(_repository, _userContext);
    }

    [Fact]
    public async Task UseCase_WillReturnAllItems_WhenTheyHaveManageEventsPolicy()
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
                IsPublished = true
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
                IsPublished = false
            },
        };
        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(returnList, results.Value.Events);
    }
    
    [Fact]
    public async Task UseCase_WillReturnPublishedItems_WhenTheyHaveManageEventsPolicy()
    {
        A.CallTo(() => _userContext.CurrentUserHasPolicy(Policies.ManageEvents)).Returns(false);
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
                IsPublished = true
            },
        };
        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(returnList, results.Value.Events);
    }
}

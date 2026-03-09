using System.Globalization;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.Get;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.Events;

public class GetEventsUseCaseTests
{
    private readonly GetEventsUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly List<Event> _dbModels;
    private readonly Event _dbModel;
    private readonly IUserContext _userContext;

    public GetEventsUseCaseTests()
    {
        _dbModels = new List<Event>()
        {
            new()
            {
                Id = 1,
                Name = "Test Event 1",
                Location = "Location 1",
                StartDate = DateOnly.Parse("10/31/2025", CultureInfo.InvariantCulture),
                EndDate = DateOnly.Parse("11/02/2025", CultureInfo.InvariantCulture),
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
                StartDate = DateOnly.Parse("10/30/2025", CultureInfo.InvariantCulture),
                EndDate = DateOnly.Parse("11/04/2025", CultureInfo.InvariantCulture),
                WebsiteName = "Website Name 2",
                AdditionalNotes = "Additional Notes 2",
                WebsiteUrl = "https://societyinshadows.org",
                TimeZoneId = "UTC",
                ConExperience = 25,
                IsPublished = false,
            },
        };

        _dbModel = new()
        {
            Id = 3,
            Name = "Default Event",
            Location = "Location 2",
            StartDate = DateOnly.Parse("10/30/2025", CultureInfo.InvariantCulture),
            EndDate = DateOnly.Parse("11/04/2025", CultureInfo.InvariantCulture),
            WebsiteName = "Website Name 2",
            AdditionalNotes = "Additional Notes 2",
            WebsiteUrl = "https://societyinshadows.org",
            TimeZoneId = "UTC",
            ConExperience = 25,
            IsPublished = false,
        };

        _repository = A.Fake<IEventRepository>();
        _userContext = A.Fake<IUserContext>();

        A.CallTo(() => _repository.GetEventsAsync()).Returns(_dbModels);
        A.CallTo(() => _userContext.CurrentUserHasPermission(Permissions.Event.View)).Returns(true);
        A.CallTo(() => _userContext.CurrentUserHasPermission(Permissions.Event.ModifyDefaults))
            .Returns(false);
        A.CallTo(() => _repository.GetAnyEventAsync(1)).Returns(_dbModel);

        _useCase = new GetEventsUseCase(_repository, _userContext);
    }

    [Fact]
    public async Task UseCase_WillReturnAllItems_WhenTheyHaveViewEventsPermission()
    {
        var returnList = _dbModels.Select(EventModels());

        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(returnList, results.Value.Events);
    }

    [Fact]
    public async Task UseCase_WillOnlyReturnPublishedItems_WhenTheyDoNotHaveTheViewEventPermission()
    {
        A.CallTo(() => _userContext.CurrentUserHasPermission(Permissions.Event.View))
            .Returns(false);

        var returnList = _dbModels.Where(x => x.IsPublished).Select(EventModels());

        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(returnList, results.Value.Events);
    }

    [Fact]
    public async Task UseCase_WillReturnDefaultEvent_WhenTheyHaveManageDefaultsPermission()
    {
        A.CallTo(() => _userContext.CurrentUserHasPermission(Permissions.Event.ModifyDefaults))
            .Returns(true);

        var events = _dbModels.Concat([_dbModel]).ToList();

        var returnList = events.Select(EventModels());

        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(returnList, results.Value.Events);
    }

    private static Func<Event, EventModel> EventModels()
    {
        return x => new EventModel()
        {
            Id = x.Id,
            Name = x.Name,
            Location = x.Location,
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            WebsiteName = x.WebsiteName,
            AdditionalNotes = x.AdditionalNotes,
            WebsiteUrl = x.WebsiteUrl,
            TimeZoneId = x.TimeZoneId,
            ConExperience = x.ConExperience,
            IsPublished = x.IsPublished,
        };
    }
}

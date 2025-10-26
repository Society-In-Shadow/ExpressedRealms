using ExpressedRealms.Authentication;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventScheduleItems.Get;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventScheduleItems;

public class GetEventScheduleItemUseCaseTests
{
    private readonly GetEventScheduleItemUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly GetEventScheduleItemModel _model;
    private readonly Event _dbEventModel;
    private readonly List<EventScheduleItem> _dbModels;
    private readonly IUserContext _userContext;

    public GetEventScheduleItemUseCaseTests()
    {
        _dbEventModel = new Event
        {
            StartDate = DateOnly.Parse("10/28/2025"),
            EndDate = DateOnly.Parse("10/31/2025"),
            Name = "Name",
            Location = "Location",
            WebsiteName = "website",
            WebsiteUrl = "url",
            AdditionalNotes = "additional notes",
            TimeZoneId = "TimeZoneId",
            IsPublished = true,
        };

        _dbModels = new List<EventScheduleItem>()
        {
            new()
            {
                Id = 1,
                Description = "Test Event 1",
                StartTime = TimeOnly.Parse("10:00"),
                EndTime = TimeOnly.Parse("10:30"),
                Date = DateOnly.Parse("11/02/2025"),
                EventId = 1,
            },
            new()
            {
                Id = 2,
                Description = "Test Event 2",
                StartTime = TimeOnly.Parse("11:00"),
                EndTime = TimeOnly.Parse("11:30"),
                Date = DateOnly.Parse("11/02/2025"),
                EventId = 1,
            },
        };

        _model = new GetEventScheduleItemModel() { EventId = 1 };

        _repository = A.Fake<IEventRepository>();
        _userContext = A.Fake<IUserContext>();

        A.CallTo(() => _repository.FindEventAsync(_model.EventId)).Returns(_dbEventModel);
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(true);
        A.CallTo(() => _userContext.CurrentUserHasPolicy(Policies.ManageEvents)).Returns(true);
        A.CallTo(() => _repository.GetEventScheduleItems(_model.EventId)).Returns(_dbModels);

        var validator = new GetEventScheduleItemModelValidator(_repository, _userContext);

        _useCase = new GetEventScheduleItemUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetEventScheduleItemModel.EventId),
            "Event Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEventDoesNotExist()
    {
        A.CallTo(() => _repository.FindEventAsync(_model.EventId))
            .Returns(Task.FromResult<Event?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetEventScheduleItemModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_TheyDoNotHaveManageEventsPolicy_AndItIsNotPublished()
    {
        _dbEventModel.IsPublished = false;
        A.CallTo(() => _userContext.CurrentUserHasPolicy(Policies.ManageEvents)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetEventScheduleItemModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillReturnAllItems_WhenTheyHaveManageEventsPolicy()
    {
        var returnList = new List<EventScheduleItemModel>()
        {
            new()
            {
                Id = 1,
                Description = "Test Event 1",
                StartTime = TimeOnly.Parse("10:00"),
                EndTime = TimeOnly.Parse("10:30"),
                Date = DateOnly.Parse("11/02/2025"),
                EventId = 1,
            },
            new()
            {
                Id = 2,
                Description = "Test Event 2",
                StartTime = TimeOnly.Parse("11:00"),
                EndTime = TimeOnly.Parse("11:30"),
                Date = DateOnly.Parse("11/02/2025"),
                EventId = 1,
            },
        };
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(returnList, results.Value.EventScheduleItems);
    }
}

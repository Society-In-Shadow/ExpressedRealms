using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.PublishEvent;
using ExpressedRealms.Events.API.UseCases.Events.SendEventPublishedMessages;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.Events;

public class PublishEventUseCaseTests
{
    private readonly PublishEventUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly PublishEventModel _model;
    private readonly Event _dbModel;
    private readonly ISendEventPublishedMessagesUseCase _publishMessageUseCase;

    public PublishEventUseCaseTests()
    {
        _model = new PublishEventModel()
        {
            Id = 2,
        };

        _dbModel = new Event()
        {
            Id = 2,
            Name = "Test Event",
            Location = "Location",
            StartDate = DateOnly.Parse("10/31/2025"),
            EndDate = DateOnly.Parse("11/02/2025"),
            WebsiteName = "Website Name",
            AdditionalNotes = "Additional Notes",
            WebsiteUrl = "https://societyinshadows.org",
            TimeZoneId = "UTC",
            ConExperience = 20,
        };

        _repository = A.Fake<IEventRepository>();
        _publishMessageUseCase = A.Fake<ISendEventPublishedMessagesUseCase>();

        A.CallTo(() => _repository.FindEventAsync(_model.Id)).Returns(_dbModel);

        var validator = new PublishEventModelValidator(_repository);

        _useCase = new PublishEventUseCase(_repository, validator, _publishMessageUseCase, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(PublishEventModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillReturnNotFound_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.FindEventAsync(_model.Id)).Returns(Task.FromResult<Event?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<NotFoundFailure>(
            nameof(PublishEventModel.Id),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillEditTheEvent()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditAsync(
                    A<Event>.That.Matches(k =>
                        k.IsPublished == true
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillPassThroughTheDbModel()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.EditAsync(A<Event>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillTriggerMessagePublish()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _publishMessageUseCase.ExecuteAsync(A<SendEventPublishedMessagesModel>.That.Matches(k =>
            k.Id == _model.Id
        ))).MustHaveHappenedOnceExactly();
    }
}

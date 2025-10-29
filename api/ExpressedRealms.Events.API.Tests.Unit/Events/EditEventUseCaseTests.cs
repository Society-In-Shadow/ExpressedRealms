using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.Edit;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.Events;

public class EditEventUseCaseTests
{
    private readonly EditEventUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly EditEventModel _model;
    private readonly Event _dbModel;

    public EditEventUseCaseTests()
    {
        _model = new EditEventModel()
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

        A.CallTo(() => _repository.GetEventAsync(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _repository.IsExistingEvent(_model.Id)).Returns(true);

        var validator = new EditEventModelValidator(_repository);

        _useCase = new EditEventUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditEventModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillReturnNotFound_WhenItDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.Id)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError<NotFoundFailure>(
            nameof(EditEventModel.Id),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenEmpty()
    {
        _model.Name = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditEventModel.Name), "Name is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenOver250Characters()
    {
        _model.Name = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.Name),
            "Name must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_StartDate_WillFail_WhenEmpty()
    {
        _model.StartDate = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.StartDate),
            "Start Date is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EndDate_WillFail_WhenEmpty()
    {
        _model.EndDate = default;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditEventModel.EndDate), "End Date is required.");
    }

    [Fact]
    public async Task ValidationFor_Location_WillFail_WhenEmpty()
    {
        _model.Location = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditEventModel.Location), "Location is required.");
    }

    [Fact]
    public async Task ValidationFor_Location_WillFail_WhenOver1000Characters()
    {
        _model.Location = new string('x', 1001);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.Location),
            "Location must be between 1 and 1000 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteName_WillFail_WhenEmpty()
    {
        _model.WebsiteName = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.WebsiteName),
            "Website Name is required."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteName_WillFail_WhenOver250Characters()
    {
        _model.WebsiteName = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.WebsiteName),
            "Website Name must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillFail_WhenEmpty()
    {
        _model.WebsiteUrl = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.WebsiteUrl),
            "Website Url is required."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillFail_WhenOver500Characters()
    {
        _model.WebsiteUrl = "https://" + new string('x', 495);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.WebsiteUrl),
            "Website Url must be between 1 and 500 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillFail_WhenInvalidUrl()
    {
        _model.WebsiteUrl = "not a url";

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.WebsiteUrl),
            "Website Url must be a valid URL."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillFail_WhenUnsupportedScheme()
    {
        _model.WebsiteUrl = "ftp://example.com";

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.WebsiteUrl),
            "Website Url must be a valid URL."
        );
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillPass_WhenValidHttpUrl()
    {
        _model.WebsiteUrl = "http://example.com";

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_WebsiteUrl_WillPass_WhenValidHttpsUrl()
    {
        _model.WebsiteUrl = "https://example.com";

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_AdditionalNotes_WillFail_WhenOver5000Characters()
    {
        _model.AdditionalNotes = new string('x', 5001);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.AdditionalNotes),
            "Additional Notes must be between 1 and 5000 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_TimeZoneId_WillFail_WhenEmpty()
    {
        _model.TimeZoneId = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.TimeZoneId),
            "Time Zone Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_TimeZoneId_WillFail_WhenOver250Characters()
    {
        _model.TimeZoneId = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.TimeZoneId),
            "Time Zone Id must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_TimeZoneId_WillFail_WhenItIsNotAValidTimeZoneId()
    {
        _model.TimeZoneId = "Not Time Zone";

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.TimeZoneId),
            "Time Zone Id is not a valid time zone."
        );
    }

    [Fact]
    public async Task ValidationFor_ConExperience_WillFail_WhenEmpty()
    {
        _model.ConExperience = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventModel.ConExperience),
            "Con Experience is required."
        );
    }

    [Fact]
    public async Task UseCase_WillEditTheEvent()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditAsync(
                    A<Event>.That.Matches(k =>
                        k.Name == _model.Name
                        && k.Location == _model.Location
                        && k.StartDate == _model.StartDate
                        && k.EndDate == _model.EndDate
                        && k.WebsiteName == _model.WebsiteName
                        && k.WebsiteUrl == _model.WebsiteUrl
                        && k.AdditionalNotes == _model.AdditionalNotes
                        && k.ConExperience == _model.ConExperience
                        && k.TimeZoneId == _model.TimeZoneId
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
}

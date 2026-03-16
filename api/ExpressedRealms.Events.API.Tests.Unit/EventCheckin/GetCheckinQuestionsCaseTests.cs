using ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.DB.Models.Events.Questions.QuestionTypeSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.UseCases.EventCheckin.GetCheckinQuestions;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class GetCheckinQuestionsUseCaseTests
{
    private readonly GetCheckinQuestionsUseCase _useCase;
    private readonly GetCheckinQuestionsModelValidator _validator;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private readonly IEventQuestionRepository _questionRepository;
    private readonly GetCheckinQuestionsModel _model;
    private const int EventId = 2;
    private Guid PlayerId = Guid.NewGuid();
    private const int CheckinId = 5;
    private readonly Event _event;
    private readonly List<EventQuestion> questionList;
    private readonly List<CheckinQuestionResponse> answerList;

    public GetCheckinQuestionsUseCaseTests()
    {
        _model = new GetCheckinQuestionsModel { LookupId = "ABCDEFGH" };
        _event = new Event
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
            CollectAttendeeInformation = true,
        };

        answerList = new List<CheckinQuestionResponse>()
        {
            new() { Response = "Test Response", EventQuestionId = 1 },
            new() { Response = "Test 3", EventQuestionId = 3 },
        };

        questionList = new List<EventQuestion>()
        {
            new()
            {
                Question = "Foo",
                Id = 1,
                QuestionTypeId = QuestionTypeEnum.PlayerBadgeNumber,
            },
            new()
            {
                Question = "Bar",
                Id = 3,
                QuestionTypeId = 2,
            },
            new()
            {
                Question = "Gar",
                Id = 2,
                QuestionTypeId = 1,
            },
        };

        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();
        _questionRepository = A.Fake<IEventQuestionRepository>();

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId)).Returns(true);
        A.CallTo(() => _eventCheckinRepository.GetActiveEventInfoOrDefaultAsync()).Returns(_event);

        A.CallTo(() => _eventCheckinRepository.GetPlayerId(_model.LookupId)).Returns(PlayerId);
        A.CallTo(() => _eventCheckinRepository.GetCheckinAsync(EventId, PlayerId))
            .Returns(new Checkin { Id = CheckinId });

        A.CallTo(() => _eventCheckinRepository.GetAnsweredQuestions(CheckinId)).Returns(answerList);
        A.CallTo(() => _questionRepository.GetEventQuestionsForEvent(EventId))
            .Returns(questionList);

        _validator = new GetCheckinQuestionsModelValidator(_eventCheckinRepository);

        _useCase = new GetCheckinQuestionsUseCase(
            _eventCheckinRepository,
            _questionRepository,
            _validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_LookupId_WillFail_WhenEmpty()
    {
        _model.LookupId = "";

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(nameof(_model.LookupId), "Lookup Id is required.");
    }

    [Theory]
    [InlineData("ABC")] // too short
    [InlineData("ABCDEFGHI")] // too long
    public async Task ValidationFor_LookupId_WillFail_WhenLengthIsNotEight(string invalidLookupId)
    {
        _model.LookupId = invalidLookupId;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(_model.LookupId),
            "Lookup Id must be 8 characters long."
        );
    }

    [Fact]
    public async Task ValidationFor_LookupId_WillFail_WhenDoesNotExistInRepository()
    {
        _model.LookupId = "ABCDEFGH";

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId))
            .Returns(false);

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveNotFoundError(nameof(_model.LookupId), "Lookup Id does not exist.");
    }

    [Fact]
    public async Task UseCase_WillBlock_BadgeQuestion_IfEventDoesNotHave_CollectAttendeeInfoEnabled()
    {
        _event.CollectAttendeeInformation = false;
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(
            questionList
                .Where(x => x.QuestionTypeId != QuestionTypeEnum.PlayerBadgeNumber)
                .Select(x => new QuestionResponse()
                {
                    QuestionId = x.Id,
                    Question = x.Question,
                    QuestionTypeId = x.QuestionTypeId,
                    Response = answerList.FirstOrDefault(y => y.EventQuestionId == x.Id)?.Response,
                })
                .ToList(),
            results.Value.Questions
        );
    }

    [Fact]
    public async Task UseCase_WillReturn_QuestionAnswers()
    {
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(
            questionList
                .Select(x => new QuestionResponse()
                {
                    QuestionId = x.Id,
                    Question = x.Question,
                    QuestionTypeId = x.QuestionTypeId,
                    Response = answerList.FirstOrDefault(y => y.EventQuestionId == x.Id)?.Response,
                })
                .ToList(),
            results.Value.Questions
        );
    }

    [Fact]
    public async Task UseCase_WillReturn_IfStageIsComplete()
    {
        A.CallTo(() =>
                _eventCheckinRepository.GetStageStatus(
                    CheckinId,
                    CheckinStageEnum.EventQuestionsCheck
                )
            )
            .Returns(true);

        var results = await _useCase.ExecuteAsync(_model);

        Assert.True(results.Value.HasCompletedStage);
    }
}

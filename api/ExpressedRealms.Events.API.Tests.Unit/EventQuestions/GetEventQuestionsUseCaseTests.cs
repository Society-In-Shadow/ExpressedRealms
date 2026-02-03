using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventQuestions.Get;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventQuestions;

public class GetEventQuestionUseCaseTests
{
    private readonly GetEventQuestionsUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly IEventQuestionRepository _questionRepository;
    private readonly GetEventQuestionModel _model;
    private readonly List<EventQuestion> _dbModel;

    public GetEventQuestionUseCaseTests()
    {
        _model = new GetEventQuestionModel()
        {
            EventId = 1,
        };

        _dbModel = new List<EventQuestion>()
        {
            new EventQuestion()
            {
                Question = "Foo",
                EventId = 1,
                Id = 4,
                QuestionTypeId = 5
            },
            new EventQuestion()
            {
                Question = "Goo",
                EventId = 1,
                Id = 5,
                QuestionTypeId = 4
            }
        }
;

        _repository = A.Fake<IEventRepository>();
        _questionRepository = A.Fake<IEventQuestionRepository>();

        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(true);
        A.CallTo(() => _questionRepository.GetEventQuestionsForEvent(_model.EventId))
            .Returns(_dbModel);

        var validator = new GetEventQuestionModelValidator(_repository, _questionRepository);

        _useCase = new GetEventQuestionsUseCase(
            _questionRepository,
            validator,
            CancellationToken.None
        );
    }

   [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetEventQuestionModel.EventId),
            "Event Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEventDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(
            nameof(GetEventQuestionModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillGetTheEventQuestion()
    {
        var returnList = _dbModel.Select(x => new QuestionReturnModel()
        {
            Id = x.Id,
            Question = x.Question,
            QuestionTypeId = x.QuestionTypeId
        }).ToList();
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(returnList, results.Value);
    }
}

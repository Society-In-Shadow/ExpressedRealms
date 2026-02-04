using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventQuestions.PopulateDefaults;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventQuestions;

public class PopulateDefaultQuestionsUseCaseTests
{
    private readonly PopulateDefaultQuestionsUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly IEventQuestionRepository _questionRepository;
    private readonly PopulateDefaultQuestionsModel _model;
    private readonly List<EventQuestion> _dbModel;

    public PopulateDefaultQuestionsUseCaseTests()
    {
        _model = new PopulateDefaultQuestionsModel() { EventId = 1 };

        _repository = A.Fake<IEventRepository>();
        _questionRepository = A.Fake<IEventQuestionRepository>();

        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(true);
        
        var validator = new PopulateDefaultQuestionsModelValidator(_repository, _questionRepository);

        _useCase = new PopulateDefaultQuestionsUseCase(
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
            nameof(PopulateDefaultQuestionsModel.EventId),
            "Event Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEventDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(
            nameof(PopulateDefaultQuestionsModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillGetTheEventQuestion()
    {
        var defaultQuestions = new List<EventQuestion>()
        {
            new()
            {
                EventId = _model.EventId,
                Question = "What is your badge number / name on your badge?",
                QuestionTypeId = 2
            },
            new()
            {
                EventId = _model.EventId,
                Question = "Are you under the age of 18?",
                QuestionTypeId = 1
            },
            new()
            {
                EventId = _model.EventId,
                Question = "Have you brought in a new player? If so, what is their name?",
                QuestionTypeId = 6
            },
        };
        
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _questionRepository.AddDefaultQuestionsToEvent(A<List<EventQuestion>>.That.Matches(actual =>
                actual.Count == defaultQuestions.Count &&
                actual.All(a =>
                    defaultQuestions.Any(d =>
                        d.EventId == a.EventId && d.Question == a.Question && d.QuestionTypeId == a.QuestionTypeId
                    )
                ))))
            .MustHaveHappenedOnceExactly();
    }
}

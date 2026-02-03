using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventQuestions.Create;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventQuestions;

public class CreateEventQuestionUseCaseTests
{
    private readonly CreateEventQuestionUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly IEventQuestionRepository _questionRepository;
    private readonly CreateEventQuestionModel _model;

    public CreateEventQuestionUseCaseTests()
    {
        _model = new CreateEventQuestionModel()
        {
            EventId = 1,
            Question = "Question",
            QuestionTypeId = 4,
        };
        
        _repository = A.Fake<IEventRepository>();
        _questionRepository = A.Fake<IEventQuestionRepository>();
        
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(true);
        A.CallTo(() => _questionRepository.IsExistingEventQuestion(_model.EventId, _model.Question)).Returns(false);
        A.CallTo(() => _questionRepository.IsExistingCustomizableQuestionType(_model.QuestionTypeId)).Returns(true);

        var validator = new CreateEventQuestionModelValidator(_repository, _questionRepository);

        _useCase = new CreateEventQuestionUseCase(
            _questionRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Question_WillFail_WhenEmpty()
    {
        _model.Question = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventQuestionModel.Question),
            "Question is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Question_WillFail_WhenOver250Characters()
    {
        _model.Question = new string('x', 5001);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventQuestionModel.Question),
            "Question must be between 1 and 500 characters."
        );
    }
    
    [Fact]
    public async Task ValidationFor_Question_WillFail_WhenItsADuplicateForTheEvent()
    {
        A.CallTo(() => _questionRepository.IsExistingEventQuestion(_model.EventId, _model.Question)).Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventQuestionModel.Question),
            "Question already exists."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventQuestionModel.EventId),
            "Event Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEventDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(
            nameof(CreateEventQuestionModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_QuestionTypeId_WillFail_WhenEmpty()
    {
        _model.QuestionTypeId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventQuestionModel.QuestionTypeId),
            "Question Type is required."
        );
    }

    [Fact]
    public async Task ValidationFor_QuestionTypeId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _questionRepository.IsExistingCustomizableQuestionType(_model.QuestionTypeId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateEventQuestionModel.QuestionTypeId),
            "Question Type does not exist."
        );
    }
    
    [Fact]
    public async Task UseCase_WillCreateTheEventQuestion()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _questionRepository.CreateAsync(
                    A<EventQuestion>.That.Matches(k =>
                        k.Question == _model.Question
                        && k.QuestionTypeId == _model.QuestionTypeId
                        && k.EventId == _model.EventId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_EventId_IfSuccessful()
    {
        A.CallTo(() => _questionRepository.CreateAsync(A<EventQuestion>._)).Returns(5);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.Equal(5, result.Value);
    }
}

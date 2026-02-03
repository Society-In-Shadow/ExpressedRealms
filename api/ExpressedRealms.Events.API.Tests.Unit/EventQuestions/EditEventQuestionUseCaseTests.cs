using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventQuestions.Edit;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventQuestions;

public class EditEventQuestionUseCaseTests
{
    private readonly EditEventQuestionUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly IEventQuestionRepository _questionRepository;
    private readonly EditEventQuestionModel _model;
    private readonly EventQuestion _dbModel;

    public EditEventQuestionUseCaseTests()
    {
        _model = new EditEventQuestionModel()
        {
            EventId = 1,
            Question = "Question",
            Id = 4,
        };

        _dbModel = new EventQuestion()
        {
            Question = "Foo",
            EventId = 1,
            Id = 4,
        };

        _repository = A.Fake<IEventRepository>();
        _questionRepository = A.Fake<IEventQuestionRepository>();

        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(true);
        A.CallTo(() => _questionRepository.IsExistingEventQuestion(_model.EventId, _model.Id))
            .Returns(true);
        A.CallTo(() =>
                _questionRepository.IsDuplicateEventQuestionQuestion(
                    _model.EventId,
                    _model.Id,
                    _model.Question
                )
            )
            .Returns(false);
        A.CallTo(() => _questionRepository.GetEventQuestionForEdit(_model.EventId, _model.Id))
            .Returns(_dbModel);

        var validator = new EditEventQuestionModelValidator(_repository, _questionRepository);

        _useCase = new EditEventQuestionUseCase(
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
            nameof(EditEventQuestionModel.Question),
            "Question is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Question_WillFail_WhenOver500Characters()
    {
        _model.Question = new string('x', 501);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventQuestionModel.Question),
            "Question must be between 1 and 500 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Question_WillFail_WhenItsADuplicateForTheEvent()
    {
        A.CallTo(() =>
                _questionRepository.IsDuplicateEventQuestionQuestion(
                    _model.EventId,
                    _model.Id,
                    _model.Question
                )
            )
            .Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventQuestionModel.Question),
            "Question already exists."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditEventQuestionModel.EventId),
            "Event Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEventDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(
            nameof(EditEventQuestionModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditEventQuestionModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _questionRepository.IsExistingEventQuestion(_model.EventId, _model.Id))
            .Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(
            nameof(EditEventQuestionModel.Id),
            "Question does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillEditTheEventQuestion()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _questionRepository.EditAsync(
                    A<EventQuestion>.That.Matches(k =>
                        k.Question == _model.Question
                        && k.Id == _model.Id
                        && k.EventId == _model.EventId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillPassThroughTheDbModel()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _questionRepository.EditAsync(A<EventQuestion>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }
}

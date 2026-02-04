using ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;
using ExpressedRealms.DB.Models.Events.Questions.QuestionTypeSetup;
using ExpressedRealms.Events.API.Repositories.EventQuestions;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.EventQuestions.Delete;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventQuestions;

public class DeleteEventQuestionUseCaseTests
{
    private readonly DeleteEventQuestionUseCase _useCase;
    private readonly IEventRepository _repository;
    private readonly IEventQuestionRepository _questionRepository;
    private readonly DeleteEventQuestionModel _model;
    private readonly EventQuestion _dbModel;

    public DeleteEventQuestionUseCaseTests()
    {
        _model = new DeleteEventQuestionModel() { EventId = 1, Id = 4 };

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

        A.CallTo(() => _questionRepository.GetEventQuestionForEdit(_model.EventId, _model.Id))
            .Returns(_dbModel);

        var validator = new DeleteEventQuestionModelValidator(_repository, _questionRepository);

        _useCase = new DeleteEventQuestionUseCase(
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
            nameof(DeleteEventQuestionModel.EventId),
            "Event Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenEventDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingEvent(_model.EventId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(
            nameof(DeleteEventQuestionModel.EventId),
            "Event does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteEventQuestionModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _questionRepository.IsExistingEventQuestion(_model.EventId, _model.Id))
            .Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(
            nameof(DeleteEventQuestionModel.Id),
            "Question does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillReturnFail_IfTheyDeleteMinorCheckQuestion()
    {
        _dbModel.QuestionTypeId = QuestionTypeEnum.IsMinorCheck;
        var results = await _useCase.ExecuteAsync(_model);
        Assert.False(results.IsSuccess);
        Assert.Single(results.Errors);
        Assert.Equal("Cannot delete the minor check question", results.Errors.First().Message);
    }
    
    [Fact]
    public async Task UseCase_WillReturnFail_IfTheyDeleteNewPlayerCheckQuestion()
    {
        _dbModel.QuestionTypeId = QuestionTypeEnum.BroughtNewPlayer;
        var results = await _useCase.ExecuteAsync(_model);
        Assert.False(results.IsSuccess);
        Assert.Single(results.Errors);
        Assert.Equal("Cannot delete the new player question", results.Errors.First().Message);
    }
    
    [Fact]
    public async Task UseCase_WillDeleteTheEventQuestion()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _questionRepository.EditAsync(
                    A<EventQuestion>.That.Matches(k =>
                        k.Id == _model.Id && k.EventId == _model.EventId
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

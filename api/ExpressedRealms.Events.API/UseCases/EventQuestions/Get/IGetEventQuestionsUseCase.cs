using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Get;

public interface IGetEventQuestionsUseCase
    : IGenericUseCase<Result<List<QuestionReturnModel>>, GetEventQuestionModel> { }

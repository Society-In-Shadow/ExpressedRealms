using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.GetQuestionResponses;

public interface IGetEventQuestionResponsesUseCase
    : IGenericUseCase<Result<List<AnsweredQuestionReturnModel>>, GetEventQuestionResponsesModel> { }

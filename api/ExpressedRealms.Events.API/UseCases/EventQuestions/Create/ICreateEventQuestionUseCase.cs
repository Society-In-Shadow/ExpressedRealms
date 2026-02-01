using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.Create;

public interface ICreateEventQuestionUseCase
    : IGenericUseCase<Result<int>, CreateEventQuestionModel> { }

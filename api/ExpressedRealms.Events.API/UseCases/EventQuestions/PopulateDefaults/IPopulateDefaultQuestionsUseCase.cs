using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventQuestions.PopulateDefaults;

public interface IPopulateDefaultQuestionsUseCase
    : IGenericUseCase<Result, PopulateDefaultQuestionsModel> { }

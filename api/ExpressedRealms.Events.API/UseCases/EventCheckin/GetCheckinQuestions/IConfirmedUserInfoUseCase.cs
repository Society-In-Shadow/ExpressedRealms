using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCheckinQuestions;

public interface IGetCheckinQuestionsUseCase
    : IGenericUseCase<Result<GetCheckinQuestionsReturnModel>, GetCheckinQuestionsModel> { }

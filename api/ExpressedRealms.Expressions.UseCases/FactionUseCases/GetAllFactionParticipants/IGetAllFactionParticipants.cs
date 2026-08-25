using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetAllFactionParticipants;

public interface IGetAllFactionParticipants
    : IGenericUseCase<Result<GetAllFactionParticipantsReturnModel>> { }

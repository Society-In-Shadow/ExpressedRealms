using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.GetPrerequisiteUseCase;

public interface IGetPrerequisiteUseCase
    : IGenericUseCase<Result<GetPrerequisiteData?>, GetPrerequisiteModel> { }

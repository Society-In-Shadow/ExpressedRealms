using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.Create;

public interface ICreateContactUseCase : IGenericUseCase<Result<int>, CreateContactModel> { }

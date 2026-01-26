using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.GetContact;

public interface IGetContactUseCase
    : IGenericUseCase<Result<ContactReturnModel>, GetContactModel> { }

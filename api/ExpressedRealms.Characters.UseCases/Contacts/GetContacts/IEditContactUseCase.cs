using ExpressedRealms.Characters.Repository.Contacts.Dtos;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.GetContacts;

public interface IGetContactsUseCase
    : IGenericUseCase<Result<List<ContactListReturnModel>>, GetContactsModel> { }

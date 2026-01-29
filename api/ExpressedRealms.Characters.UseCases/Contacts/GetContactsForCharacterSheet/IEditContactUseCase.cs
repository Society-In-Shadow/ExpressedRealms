using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.GetContactsForCharacterSheet;

public interface IGetContactsForCharacterSheetUseCase
    : IGenericUseCase<Result<List<ContactListReturnModel>>, GetContactsForCharacterSheetModel> { }

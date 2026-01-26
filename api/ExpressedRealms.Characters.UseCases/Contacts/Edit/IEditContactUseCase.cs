using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Contacts.Edit;

public interface IEditContactUseCase : IGenericUseCase<Result<int>, EditContactModel> { }

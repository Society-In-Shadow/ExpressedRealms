using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.SendEventPublishedMessages;

public interface ISendEventPublishedMessagesUseCase
    : IGenericUseCase<Result, SendEventPublishedMessagesModel> { }

using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.ApprovePreApprovalStageAndSendMessages;

public interface IApprovePreApprovalStageAndSendMessageUseCase
    : IGenericUseCase<Result, ApprovePreApprovalStageAndSendMessageModel> { }

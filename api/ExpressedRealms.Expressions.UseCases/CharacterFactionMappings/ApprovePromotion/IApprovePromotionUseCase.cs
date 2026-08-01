using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.ApprovePromotion;

public interface IApprovePromotionUseCase : IGenericUseCase<Result<int>, ApprovePromotionModel> { }

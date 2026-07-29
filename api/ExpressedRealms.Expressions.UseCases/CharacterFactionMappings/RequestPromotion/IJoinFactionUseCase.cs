using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.RequestPromotion;

public interface IRequestPromotionUseCase : IGenericUseCase<Result<int>, RequestPromotionModel> { }

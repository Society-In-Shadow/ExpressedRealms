using FluentResults;

namespace ExpressedRealms.UseCases.Shared.CommonFailureTypes;

public sealed class NotEnoughXPFailure(int availableXp, int amountTryingToSpend) : Error
{
    public int AvailableXP { get; set; } = availableXp;
    public int AmountTryingToSpend { get; set; } = amountTryingToSpend;
}

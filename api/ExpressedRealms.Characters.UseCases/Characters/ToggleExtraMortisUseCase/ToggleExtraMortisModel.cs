namespace ExpressedRealms.Characters.UseCases.Characters.ToggleExtraMortisUseCase;

public sealed record ToggleExtraMortisModel
{
    public int Id { get; set; }
    public bool HasExtraMortis { get; set; }
}

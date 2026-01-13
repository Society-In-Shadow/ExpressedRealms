namespace ExpressedRealms.DB.Shared;

public class GenericListDto<T>
{
    public required T Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

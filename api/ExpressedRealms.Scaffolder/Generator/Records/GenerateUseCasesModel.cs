namespace ExpressedRealms.Scaffolder.Generator.Records;

public sealed class GenerateUseCasesModel()
{
    public required string Singular { get; set; }
    public required string Plural { get; set; }
    public string? TargetFolder { get; set; }
    public List<PropertyDefinition> AdditionalProperties { get; set; } = new();
};
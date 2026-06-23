namespace ExpressedRealms.Scaffolder.Generator.Records;

public record PropertyDefinition(
    string Name,
    string Type,
    bool Required,
    int? MinValue,
    int? MaxValue
);

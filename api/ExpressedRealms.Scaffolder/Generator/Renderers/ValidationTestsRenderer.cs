using System.Text;
using ExpressedRealms.Scaffolder.Generator.Records;
using Scriban.Runtime;

namespace ExpressedRealms.Scaffolder.Generator.Renderers;

public static class ValidationTestsRenderer
{
    internal static void AddValidationTestsRenderer(this ScriptObject scriptObject)
    {
        scriptObject.Import(
            "validation_property_tests",
            new Func<IEnumerable<PropertyDefinition>, string, string>(
                (properties, targetModel) =>
                {
                    var sb = new StringBuilder();

                    var propertyDefinitions =
                        properties as PropertyDefinition[] ?? properties.ToArray();
                    foreach (var property in propertyDefinitions)
                    {
                        HandleRequiredTest(property, sb, targetModel, propertyDefinitions);
                        HandleMaxValidationTests(property, sb, targetModel, propertyDefinitions);
                        HandleMinValidationTestss(property, sb, targetModel, propertyDefinitions);

                        sb.AppendLine($"// TODO: Create unit test for {property.Name}");
                        if (propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                            sb.AppendLine();
                    }

                    return sb.ToString();
                }
            )
        );
    }

    private static void HandleRequiredTest(
        PropertyDefinition property,
        StringBuilder sb,
        string targetModel,
        PropertyDefinition[] propertyDefinitions
    )
    {
        if (!property.Required)
            return;

        var emptyValue = string.Equals(
            property.Type,
            "string",
            StringComparison.InvariantCultureIgnoreCase
        )
            ? "string.Empty"
            : "default";
        sb.AppendLine(
            $$"""
            [Fact]
            public async Task ValidationFor_{{property.Name}}_WillFail_WhenItIsEmpty()
            {
                _model.{{property.Name}} = {{emptyValue}};

                var results = await _useCase.ExecuteAsync(_model);
                results.MustHaveValidationError(nameof({{targetModel}}.{{property.Name}}), "{{property.Name}} is required.");
            }
            """
        );
        if (propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
            sb.AppendLine();
    }

    private static void HandleMaxValidationTests(
        PropertyDefinition property,
        StringBuilder sb,
        string targetModel,
        PropertyDefinition[] propertyDefinitions
    )
    {
        if (property.MaxValue is null)
            return;

        if (string.Equals(property.Type, "string", StringComparison.InvariantCultureIgnoreCase))
        {
            sb.AppendLine(
                $$"""
                [Fact]
                public async Task ValidationFor_{{property.Name}}_WillFail_When{{property.Name}}_IsOver{{property.MaxValue}}Characters()
                {
                    _model.{{property.Name}} = new string('x', {{property.MaxValue + 1}});

                    var results = await _useCase.ExecuteAsync(_model);
                    results.MustHaveValidationError(nameof({{targetModel}}.{{property.Name}}), "{{property.Name}} cannot exceed {{property.MaxValue}} characters.");
                }
                """
            );
        }
        else
        {
            sb.AppendLine(
                $$"""
                [Fact]
                public async Task ValidationFor_{{property.Name}}_WillFail_When{{property.Name}}_IsOver{{property.MaxValue}}()
                {
                    _model.{{property.Name}} = {{property.MaxValue + 1}};

                    var results = await _useCase.ExecuteAsync(_model);
                    results.MustHaveValidationError(nameof({{targetModel}}.{{property.Name}}), "{{property.Name}} cannot exceed {{property.MaxValue}}.");
                }
                """
            );
        }

        if (propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
            sb.AppendLine();
    }

    private static void HandleMinValidationTestss(
        PropertyDefinition property,
        StringBuilder sb,
        string targetModel,
        PropertyDefinition[] propertyDefinitions
    )
    {
        if (property.MinValue is null)
            return;
        if (string.Equals(property.Type, "string", StringComparison.InvariantCultureIgnoreCase))
        {
            sb.AppendLine(
                $$"""
                [Fact]
                public async Task ValidationFor_{{property.Name}}_WillFail_When{{property.Name}}_IsUnder{{property.MinValue}}Characters()
                {
                    _model.{{property.Name}} = new string('x', {{property.MinValue - 1}});

                    var results = await _useCase.ExecuteAsync(_model);
                    results.MustHaveValidationError(nameof({{targetModel}}.{{property.Name}}), "{{property.Name}} cannot be below {{property.MinValue}} characters.");
                }
                """
            );
        }
        else
        {
            sb.AppendLine(
                $$"""
                [Fact]
                public async Task ValidationFor_{{property.Name}}_WillFail_When{{property.Name}}_IsUnder{{property.MinValue}}()
                {
                    _model.{{property.Name}} = {{property.MinValue - 1}};

                    var results = await _useCase.ExecuteAsync(_model);
                    results.MustHaveValidationError(nameof({{targetModel}}.{{property.Name}}), "{{property.Name}} cannot be below {{property.MinValue}}.");
                }
                """
            );
        }
        if (propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
            sb.AppendLine();
    }
}

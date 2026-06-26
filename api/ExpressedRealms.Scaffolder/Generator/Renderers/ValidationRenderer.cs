using System.Text;
using ExpressedRealms.Scaffolder.Generator.Records;
using Scriban.Runtime;

namespace ExpressedRealms.Scaffolder.Generator.Renderers;

public static class ValidationRenderer
{
    internal static void AddValidationRuleGenerator(this ScriptObject scriptObject)
    {
        scriptObject.Import(
            "validation_rule_generator",
            new Func<IEnumerable<PropertyDefinition>, string>(
                (properties) =>
                {
                    var sb = new StringBuilder();

                    var propertyDefinitions =
                        properties as PropertyDefinition[] ?? properties.ToArray();

                    foreach (var property in propertyDefinitions)
                    {
                        sb.Append($"RuleFor(x => x.{property.Name})");

                        HandleRequiredValidation(property, sb);
                        HandleMaxLengthValidation(property, sb);
                        HandleMinLengthValidation(property, sb);

                        sb.AppendLine(";");

                        if (propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                            sb.AppendLine();
                    }

                    return sb.ToString();
                }
            )
        );
    }

    private static void HandleMinLengthValidation(PropertyDefinition property, StringBuilder sb)
    {
        if (property.MinValue == null)
            return;

        sb.Append($"{Environment.NewLine}    .MinimumLength({property.MinValue})");
        if (
            string.Equals(
                property.Type.ToLowerInvariant(),
                "string",
                StringComparison.InvariantCultureIgnoreCase
            )
        )
        {
            sb.Append(
                $"{Environment.NewLine}    .WithMessage(\"{property.Name} cannot be below {property.MinValue} characters.\")"
            );
        }
        else
        {
            sb.Append(
                $"{Environment.NewLine}    .WithMessage(\"{property.Name} cannot be below {property.MinValue}.\")"
            );
        }
    }

    private static void HandleMaxLengthValidation(PropertyDefinition property, StringBuilder sb)
    {
        if (property.MaxValue == null)
            return;

        sb.Append($"{Environment.NewLine}    .MaximumLength({property.MaxValue})");
        if (
            string.Equals(
                property.Type.ToLowerInvariant(),
                "string",
                StringComparison.InvariantCultureIgnoreCase
            )
        )
        {
            sb.Append(
                $"{Environment.NewLine}    .WithMessage(\"{property.Name} cannot exceed {property.MaxValue} characters.\")"
            );
        }
        else
        {
            sb.Append(
                $"{Environment.NewLine}    .WithMessage(\"{property.Name} cannot exceed{property.MaxValue}.\")"
            );
        }
    }

    private static void HandleRequiredValidation(PropertyDefinition property, StringBuilder sb)
    {
        if (!property.Required)
            return;

        sb.Append($"{Environment.NewLine}    .NotEmpty()");
        sb.Append($"{Environment.NewLine}    .WithMessage(\"{property.Name} is required.\")");
    }
}

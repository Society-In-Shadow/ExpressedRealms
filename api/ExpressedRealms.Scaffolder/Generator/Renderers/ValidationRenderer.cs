using System.Text;
using ExpressedRealms.Scaffolder.Generator.Records;
using Scriban.Runtime;

namespace ExpressedRealms.Scaffolder.Generator.Renderers;

public static class ValidationRenderer
{
    internal static void AddValidationPropertyTests(this ScriptObject scriptObject)
    {
        scriptObject.Import("validation_property_tests", new Func<IEnumerable<PropertyDefinition>, string, string>((properties, targetModel) =>
        {
            var sb = new StringBuilder();

            var propertyDefinitions = properties as PropertyDefinition[] ?? properties.ToArray();
            foreach (var property in propertyDefinitions)
            {
                if (property.Required)
                {
                    var emptyValue = property.Type == "string" ? "string.Empty" : "default";
                    sb.AppendLine(
                        $$"""
                          [Fact]
                          public async Task ValidationFor_{{property.Name}}_WillFail_When{{property.Name}}_IsEmpty()
                          {
                              _model.{{property.Name}} = {{emptyValue}};

                              var results = await _useCase.ExecuteAsync(_model);
                              results.MustHaveValidationError(nameof({{targetModel}}.{{property.Name}}), "{{property.Name}} is required.");
                          }
                          """
                    );
                    if(propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                        sb.AppendLine();
                }
                else
                {
                    sb.AppendLine(
                        $"// TODO: Create unit test for {property.Name}"
                    );
                    if(propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                        sb.AppendLine();
                }

            }

            return sb.ToString();
        }));
    }
}
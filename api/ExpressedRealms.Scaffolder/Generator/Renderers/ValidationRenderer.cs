using System.Text;
using ExpressedRealms.Scaffolder.Generator.Records;
using Scriban.Runtime;

namespace ExpressedRealms.Scaffolder.Generator.Renderers;

public static class ValidationRenderer
{
    internal static void AddValidationRuleGenerator(this ScriptObject scriptObject)
    {
        scriptObject.Import("validation_rule_generator", new Func<IEnumerable<PropertyDefinition>, string>((properties) =>
        {
            var sb = new StringBuilder();
            
            var propertyDefinitions = properties as PropertyDefinition[] ?? properties.ToArray();
            foreach (var property in propertyDefinitions)
            {
                sb.Append($"RuleFor(x => x.{property.Name})");
                
                if (property.Required)
                {
                    sb.Append($"{Environment.NewLine}    .NotEmpty()");
                }
                if (property.MaxValue != null)
                {
                    sb.Append($"{Environment.NewLine}    .MaximumLength({property.MaxValue})");
                }
                if (property.MinValue != null)
                {
                    sb.Append($"{Environment.NewLine}    .MinimumLength({property.MinValue})");
                }

                sb.AppendLine(";");
                
                if(propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                    sb.AppendLine();
            }

            return sb.ToString();
        }));
    }
    
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

                HandleMinMaxValues(property, sb, targetModel, propertyDefinitions);

                sb.AppendLine(
                    $"// TODO: Create unit test for {property.Name}"
                );
                if(propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                    sb.AppendLine();

            }

            return sb.ToString();
        }));
    }

    private static void HandleMinMaxValues(PropertyDefinition property, StringBuilder sb, string targetModel,
        PropertyDefinition[] propertyDefinitions)
    {
        if (property.MaxValue != null && property.Type == "string")
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
            if(propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                sb.AppendLine();
        }
                
        else if (property.MaxValue != null && property.Type == "int")
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
            if(propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                sb.AppendLine();
        }

        if (property.MinValue != null && property.Type == "string")
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
            if(propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                sb.AppendLine();
        }
        else if (property.MinValue != null && property.Type == "int")
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
            if(propertyDefinitions.IndexOf(property) != propertyDefinitions.Length - 1)
                sb.AppendLine();
        }
    }
}
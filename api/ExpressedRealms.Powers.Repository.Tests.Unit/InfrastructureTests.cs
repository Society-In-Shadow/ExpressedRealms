using System.Reflection;
using System.Text;
using ExpressedRealms.Shared;
using FluentValidation;
using NetArchTest.Rules;
using Xunit;

namespace ExpressedRealms.Powers.Repository.Tests.Unit;

public class InfrastructureTests
{
    [Fact]
    public void UseCases_MustEndIn_UseCase()
    {
        var result = Types
            .InAssembly(typeof(PowersRepositoryInjections).Assembly)
            .That()
            .ImplementInterface(typeof(IGenericUseCase<,>))
            .Should()
            .HaveNameEndingWith("UseCase")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void UseCases_ShouldNeverCall_DBContextDirectly()
    {
        var result = Types
            .InAssembly(typeof(PowersRepositoryInjections).Assembly)
            .That()
            .ImplementInterface(typeof(IGenericUseCase<,>))
            .ShouldNot()
            .HaveDependencyOn("ExpressedRealms.DB.ExpressedRealmsDbContext")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    private record TestMethodFailure(string MethodName, string ClassName);

    [Fact]
    public void TestMethods_MustStartWith_ValidationForOrUseCase()
    {
        var assemblies = AppDomain
            .CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && a.GetName().Name?.Contains("Tests") == true);

        var failures = new List<TestMethodFailure>();

        foreach (var assembly in assemblies)
        {
            var testClasses = assembly
                .GetTypes()
                .Where(t =>
                    t.Name.EndsWith("Tests")
                    && t.IsClass
                    && !t.IsAbstract
                    && !t.Name.Contains("InfrastructureTests")
                );

            foreach (var testClass in testClasses)
            {
                var methods = testClass
                    .GetMethods(
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly
                    )
                    .Where(m =>
                        m.GetCustomAttributes(typeof(FactAttribute), false).Length > 0
                        || m.GetCustomAttributes(typeof(TheoryAttribute), false).Length > 0
                    );

                failures.AddRange(
                    methods
                        .Where(x =>
                            !(x.Name.StartsWith("ValidationFor_") || x.Name.StartsWith("UseCase_"))
                        )
                        .Select(x => new TestMethodFailure(x.Name, testClass.FullName!))
                        .ToList()
                );
            }
        }

        if (failures.Count == 0)
            return;

        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("Test method naming violations found:");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Test Names must start with ValidationFor_ or UseCase_");
        stringBuilder.AppendLine();
        var failuresGroupedByClass = failures.GroupBy(x => x.ClassName);
        foreach (var className in failuresGroupedByClass)
        {
            stringBuilder.AppendLine($"Class: {className.Key}");
            stringBuilder.AppendLine();
            foreach (var failure in className)
            {
                stringBuilder.AppendLine($"  {failure.MethodName}");
            }
            stringBuilder.AppendLine();
        }

        Assert.Fail(stringBuilder.ToString());
    }

    private record ValidatorInformation(string ValidatorName, string UseCaseName);

    [Fact]
    public void UseCases_ShouldHaveCorresponding_ModelAndValidator()
    {
        var assembly = typeof(PowersRepositoryInjections).Assembly;

        var useCaseTypes = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IGenericUseCase<,>))
            .And()
            .AreClasses()
            .GetTypes();

        var modelTypes = Types
            .InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Model")
            .GetTypes()
            .Select(t => t.Name)
            .ToHashSet();

        var validators = Types
            .InAssembly(assembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .GetTypes()
            .ToHashSet();

        var validationErrors = GetValidationMessages(useCaseTypes, modelTypes, validators);

        if (validationErrors.Count == 0)
            return;

        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine(
            "One or more Use Cases are missing Models or Validators, or have incorrect validators"
        );

        stringBuilder.AppendLine();

        var failuresGroupedByClass = validationErrors.GroupBy(x => x.UseCaseName);
        foreach (var className in failuresGroupedByClass)
        {
            stringBuilder.AppendLine($"Use Case: {className.Key}");
            stringBuilder.AppendLine();
            foreach (var failure in className)
            {
                stringBuilder.AppendLine($"  {failure.ValidatorName}");
            }
            stringBuilder.AppendLine();
        }
        Assert.Fail(stringBuilder.ToString());
    }

    private static List<ValidatorInformation> GetValidationMessages(
        IEnumerable<Type> useCaseTypes,
        HashSet<string> modelTypes,
        HashSet<Type> validators
    )
    {
        var validationErrors = new List<ValidatorInformation>();
        foreach (var useCaseType in useCaseTypes)
        {
            // Extract the base name by removing "UseCase" suffix
            var baseName = useCaseType.Name.EndsWith("UseCase")
                ? useCaseType.Name.Substring(0, useCaseType.Name.Length - 7)
                : useCaseType.Name;

            var expectedModelName = baseName + "Model";

            if (!modelTypes.Contains(expectedModelName))
            {
                validationErrors.Add(
                    new ValidatorInformation(
                        $"Missing Model: {expectedModelName}",
                        useCaseType.Name
                    )
                );
            }

            var expectedValidatorName = baseName + "ModelValidator";

            var validatorNames = validators.Select(x => x.Name).ToHashSet();
            if (!validatorNames.Contains(expectedValidatorName))
            {
                validationErrors.Add(
                    new ValidatorInformation(
                        $"Missing Validator: {expectedValidatorName}",
                        useCaseType.Name
                    )
                );
            }
            else
            {
                var validator = validators.First(x => x.Name == expectedValidatorName);
                var genericArguments = validator.BaseType!.GenericTypeArguments;
                var name = genericArguments[0].Name;
                if (name != expectedModelName)
                {
                    validationErrors.Add(
                        new ValidatorInformation(
                            $"Validator '{expectedValidatorName}' should have generic argument '{expectedModelName}' instead found '{name}'",
                            useCaseType.Name
                        )
                    );
                }
            }
        }

        return validationErrors;
    }
}

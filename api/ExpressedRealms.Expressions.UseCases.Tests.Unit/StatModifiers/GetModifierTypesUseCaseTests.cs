using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.Expressions.UseCases.StatModifiers;
using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;
using ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifierTypes;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;
using DbStatModifier = ExpressedRealms.DB.Models.ModifierSystem.StatModifiers.StatModifier;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.StatModifiers;

public class GetModifierTypesUseCaseTests
{
    private readonly GetModifierTypesUseCase _useCase;
    private readonly IStatModifierRepository _repository;
    private readonly IExpressionRepository _expressionRepository;
    private readonly IUserContext _userContext;
    private readonly GetModifierTypesModel _model;
    private readonly List<DbStatModifier> _modifierTypes;
    private readonly List<ExpressionInfoForModifiersProjection> _expressions;

    public GetModifierTypesUseCaseTests()
    {
        _model = new GetModifierTypesModel()
        {
            Source = SourceTableEnum.ProgressionLevels,
        };

        _modifierTypes =
        [
            new DbStatModifier()
            {
                Id = 10,
                Name = "Body",
            },
            new DbStatModifier()
            {
                Id = 11,
                Name = "Mind",
            },
        ];

        _expressions =
        [
            new ExpressionInfoForModifiersProjection()
            {
                Id = 20,
                Name = "Expression One",
                ProgressionPaths =
                [
                    new ExpressionPathProjection()
                    {
                        Id = 30,
                        Name = "Path One",
                    },
                    new ExpressionPathProjection()
                    {
                        Id = 31,
                        Name = "Path Two",
                    },
                ],
            },
            new ExpressionInfoForModifiersProjection()
            {
                Id = 21,
                Name = "Expression Two",
                ProgressionPaths = [],
            },
        ];

        _repository = A.Fake<IStatModifierRepository>();
        _expressionRepository = A.Fake<IExpressionRepository>();
        _userContext = A.Fake<IUserContext>();

        A.CallTo(() => _repository.GetModifierTypes()).Returns(_modifierTypes);
        A.CallTo(() => _expressionRepository.GetAllEnabledExpressionAndSubpaths())
            .Returns(_expressions);

        A.CallTo(() =>
                _userContext.CurrentUserHasPermission(Permissions.ProgressionPath.EditModifiers)
            )
            .Returns(true);
        A.CallTo(() => _userContext.CurrentUserHasPermission(Permissions.Blessings.EditModifiers))
            .Returns(true);
        A.CallTo(() => _userContext.CurrentUserHasPermission(Permissions.Powers.EditModifiers))
            .Returns(true);
        A.CallTo(() =>
                _userContext.CurrentUserHasPermission(Permissions.CharacterManagement.EditModifiers)
            )
            .Returns(true);

        var validator = new GetModifierTypesModelValidator(_repository);
        var permissionChecks = new StatModifierPermissionChecks(_userContext);

        _useCase = new GetModifierTypesUseCase(
            _repository,
            permissionChecks,
            _expressionRepository,
            validator,
            CancellationToken.None
        );
    }

    public static TheoryData<SourceTableEnum> SourceTableEnums =>
        new()
        {
            SourceTableEnum.ProgressionLevels,
            SourceTableEnum.Blessings,
            SourceTableEnum.Powers,
            SourceTableEnum.Characters,
        };

    [Fact]
    public async Task ValidationFor_Source_WillFail_WhenSource_IsOutsideEnumRange()
    {
        _model.Source = (SourceTableEnum)999;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetModifierTypesModel.Source),
            "Source is not recognized as a valid value."
        );
    }
    
    [Fact]
    public async Task ValidationFor_Source_WillFail_WhenSource_IsEmpty()
    {
        _model.Source = 0;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetModifierTypesModel.Source),
            "Source is required."
        );
    }

    [Fact]
    public async Task UseCase_WillNotCheckPermissions_WhenValidationFails()
    {
        _model.Source = (SourceTableEnum)999;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _userContext.CurrentUserHasPermission(Permissions.ProgressionPath.EditModifiers)
            )
            .MustNotHaveHappened();
        A.CallTo(() => _userContext.CurrentUserHasPermission(Permissions.Blessings.EditModifiers))
            .MustNotHaveHappened();
        A.CallTo(() => _userContext.CurrentUserHasPermission(Permissions.Powers.EditModifiers))
            .MustNotHaveHappened();
        A.CallTo(() =>
                _userContext.CurrentUserHasPermission(Permissions.CharacterManagement.EditModifiers)
            )
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillNotGetOptions_WhenValidationFails()
    {
        _model.Source = (SourceTableEnum)999;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetModifierTypes()).MustNotHaveHappened();
        A.CallTo(() => _expressionRepository.GetAllEnabledExpressionAndSubpaths())
            .MustNotHaveHappened();
    }

    [Theory]
    [MemberData(nameof(SourceTableEnums))]
    public async Task UseCase_WillCheckPermission_BySourceTable(SourceTableEnum sourceTable)
    {
        _model.Source = sourceTable;

        await _useCase.ExecuteAsync(_model);

        switch (sourceTable)
        {
            case SourceTableEnum.ProgressionLevels:
                A.CallTo(() =>
                        _userContext.CurrentUserHasPermission(
                            Permissions.ProgressionPath.EditModifiers
                        )
                    )
                    .MustHaveHappenedOnceExactly();
                break;

            case SourceTableEnum.Blessings:
                A.CallTo(() =>
                        _userContext.CurrentUserHasPermission(Permissions.Blessings.EditModifiers)
                    )
                    .MustHaveHappenedOnceExactly();
                break;

            case SourceTableEnum.Powers:
                A.CallTo(() =>
                        _userContext.CurrentUserHasPermission(Permissions.Powers.EditModifiers)
                    )
                    .MustHaveHappenedOnceExactly();
                break;

            case SourceTableEnum.Characters:
                A.CallTo(() =>
                        _userContext.CurrentUserHasPermission(
                            Permissions.CharacterManagement.EditModifiers
                        )
                    )
                    .MustHaveHappenedOnceExactly();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(sourceTable), sourceTable, null);
        }
    }

    [Fact]
    public async Task UseCase_WillNotGetOptions_WhenUserDoesNotHavePermission()
    {
        A.CallTo(() =>
                _userContext.CurrentUserHasPermission(Permissions.ProgressionPath.EditModifiers)
            )
            .Returns(false);

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetModifierTypes()).MustNotHaveHappened();
        A.CallTo(() => _expressionRepository.GetAllEnabledExpressionAndSubpaths())
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillGetModifierTypes_AndExpressions()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetModifierTypes()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _expressionRepository.GetAllEnabledExpressionAndSubpaths())
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillMap_ModifierTypes()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
        Assert.Equal(_modifierTypes.Count, result.Value.ModifierTypes.Count);

        Assert.Collection(
            result.Value.ModifierTypes,
            first =>
            {
                Assert.Equal(_modifierTypes[0].Id, first.Id);
                Assert.Equal(_modifierTypes[0].Name, first.Name);
            },
            second =>
            {
                Assert.Equal(_modifierTypes[1].Id, second.Id);
                Assert.Equal(_modifierTypes[1].Name, second.Name);
            }
        );
    }

    [Fact]
    public async Task UseCase_WillMap_Expressions_AndProgressionPaths()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
        Assert.Equal(_expressions.Count, result.Value.Expressions.Count);

        Assert.Collection(
            result.Value.Expressions,
            first =>
            {
                Assert.Equal(_expressions[0].Id, first.Id);
                Assert.Equal(_expressions[0].Name, first.Name);
                Assert.Equal(_expressions[0].ProgressionPaths.Count, first.ProgressionPaths.Count);

                Assert.Collection(
                    first.ProgressionPaths,
                    firstPath =>
                    {
                        Assert.Equal(_expressions[0].ProgressionPaths[0].Id, firstPath.Id);
                        Assert.Equal(_expressions[0].ProgressionPaths[0].Name, firstPath.Name);
                    },
                    secondPath =>
                    {
                        Assert.Equal(_expressions[0].ProgressionPaths[1].Id, secondPath.Id);
                        Assert.Equal(_expressions[0].ProgressionPaths[1].Name, secondPath.Name);
                    }
                );
            },
            second =>
            {
                Assert.Equal(_expressions[1].Id, second.Id);
                Assert.Equal(_expressions[1].Name, second.Name);
                Assert.Empty(second.ProgressionPaths);
            }
        );
    }

    [Fact]
    public void UseCase_SourceTableEnums_WillCover_AllSourceTableEnumValues()
    {
        var expectedEnums = Enum.GetValues<SourceTableEnum>().Order().ToList();
        var coveredEnums = SourceTableEnums
            .Select(x => (SourceTableEnum)x)
            .Order()
            .ToList();

        Assert.Equal(expectedEnums, coveredEnums);
    }
}
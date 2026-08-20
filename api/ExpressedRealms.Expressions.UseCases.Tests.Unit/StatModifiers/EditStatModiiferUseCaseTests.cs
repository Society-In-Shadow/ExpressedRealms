using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.Expressions.UseCases.StatModifiers;
using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;
using ExpressedRealms.Expressions.UseCases.StatModifiers.Edit;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.StatModifiers;

public class EditStatModifierUseCaseTests
{
    private readonly EditStatModifierUseCase _useCase;
    private readonly IStatModifierRepository _repository;
    private readonly IExpressionRepository _expressionRepository;
    private readonly IUserContext _userContext;
    private readonly EditStatModifierModel _model;
    private readonly StatGroupMapping _dbModel;

    public EditStatModifierUseCaseTests()
    {
        _model = new EditStatModifierModel()
        {
            Id = 10,
            StatModifierGroupId = 20,
            ScaleWithLevel = true,
            Modifier = 3,
            CreationSpecificBonus = true,
            StatModifierId = 4,
            Source = SourceTableEnum.ProgressionLevels,
            TargetExpressionId = null,
            TargetProgressionPathId = null,
            Notes = "Updated notes",
        };

        _dbModel = new StatGroupMapping()
        {
            Id = _model.Id,
            StatGroupId = _model.StatModifierGroupId,
            StatModifierId = 30,
            Modifier = 1,
            ScaleWithLevel = false,
            CreationSpecificBonus = false,
            TargetExpressionId = null,
            TargetProgressionPathId = null,
            Notes = "Original notes",
        };

        _repository = A.Fake<IStatModifierRepository>();
        _expressionRepository = A.Fake<IExpressionRepository>();
        _userContext = A.Fake<IUserContext>();

        A.CallTo(() => _repository.GroupMappingExists(_model.StatModifierGroupId, _model.Id))
            .Returns(true);
        A.CallTo(() => _repository.ModifierTypeExists(_model.StatModifierId)).Returns(true);
        A.CallTo(() => _repository.GetGroupMappingForEditing(_model.Id)).Returns(_dbModel);

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

        var validator = new EditStatModifierModelValidator(_repository, _expressionRepository);
        var permissionChecks = new StatModifierPermissionChecks(_userContext);

        _useCase = new EditStatModifierUseCase(
            _repository,
            _expressionRepository,
            permissionChecks,
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
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditStatModifierModel.Id),
            "Stat Modifier Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_StatModifierGroupId_WillFail_WhenStatModifierGroupId_IsEmpty()
    {
        _model.StatModifierGroupId = 0;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditStatModifierModel.StatModifierGroupId),
            "Stat Group Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_StatModifierId_WillFail_WhenStatModifierId_IsEmpty()
    {
        _model.StatModifierId = 0;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditStatModifierModel.StatModifierId),
            "Stat Group Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_GroupMapping_WillFail_WhenStatModifierDoesNotExist()
    {
        A.CallTo(() => _repository.GroupMappingExists(_model.StatModifierGroupId, _model.Id))
            .Returns(false);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(string.Empty, "Stat Modifier does not exist.");
    }

    [Fact]
    public async Task ValidationFor_Source_WillFail_WhenSource_IsOutsideEnumRange()
    {
        _model.Source = (SourceTableEnum)999;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditStatModifierModel.Source),
            "'Source' has a range of values which does not include '999'."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenNotes_IsOver1000Characters()
    {
        _model.Notes = new string('x', 1001);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditStatModifierModel.Notes),
            "The length of 'Notes' must be 1000 characters or fewer. You entered 1001 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_TargetExpressionId_WillFail_WhenExpressionDoesNotExist()
    {
        _model.TargetExpressionId = 123;

        A.CallTo(() => _expressionRepository.GetAllEnabledExpressionAndSubpaths())
            .Returns(
                [
                    new ExpressionInfoForModifiersProjection()
                    {
                        Id = 456,
                        Name = "Different Expression",
                    },
                ]
            );

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditStatModifierModel.TargetExpressionId),
            "Expression does not exist"
        );
    }

    [Fact]
    public async Task ValidationFor_TargetProgressionPathId_WillSkipExpressionLookup_WhenTargetExpressionId_IsNull()
    {
        _model.TargetExpressionId = null;
        _model.TargetProgressionPathId = 123;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _expressionRepository.GetAllEnabledExpressionAndSubpaths())
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task ValidationFor_TargetProgressionPathId_WillFail_WhenProgressionPathDoesNotBelongToExpression()
    {
        _model.TargetExpressionId = 123;
        _model.TargetProgressionPathId = 456;

        A.CallTo(() => _expressionRepository.GetAllEnabledExpressionAndSubpaths())
            .Returns(
                [
                    new ExpressionInfoForModifiersProjection()
                    {
                        Id = _model.TargetExpressionId.Value,
                        Name = "Expression",
                        ProgressionPaths =
                        [
                            new ExpressionPathProjection()
                            {
                                Id = 789,
                                Name = "Different Progression Path",
                            },
                        ],
                    },
                ]
            );

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditStatModifierModel.TargetProgressionPathId),
            "This is not a valid progression path for the expression"
        );
    }

    [Fact]
    public async Task ValidationFor_TargetProgressionPathId_WillPass_WhenProgressionPathBelongsToExpression()
    {
        _model.TargetExpressionId = 123;
        _model.TargetProgressionPathId = 456;

        A.CallTo(() => _expressionRepository.GetAllEnabledExpressionAndSubpaths())
            .Returns(
                [
                    new ExpressionInfoForModifiersProjection()
                    {
                        Id = _model.TargetExpressionId.Value,
                        Name = "Expression",
                        ProgressionPaths =
                        [
                            new ExpressionPathProjection()
                            {
                                Id = _model.TargetProgressionPathId.Value,
                                Name = "Progression Path",
                            },
                        ],
                    },
                ]
            );

        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
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
    public async Task UseCase_WillNotUpdate_WhenUserDoesNotHavePermission()
    {
        A.CallTo(() =>
                _userContext.CurrentUserHasPermission(Permissions.ProgressionPath.EditModifiers)
            )
            .Returns(false);

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetGroupMappingForEditing(_model.Id)).MustNotHaveHappened();
        A.CallTo(() => _repository.UpdateGroupMapping(A<StatGroupMapping>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillEdit_StatGroupMapping()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetGroupMappingForEditing(_model.Id))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => _repository.UpdateGroupMapping(A<StatGroupMapping>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();

        Assert.Equal(_model.ScaleWithLevel, _dbModel.ScaleWithLevel);
        Assert.Equal(_model.Modifier, _dbModel.Modifier);
        Assert.Equal(_model.CreationSpecificBonus, _dbModel.CreationSpecificBonus);
        Assert.Equal(_model.StatModifierId, _dbModel.StatModifierId);
        Assert.Equal(_model.TargetExpressionId, _dbModel.TargetExpressionId);
        Assert.Equal(_model.TargetProgressionPathId, _dbModel.TargetProgressionPathId);
        Assert.Equal(_model.Notes, _dbModel.Notes);
    }

    [Fact]
    public async Task UseCase_WillReturnSuccess_WhenEditIsSuccessful()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void UseCase_SourceTableEnums_WillCover_AllSourceTableEnumValues()
    {
        var expectedEnums = Enum.GetValues<SourceTableEnum>().Order().ToList();
        var coveredEnums = SourceTableEnums.Select(x => (SourceTableEnum)x).Order().ToList();

        Assert.Equal(expectedEnums, coveredEnums);
    }
}

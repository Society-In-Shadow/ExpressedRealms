using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Expressions.Repository.Expressions.DTOs;
using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.Expressions.UseCases.StatModifiers;
using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.StatModifiers;

public class AddStatModifierUseCaseTests
{
    private readonly AddStatModifierUseCase _useCase;
    private readonly IStatModifierRepository _repository;
    private readonly IExpressionRepository _expressionRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IUserContext _userContext;
    private readonly AddStatModifierModel _model;

    private const int NewGroupId = 5;
    private const int NewMappingId = 10;

    public AddStatModifierUseCaseTests()
    {
        _model = new AddStatModifierModel()
        {
            SourceTable = SourceTableEnum.ProgressionLevels,
            SourceId = 1,
            StatModifierGroupId = 2,
            ScaleWithLevel = true,
            Modifier = 3,
            CreationSpecificBonus = true,
            StatModifierId = 4,
            Notes = "Test notes",
        };

        _repository = A.Fake<IStatModifierRepository>();
        _expressionRepository = A.Fake<IExpressionRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _userContext = A.Fake<IUserContext>();

        A.CallTo(() => _repository.ProgressionLevelExists(_model.SourceId)).Returns(true);
        A.CallTo(() => _repository.BlessingLevelExists(_model.SourceId)).Returns(true);
        A.CallTo(() => _repository.PowerExists(_model.SourceId)).Returns(true);
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.SourceId)).Returns(true);
        A.CallTo(() => _repository.GroupIdExists(_model.StatModifierGroupId.Value)).Returns(true);
        A.CallTo(() => _repository.ModifierTypeExists(_model.StatModifierId)).Returns(true);
        A.CallTo(() => _repository.AddGroup()).Returns(NewGroupId);
        A.CallTo(() => _repository.AddStatGroupMapping(A<StatGroupMapping>._)).Returns(NewMappingId);

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

        var validator = new AddStatModifierModelValidator(
            _repository,
            _characterRepository,
            _expressionRepository
        );
        var permissionChecks = new StatModifierPermissionChecks(_userContext);

        _useCase = new AddStatModifierUseCase(
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
    public async Task ValidationFor_SourceTable_WillFail_WhenSourceTable_IsEmpty()
    {
        _model.SourceTable = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(AddStatModifierModel.SourceTable),
            "Source Table is required."
        );
    }
    
    [Fact]
    public async Task ValidationFor_SourceTable_WillFail_WhenSourceTable_IsOutsideEnumRange()
    {
        _model.SourceTable = (SourceTableEnum)999;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(AddStatModifierModel.SourceTable),
            "Source Table is not recognized as a valid value."
        );
    }

    [Theory]
    [MemberData(nameof(SourceTableEnums))]
    public async Task ValidationFor_SourceId_WillFail_WhenSourceId_DoesNotExist(
        SourceTableEnum sourceTable
    )
    {
        _model.SourceTable = sourceTable;

        A.CallTo(() => _repository.ProgressionLevelExists(_model.SourceId)).Returns(false);
        A.CallTo(() => _repository.BlessingLevelExists(_model.SourceId)).Returns(false);
        A.CallTo(() => _repository.PowerExists(_model.SourceId)).Returns(false);
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.SourceId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            string.Empty,
            "Source Id does not exist in the Corresponding Source Table."
        );
    }

    [Theory]
    [MemberData(nameof(SourceTableEnums))]
    public async Task ValidationFor_SourceId_WillCheckCorrectRepository_BySourceTable(
        SourceTableEnum sourceTable
    )
    {
        _model.SourceTable = sourceTable;

        await _useCase.ExecuteAsync(_model);

        switch (sourceTable)
        {
            case SourceTableEnum.ProgressionLevels:
                A.CallTo(() => _repository.ProgressionLevelExists(_model.SourceId))
                    .MustHaveHappenedOnceExactly();
                break;

            case SourceTableEnum.Blessings:
                A.CallTo(() => _repository.BlessingLevelExists(_model.SourceId))
                    .MustHaveHappenedOnceExactly();
                break;

            case SourceTableEnum.Powers:
                A.CallTo(() => _repository.PowerExists(_model.SourceId))
                    .MustHaveHappenedOnceExactly();
                break;

            case SourceTableEnum.Characters:
                A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.SourceId))
                    .MustHaveHappenedOnceExactly();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(sourceTable), sourceTable, null);
        }
    }

    [Fact]
    public async Task ValidationFor_StatModifierGroupId_WillFail_WhenGroup_DoesNotExist()
    {
        A.CallTo(() => _repository.GroupIdExists(_model.StatModifierGroupId.Value)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(AddStatModifierModel.StatModifierGroupId),
            "The Group does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_StatModifierId_WillFail_WhenStatModifierId_IsEmpty()
    {
        _model.StatModifierId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(AddStatModifierModel.StatModifierId),
            "Stat Modifier Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_StatModifierId_WillFail_WhenStatModifier_DoesNotExist()
    {
        A.CallTo(() => _repository.ModifierTypeExists(_model.StatModifierId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(AddStatModifierModel.StatModifierId),
            "The Stat Modifier does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenNotes_IsOver1000Characters()
    {
        _model.Notes = new string('x', 1001);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(AddStatModifierModel.Notes),
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

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(AddStatModifierModel.TargetExpressionId),
            "Expression does not exist"
        );
    }
    
    [Fact]
    public async Task ValidationFor_TargetProgressionPathId_WillFail_WhenTargetExpressionId_IsNull()
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

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(AddStatModifierModel.TargetProgressionPathId),
            "This is not a valid progression path for the expression"
        );
    }
    
    [Theory]
    [MemberData(nameof(SourceTableEnums))]
    public async Task UseCase_WillCheckPermission_BySourceTable(SourceTableEnum sourceTable)
    {
        _model.SourceTable = sourceTable;

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
    public async Task UseCase_WillCreate_StatGroupMapping()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _repository.AddStatGroupMapping(
                    A<StatGroupMapping>.That.Matches(x =>
                        x.StatGroupId == _model.StatModifierGroupId
                        && x.Modifier == _model.Modifier
                        && x.ScaleWithLevel == _model.ScaleWithLevel
                        && x.CreationSpecificBonus == _model.CreationSpecificBonus
                        && x.StatModifierId == _model.StatModifierId
                        && x.TargetExpressionId == _model.TargetExpressionId
                        && x.TargetProgressionPathId == _model.TargetProgressionPathId
                        && x.Notes == _model.Notes
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [MemberData(nameof(SourceTableEnums))]
    public async Task UseCase_WillCreate_Group_WhenStatModifierGroupId_IsNull(
        SourceTableEnum sourceTable
    )
    {
        _model.StatModifierGroupId = null;
        _model.SourceTable = sourceTable;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.AddGroup()).MustHaveHappenedOnceExactly();
    }

    [Theory]
    [MemberData(nameof(SourceTableEnums))]
    public async Task UseCase_WillUpdate_SourceGroupId_WhenCreatingGroup_BySourceTable(
        SourceTableEnum sourceTable
    )
    {
        _model.StatModifierGroupId = null;
        _model.SourceTable = sourceTable;

        await _useCase.ExecuteAsync(_model);

        switch (sourceTable)
        {
            case SourceTableEnum.ProgressionLevels:
                A.CallTo(() => _repository.UpdateProgressionPathGroupId(_model.SourceId, NewGroupId))
                    .MustHaveHappenedOnceExactly();
                break;

            case SourceTableEnum.Blessings:
                A.CallTo(() => _repository.UpdateBlessingGroupId(_model.SourceId, NewGroupId))
                    .MustHaveHappenedOnceExactly();
                break;

            case SourceTableEnum.Powers:
                A.CallTo(() => _repository.UpdatePowerGroupId(_model.SourceId, NewGroupId))
                    .MustHaveHappenedOnceExactly();
                break;

            case SourceTableEnum.Characters:
                A.CallTo(() => _repository.UpdateCharacterGroupId(_model.SourceId, NewGroupId))
                    .MustHaveHappenedOnceExactly();
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(sourceTable), sourceTable, null);
        }
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

    [Fact]
    public async Task UseCase_WillReturn_GroupId_AndModifierMappingId_IfSuccessful()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(_model.StatModifierGroupId, result.Value.GroupId);
        Assert.Equal(NewMappingId, result.Value.ModifierMappingId);
    }

    [Theory]
    [MemberData(nameof(SourceTableEnums))]
    public async Task UseCase_WillReturn_NewGroupId_WhenStatModifierGroupId_IsNull(
        SourceTableEnum sourceTable
    )
    {
        _model.StatModifierGroupId = null;
        _model.SourceTable = sourceTable;

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(NewGroupId, result.Value.GroupId);
        Assert.Equal(NewMappingId, result.Value.ModifierMappingId);
    }
}
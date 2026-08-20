using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.Expressions.UseCases.StatModifiers;
using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;
using ExpressedRealms.Expressions.UseCases.StatModifiers.GetModifiers;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.StatModifiers;

public class GetModifiersUseCaseTests
{
    private readonly GetModifiersUseCase _useCase;
    private readonly IStatModifierRepository _repository;
    private readonly IUserContext _userContext;
    private readonly GetModifiersModel _model;
    private readonly List<StatGroupMapping> _dbModels;

    public GetModifiersUseCaseTests()
    {
        _model = new GetModifiersModel()
        {
            GroupId = 20,
            Source = SourceTableEnum.ProgressionLevels,
        };

        _dbModels =
        [
            new StatGroupMapping()
            {
                Id = 10,
                StatGroupId = _model.GroupId,
                StatModifierId = 30,
                Modifier = 4,
                ScaleWithLevel = true,
                CreationSpecificBonus = false,
                TargetExpressionId = 40,
                TargetProgressionPathId = 50,
                Notes = "First note",
            },
            new StatGroupMapping()
            {
                Id = 11,
                StatGroupId = _model.GroupId,
                StatModifierId = 31,
                Modifier = -2,
                ScaleWithLevel = false,
                CreationSpecificBonus = true,
                TargetExpressionId = null,
                TargetProgressionPathId = null,
                Notes = "Second note",
            },
        ];

        _repository = A.Fake<IStatModifierRepository>();
        _userContext = A.Fake<IUserContext>();

        A.CallTo(() => _repository.GroupIdExists(_model.GroupId)).Returns(true);
        A.CallTo(() => _repository.GetGroupMappings(_model.GroupId)).Returns(_dbModels);

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

        var validator = new GetModifiersModelValidator(_repository);
        var permissionChecks = new StatModifierPermissionChecks(_userContext);

        _useCase = new GetModifiersUseCase(
            _repository,
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
    public async Task ValidationFor_GroupId_WillFail_WhenGroupId_IsEmpty()
    {
        _model.GroupId = 0;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(nameof(GetModifiersModel.GroupId), "Group Id is required.");
    }

    [Fact]
    public async Task ValidationFor_GroupId_WillFail_WhenGroupDoesNotExist()
    {
        A.CallTo(() => _repository.GroupIdExists(_model.GroupId)).Returns(false);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(nameof(GetModifiersModel.GroupId), "Group does not exist.");
    }

    [Fact]
    public async Task ValidationFor_Source_WillFail_WhenSource_IsOutsideEnumRange()
    {
        _model.Source = (SourceTableEnum)999;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetModifiersModel.Source),
            "'Source' has a range of values which does not include '999'."
        );
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
    public async Task UseCase_WillNotGetModifiers_WhenUserDoesNotHavePermission()
    {
        A.CallTo(() =>
                _userContext.CurrentUserHasPermission(Permissions.ProgressionPath.EditModifiers)
            )
            .Returns(false);

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetGroupMappings(_model.GroupId)).MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillGetGroupMappings()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetGroupMappings(_model.GroupId)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillMap_StatGroupMappings()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
        Assert.Equal(_dbModels.Count, result.Value.Count);

        Assert.Collection(
            result.Value,
            first =>
            {
                Assert.Equal(_dbModels[0].Id, first.Id);
                Assert.Equal(_dbModels[0].Modifier, first.Modifier);
                Assert.Equal(_dbModels[0].ScaleWithLevel, first.ScaleWithLevel);
                Assert.Equal(_dbModels[0].CreationSpecificBonus, first.CreationSpecificBonus);
                Assert.Equal(_dbModels[0].StatModifierId, first.StatModifierId);
                Assert.Equal(_dbModels[0].TargetExpressionId, first.TargetExpressionId);
                Assert.Equal(_dbModels[0].TargetProgressionPathId, first.TargetProgressionPathId);
                Assert.Equal(_dbModels[0].Notes, first.Notes);
            },
            second =>
            {
                Assert.Equal(_dbModels[1].Id, second.Id);
                Assert.Equal(_dbModels[1].Modifier, second.Modifier);
                Assert.Equal(_dbModels[1].ScaleWithLevel, second.ScaleWithLevel);
                Assert.Equal(_dbModels[1].CreationSpecificBonus, second.CreationSpecificBonus);
                Assert.Equal(_dbModels[1].StatModifierId, second.StatModifierId);
                Assert.Equal(_dbModels[1].TargetExpressionId, second.TargetExpressionId);
                Assert.Equal(_dbModels[1].TargetProgressionPathId, second.TargetProgressionPathId);
                Assert.Equal(_dbModels[1].Notes, second.Notes);
            }
        );
    }

    [Fact]
    public void UseCase_SourceTableEnums_WillCover_AllSourceTableEnumValues()
    {
        var expectedEnums = Enum.GetValues<SourceTableEnum>().Order().ToList();
        var coveredEnums = SourceTableEnums.Select(x => (SourceTableEnum)x).Order().ToList();

        Assert.Equal(expectedEnums, coveredEnums);
    }
}

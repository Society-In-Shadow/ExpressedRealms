using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;
using ExpressedRealms.Expressions.Repository.StatModifier;
using ExpressedRealms.Expressions.UseCases.StatModifiers;
using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;
using ExpressedRealms.Expressions.UseCases.StatModifiers.Delete;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Expressions.UseCases.Tests.Unit.StatModifiers;

public class DeleteStatModifierUseCaseTests
{
    private readonly DeleteStatModifierUseCase _useCase;
    private readonly IStatModifierRepository _repository;
    private readonly IUserContext _userContext;
    private readonly DeleteStatModifierModel _model;
    private readonly StatGroupMapping _dbModel;

    public DeleteStatModifierUseCaseTests()
    {
        _model = new DeleteStatModifierModel()
        {
            Id = 10,
            StatModifierGroupId = 20,
            Source = SourceTableEnum.ProgressionLevels,
        };

        _dbModel = new StatGroupMapping()
        {
            Id = _model.Id,
            StatGroupId = _model.StatModifierGroupId,
            StatModifierId = 30,
            Modifier = 4,
            ScaleWithLevel = true,
            CreationSpecificBonus = false,
            Notes = "Test notes",
        };

        _repository = A.Fake<IStatModifierRepository>();
        _userContext = A.Fake<IUserContext>();

        A.CallTo(() => _repository.GroupMappingExists(_model.StatModifierGroupId, _model.Id))
            .Returns(true);
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

        var validator = new DeleteStatModifierModelValidator(_repository);
        var permissionChecks = new StatModifierPermissionChecks(_userContext);

        _useCase = new DeleteStatModifierUseCase(
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
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(DeleteStatModifierModel.Id),
            "Stat Modifier Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_StatModifierGroupId_WillFail_WhenStatModifierGroupId_IsEmpty()
    {
        _model.StatModifierGroupId = 0;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(DeleteStatModifierModel.StatModifierGroupId),
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
            nameof(DeleteStatModifierModel.Source),
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
    public async Task UseCase_WillNotDelete_WhenUserDoesNotHavePermission()
    {
        A.CallTo(() =>
                _userContext.CurrentUserHasPermission(Permissions.ProgressionPath.EditModifiers)
            )
            .Returns(false);

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetGroupMappingForEditing(_model.Id)).MustNotHaveHappened();
        A.CallTo(() => _repository.HardDeleteGroupMapping(A<StatGroupMapping>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillHardDelete_StatGroupMapping()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetGroupMappingForEditing(_model.Id))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() => _repository.HardDeleteGroupMapping(A<StatGroupMapping>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturnSuccess_WhenDeleteIsSuccessful()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void SourceTableEnums_WillCover_AllSourceTableEnumValues()
    {
        var expectedEnums = Enum.GetValues<SourceTableEnum>().Order().ToList();
        var coveredEnums = SourceTableEnums
            .Select(x => (SourceTableEnum)x)
            .Order()
            .ToList();

        Assert.Equal(expectedEnums, coveredEnums);
    }
}
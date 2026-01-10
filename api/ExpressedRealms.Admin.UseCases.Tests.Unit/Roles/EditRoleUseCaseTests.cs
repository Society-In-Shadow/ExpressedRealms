using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.UseCases.Roles.EditRole;
using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class EditRoleUseCaseTests
{
    private readonly EditRoleUseCase _useCase;
    private readonly IRolesRepository _repository;
    private readonly EditRoleModel _model;
    private readonly Guid _addPermission;

    public EditRoleUseCaseTests()
    {
        _addPermission = Guid.CreateVersion7();
        var editPermission = Guid.CreateVersion7();
        _model = new EditRoleModel()
        {
            Id = 1,
            Name = "Test Event 1",
            Description = "Location 1",
            PermissionIds = [_addPermission, editPermission],
        };

        var dbModel = new Role()
        {
            Id = 1,
            Name = "Test Event 1",
            Description = "Location 1",
            RolePermissionMappings = new List<RolePermissionMapping>()
            {
                new RolePermissionMapping() { RoleId = 1, PermissionId = _addPermission },
                new RolePermissionMapping() { RoleId = 1, PermissionId = editPermission },
            },
        };

        _repository = A.Fake<IRolesRepository>();
        var validator = new EditRoleModelValidator(_repository);

        A.CallTo(() => _repository.RoleNameExistsAsync(_model.Id, _model.Name)).Returns(false);
        A.CallTo(() => _repository.GetInvalidPermissions(_model.PermissionIds)).Returns([]);
        A.CallTo(() => _repository.RoleExistsAsync(_model.Id)).Returns(true);
        A.CallTo(() => _repository.GetRoleForEditAsync(_model.Id)).Returns(dbModel);

        _useCase = new EditRoleUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditRoleModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_DoesNotExist()
    {
        A.CallTo(() => _repository.RoleExistsAsync(_model.Id)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(EditRoleModel.Id), "Role does not exist.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenIsEmpty()
    {
        _model.Name = String.Empty;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditRoleModel.Name), "Name is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItsOver_250Characters()
    {
        _model.Name = new String('x', 251);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditRoleModel.Name),
            "Name must be at most 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItIs_ADuplicateName()
    {
        A.CallTo(() => _repository.RoleNameExistsAsync(_model.Id, _model.Name)).Returns(true);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditRoleModel.Name), "Name has already been taken.");
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItsOver_1000Characters()
    {
        _model.Description = new String('x', 1001);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditRoleModel.Description),
            "Description must be at most 1000 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_PermissionIds_WillFail_IfAnyPermissions_DoNotExist()
    {
        var returnedList = new List<Guid> { Guid.CreateVersion7(), Guid.CreateVersion7() };
        A.CallTo(() => _repository.GetInvalidPermissions(_model.PermissionIds))
            .Returns(returnedList);
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsFailed);

        var error = Assert.Single(result.Errors);
        Assert.IsType<InvalidIdsError<Guid>>(error);

        var typedError = (InvalidIdsError<Guid>)error;
        Assert.Equal(returnedList.OrderBy(x => x), typedError.InvalidPermissionIds.OrderBy(x => x));
    }

    [Fact]
    public async Task UseCase_CreatesRole_WithExpectedValues()
    {
        _model.Name = "Test Event 2";
        _model.Description = "Location 2";
        _model.PermissionIds = [_addPermission, Guid.CreateVersion7()];

        Role capturedRole = null;

        A.CallTo(() => _repository.EditAsync(A<Role>.Ignored))
            .Invokes((Role r) => capturedRole = r);

        await _useCase.ExecuteAsync(_model);

        Assert.Equal(_model.Name, capturedRole!.Name);
        Assert.Equal(_model.Description, capturedRole.Description);
        Assert.Equal(
            _model.PermissionIds.OrderBy(x => x),
            capturedRole.RolePermissionMappings.Select(rp => rp.PermissionId).OrderBy(x => x)
        );
    }
}

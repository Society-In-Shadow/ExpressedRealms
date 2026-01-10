using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.UseCases.Roles.DeleteRole;
using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class DeleteRoleUseCaseTests
{
    private readonly DeleteRoleUseCase _useCase;
    private readonly IRolesRepository _repository;
    private readonly DeleteRoleModel _model;
    private readonly Guid _addPermission;

    public DeleteRoleUseCaseTests()
    {
        _addPermission = Guid.CreateVersion7();
        var editPermission = Guid.CreateVersion7();
        _model = new DeleteRoleModel() { Id = 1 };

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
        var validator = new DeleteRoleModelValidator(_repository);

        A.CallTo(() => _repository.RoleExistsAsync(_model.Id)).Returns(true);
        A.CallTo(() => _repository.GetRoleForEditAsync(_model.Id)).Returns(dbModel);

        _useCase = new DeleteRoleUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteRoleModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_DoesNotExist()
    {
        A.CallTo(() => _repository.RoleExistsAsync(_model.Id)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(DeleteRoleModel.Id), "Role does not exist.");
    }

    [Fact]
    public async Task UseCase_DeletesRole_WithExpectedValues()
    {
        Role? capturedRole = null;

        A.CallTo(() => _repository.EditAsync(A<Role>.Ignored))
            .Invokes((Role r) => capturedRole = r);

        await _useCase.ExecuteAsync(_model);

        Assert.True(capturedRole!.IsDeleted);
    }
}

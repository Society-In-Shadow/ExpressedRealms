using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.Admin.UseCases.Roles.GetRole;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class GetRoleUseCaseTests
{
    private readonly GetRoleUseCase _useCase;
    private readonly IRolesRepository _repository;
    private readonly EditRoleDto _dbModel;
    private readonly GetRoleModel _model;

    public GetRoleUseCaseTests()
    {
        _dbModel = new EditRoleDto
        {
            Id = 2,
            Name = "Test Event 2",
            Description = "Location 2",
            PermissionIds =
            [
                Guid.CreateVersion7(),
                Guid.CreateVersion7(),
                Guid.CreateVersion7()
            ]
        };

        _model = new GetRoleModel()
        {
            Id = 1
        };

        _repository = A.Fake<IRolesRepository>();
        var validator = new GetRoleModelValidator(_repository);
        
        A.CallTo(() => _repository.RoleExistsAsync(_model.Id)).Returns(true);
        A.CallTo(() => _repository.GetRoleForEditView(_model.Id)).Returns(_dbModel);

        _useCase = new GetRoleUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetRoleModel.Id),
            "Id is required."
        );
    }
    
    [Fact]
    public async Task ValidationFor_Id_WillReturn404_WhenId_DoesNotExist()
    {
        A.CallTo(() => _repository.RoleExistsAsync(_model.Id)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(
            nameof(GetRoleModel.Id),
            "Role does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillReturnItem()
    {
        var returnList = new EditRoleDto
        {
            Id = _dbModel.Id,
            Name = _dbModel.Name,
            Description = _dbModel.Description,
            PermissionIds = _dbModel.PermissionIds
        };
        
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(returnList, results.Value);
        Assert.StrictEqual(returnList.PermissionIds, results.Value.PermissionIds);
    }
}

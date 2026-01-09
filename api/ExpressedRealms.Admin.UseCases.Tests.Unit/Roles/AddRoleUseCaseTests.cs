using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.UseCases.Roles.AddRole;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class AddRoleUseCaseTests
{
    private readonly AddRoleUseCase _useCase;
    private readonly IRolesRepository _repository;
    private readonly AddRoleModel _model;

    public AddRoleUseCaseTests()
    {
        _model = new AddRoleModel()
        {
            Name = "Test Event 2",
            Description = "Location 2",
            PermissionIds = [Guid.CreateVersion7(), Guid.CreateVersion7(), Guid.CreateVersion7()],
        };

        _repository = A.Fake<IRolesRepository>();
        var validator = new AddRoleModelValidator(_repository);

        A.CallTo(() => _repository.RoleNameExistsAsync(_model.Name)).Returns(false);
        A.CallTo(() => _repository.GetInvalidPermissions(_model.PermissionIds)).Returns([]);

        _useCase = new AddRoleUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenIsEmpty()
    {
        _model.Name = String.Empty;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(AddRoleModel.Name), "Name is required.");
    }
    
    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItsOver_250Characters()
    {
        _model.Name = new String('x', 251);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(AddRoleModel.Name), "Name must be at most 250 characters.");
    }
    
    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItIs_ADuplicateName()
    {
        A.CallTo(() => _repository.RoleNameExistsAsync(_model.Name)).Returns(true);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(AddRoleModel.Name), "Name has already been taken.");
    }
    
    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItsOver_1000Characters()
    {
        _model.Description = new String('x', 1001);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(AddRoleModel.Description), "Description must be at most 1000 characters.");
    }

    [Fact]
    public async Task ValidationFor_PermissionIds_WillFail_IfAnyPermissions_DoNotExist()
    {
        var returnedList = new List<Guid> { Guid.CreateVersion7(), Guid.CreateVersion7() };
        A.CallTo(() => _repository.GetInvalidPermissions(_model.PermissionIds)).Returns(returnedList);
        var result = await _useCase.ExecuteAsync(_model);
        // Assert
        Assert.True(result.IsFailed);

        var error = Assert.Single(result.Errors);
        Assert.IsType<InvalidIdsError<Guid>>(error);

        var typedError = (InvalidIdsError<Guid>)error;
        Assert.Equal(returnedList.OrderBy(x => x), 
            typedError.InvalidPermissionIds.OrderBy(x => x));
    }
    
    [Fact]
    public async Task UseCase_CreatesRole_WithExpectedValues()
    {
        // Act
        await _useCase.ExecuteAsync(_model);

        // Assert
        A.CallTo(() => _repository.AddAsync(
            A<Role>.That.Matches(role =>
                role.Name == _model.Name &&
                role.Description == _model.Description &&
                role.RolePermissionMappings
                    .Select(rpm => rpm.PermissionId)
                    .OrderBy(id => id)
                    .SequenceEqual(_model.PermissionIds.OrderBy(id => id))
            )
        )).MustHaveHappenedOnceExactly();
    }
}

using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.UseCases.Roles.DeleteUserRole;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class DeleteUserRoleUseCaseTests
{
    private readonly DeleteUserRoleUseCase _useCase;
    private readonly IRolesRepository _repository;
    private readonly IUsersRepository _usersRepository;
    private readonly DeleteUserRoleModel _model;

    public DeleteUserRoleUseCaseTests()
    {
        _model = new DeleteUserRoleModel() { RoleId = 5, UserId = "userId" };

        _repository = A.Fake<IRolesRepository>();
        _usersRepository = A.Fake<IUsersRepository>();
        var validator = new DeleteUserRoleModelValidator(_repository, _usersRepository);

        A.CallTo(() => _usersRepository.UserExistsAsync(_model.UserId)).Returns(true);
        A.CallTo(() => _repository.RoleExistsAsync(_model.RoleId)).Returns(true);

        _useCase = new DeleteUserRoleUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_UserId_WillFail_WhenIsEmpty()
    {
        _model.UserId = String.Empty;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteUserRoleModel.UserId), "User Id is required.");
    }

    [Fact]
    public async Task ValidationFor_UserId_WillFail_WhenItsDoesNotExist()
    {
        A.CallTo(() => _usersRepository.UserExistsAsync(_model.UserId)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(DeleteUserRoleModel.UserId), "User does not exist.");
    }

    [Fact]
    public async Task ValidationFor_RoleId_WillFail_WhenIsEmpty()
    {
        _model.RoleId = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteUserRoleModel.RoleId), "Role Id is required.");
    }

    [Fact]
    public async Task ValidationFor_RoleId_WillFail_WhenItsDoesNotExist()
    {
        A.CallTo(() => _repository.RoleExistsAsync(_model.RoleId)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(DeleteUserRoleModel.RoleId), "Role does not exist.");
    }

    [Fact]
    public async Task UseCase_CreatesRole_WithExpectedValues()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.DeleteRoleUserMappingAsync(_model.RoleId, _model.UserId))
            .MustHaveHappenedOnceExactly();
    }
}

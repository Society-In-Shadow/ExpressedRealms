using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.UseCases.Roles.AddUserToRole;
using ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class AddUserToRoleUseCaseTests
{
    private readonly AddUserToRoleUseCase _useCase;
    private readonly IRolesRepository _repository;
    private readonly IUsersRepository _usersRepository;
    private readonly AddUserToRoleModel _model;

    public AddUserToRoleUseCaseTests()
    {
        _model = new AddUserToRoleModel()
        {
            RoleId = 5,
            UserId = "userId",
            ExpireDate = new DateOnly(2200, 1, 1),
        };

        _repository = A.Fake<IRolesRepository>();
        _usersRepository = A.Fake<IUsersRepository>();
        var validator = new AddUserToRoleModelValidator(_repository, _usersRepository);

        A.CallTo(() => _usersRepository.UserExistsAsync(_model.UserId)).Returns(true);
        A.CallTo(() => _repository.RoleExistsAsync(_model.RoleId)).Returns(true);
        A.CallTo(() => _repository.RoleUserMappingExistsAsync(_model.RoleId, _model.UserId))
            .Returns(false);

        _useCase = new AddUserToRoleUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_UserId_WillFail_WhenIsEmpty()
    {
        _model.UserId = String.Empty;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(AddUserToRoleModel.UserId), "User Id is required.");
    }

    [Fact]
    public async Task ValidationFor_UserId_WillFail_WhenItsDoesNotExist()
    {
        A.CallTo(() => _usersRepository.UserExistsAsync(_model.UserId)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(AddUserToRoleModel.UserId), "User does not exist.");
    }

    [Fact]
    public async Task ValidationFor_RoleId_WillFail_WhenIsEmpty()
    {
        _model.RoleId = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(AddUserToRoleModel.RoleId), "Role Id is required.");
    }

    [Fact]
    public async Task ValidationFor_RoleId_WillFail_WhenItsDoesNotExist()
    {
        A.CallTo(() => _repository.RoleExistsAsync(_model.RoleId)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(AddUserToRoleModel.RoleId), "Role does not exist.");
    }

    [Fact]
    public async Task ValidationFor_ExpireDate_WillFail_WhenItsInThePast()
    {
        _model.ExpireDate = new DateOnly(2022, 1, 1);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(AddUserToRoleModel.ExpireDate),
            "Expiration date must be set to today or set in the future."
        );
    }

    [Fact]
    public async Task ValidationFor_ExpireDate_WillSucceed_DayOf()
    {
        _model.ExpireDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_ExpireDate_WillSucceed_InFuture()
    {
        _model.ExpireDate = DateOnly.FromDateTime(DateTime.MaxValue);
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task UseCase_WillSkipAdding_IfMappingAlreadyExists()
    {
        A.CallTo(() => _repository.RoleUserMappingExistsAsync(_model.RoleId, _model.UserId))
            .Returns(true);
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
        A.CallTo(() => _repository.AddUserRoleMappingAsync(A<UserRoleMapping>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_CreatesRole_WithExpectedValues()
    {
        // Act
        var result = await _useCase.ExecuteAsync(_model);

        // Assert
        A.CallTo(() =>
                _repository.AddUserRoleMappingAsync(
                    A<UserRoleMapping>.That.Matches(role =>
                        role.UserId == _model.UserId
                        && role.RoleId == _model.RoleId
                        && role.ExpireDate == _model.ExpireDate
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

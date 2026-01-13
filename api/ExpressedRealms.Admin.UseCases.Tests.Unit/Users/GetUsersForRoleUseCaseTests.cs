using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.Admin.UseCases.Users.GetUsersForRole;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Users;

public class GetUsersForRoleUseCaseTests
{
    private readonly GetUsersForRoleUseCase _useCase;
    private readonly IRolesRepository _repository;
    private readonly GetUsersForRoleModel _model;
    private readonly List<UserForRoleMappingDto> _dbModel;

    public GetUsersForRoleUseCaseTests()
    {
        _model = new GetUsersForRoleModel() { RoleId = 1 };

        _dbModel = new List<UserForRoleMappingDto>()
        {
            new()
            {
                UserId = "userID",
                Name = "Test Role 1",
                ExpireDate = null,
            },
            new()
            {
                UserId = "userID2",
                Name = "Test Role 2",
                ExpireDate = new DateOnly(2200, 1, 1),
            },
        };

        _repository = A.Fake<IRolesRepository>();

        var validator = new GetUsersForRoleModelValidator(_repository);

        A.CallTo(() => _repository.RoleExistsAsync(_model.RoleId)).Returns(true);
        A.CallTo(() => _repository.GetUsersForRoleAsync(_model.RoleId)).Returns(_dbModel);

        _useCase = new GetUsersForRoleUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_RoleId_WillFail_WhenIsEmpty()
    {
        _model.RoleId = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetUsersForRoleModel.RoleId),
            "Role Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_RoleId_WillFail_WhenItsDoesNotExist()
    {
        A.CallTo(() => _repository.RoleExistsAsync(_model.RoleId)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(GetUsersForRoleModel.RoleId), "Role does not exist.");
    }

    [Fact]
    public async Task UseCase_CreatesRole_WithExpectedValues()
    {
        var expected = _dbModel
            .Select(x => new RoleForUserMappingReturnModel
            {
                UserId = x.UserId,
                Name = x.Name,
                ExpireDate = x.ExpireDate,
            })
            .ToList();

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(expected, result.Value);
    }
}

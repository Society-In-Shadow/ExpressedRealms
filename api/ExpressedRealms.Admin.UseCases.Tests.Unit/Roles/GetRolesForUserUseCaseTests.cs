using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.Admin.UseCases.Roles.GetRolesForUser;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class GetRolesForUserUseCaseTests
{
    private readonly GetRolesForUserUseCase _useCase;
    private readonly IUsersRepository _usersRepository;
    private readonly GetRolesForUserModel _model;
    private readonly List<RoleForUserMappingDto> _dbModel;

    public GetRolesForUserUseCaseTests()
    {
        _model = new GetRolesForUserModel() { UserId = "userID" };

        _dbModel = new List<RoleForUserMappingDto>()
        {
            new()
            {
                Name = "Test Role 1",
                Description = "Test Description 1",
                RoleId = 1,
                ExpireDate = null,
            },
            new()
            {
                Name = "Test Role 2",
                Description = "Test Description 2",
                RoleId = 2,
                ExpireDate = new DateOnly(2200, 1, 1),
            },
        };

        var repository = A.Fake<IRolesRepository>();
        _usersRepository = A.Fake<IUsersRepository>();

        var validator = new GetRolesForUserModelValidator(_usersRepository);

        A.CallTo(() => _usersRepository.UserExistsAsync(_model.UserId)).Returns(true);
        A.CallTo(() => repository.GetRolesForUserAsync(_model.UserId)).Returns(_dbModel);

        _useCase = new GetRolesForUserUseCase(repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_UserId_WillFail_WhenIsEmpty()
    {
        _model.UserId = String.Empty;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetRolesForUserModel.UserId),
            "User Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_UserId_WillFail_WhenItsDoesNotExist()
    {
        A.CallTo(() => _usersRepository.UserExistsAsync(_model.UserId)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveNotFoundError(nameof(GetRolesForUserModel.UserId), "User does not exist.");
    }

    [Fact]
    public async Task UseCase_CreatesRole_WithExpectedValues()
    {
        var expected = _dbModel
            .Select(x => new RoleForUserMappingReturnModel
            {
                RoleId = x.RoleId,
                Name = x.Name,
                ExpireDate = x.ExpireDate,
                Description = x.Description,
            })
            .ToList();

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(expected, result.Value);
    }
}

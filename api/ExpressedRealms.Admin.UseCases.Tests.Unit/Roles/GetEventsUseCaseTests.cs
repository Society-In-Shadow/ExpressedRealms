using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.UseCases.Roles.Get;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class GetRolesUseCaseTests
{
    private readonly GetRolesUseCase _useCase;
    private readonly IRolesRepository _repository;
    private readonly List<Role> _dbModel;

    public GetRolesUseCaseTests()
    {
        _dbModel = new List<Role>
        {
            new()
            {
                Id = 1,
                Name = "Test Event 1",
                Description = "Location 1",
            },
            new()
            {
                Id = 2,
                Name = "Test Event 2",
                Description = "Location 2",
            },
        };

        _repository = A.Fake<IRolesRepository>();

        A.CallTo(() => _repository.GetRolesForListAsync()).Returns(_dbModel);

        _useCase = new GetRolesUseCase(_repository);
    }

    [Fact]
    public async Task UseCase_WillReturnAllItems()
    {
        var returnList = new List<RoleModel>()
        {
            new()
            {
                Id = 1,
                Name = "Test Event 1",
                Description = "Location 1",
            },
            new()
            {
                Id = 2,
                Name = "Test Event 2",
                Description = "Location 2",
            },
        };
        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(returnList, results.Value.Roles);
    }
}

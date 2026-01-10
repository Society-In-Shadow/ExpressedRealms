using ExpressedRealms.Admin.Repository;
using ExpressedRealms.Admin.UseCases.Roles.GetPermissions;
using ExpressedRealms.DB.Models.Authorization.PermissionResources;
using FakeItEasy;
using Xunit;
using Permission = ExpressedRealms.DB.Models.Authorization.Permissions.Permission;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class GetPermissionsUseCaseTests
{
    private readonly GetPermissionsUseCase _useCase;
    private readonly IRolesRepository _repository;
    private readonly List<PermissionResource> _dbModel;

    public GetPermissionsUseCaseTests()
    {
        _dbModel = new List<PermissionResource>
        {
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Test Event 1",
                Description = "Location 1",
                Permissions = new List<Permission>()
                {
                    new Permission()
                    {
                        Id = Guid.CreateVersion7(),
                        Name = "Test Permission 1",
                        Description = "Test Permission Description 1",
                        Key = "foo.bar"
                    },
                    new Permission()
                    {
                        Id = Guid.CreateVersion7(),
                        Name = "Test Permission 1",
                        Key = "foo.bar"
                    }
                }
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Test Event 2",
                Description = "Location 2",
                Permissions = new List<Permission>()
                {
                    new Permission()
                    {
                        Id = Guid.CreateVersion7(),
                        Name = "Test Permission 1",
                        Description = "Test Permission Description 1",
                        Key = "foo.bar"
                    }
                }
            },
        };

        _repository = A.Fake<IRolesRepository>();

        A.CallTo(() => _repository.GetPermissionResourcesForList()).Returns(_dbModel);

        _useCase = new GetPermissionsUseCase(_repository);
    }

    [Fact]
    public async Task UseCase_WillReturnAllItems()
    {
        var returnList = new PermissionsBaseReturnModel()
        {
            Resources = _dbModel
                .Select(x => new ResourceGroup()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Permissions = x.Permissions.Select(y => new UseCases.Roles.GetPermissions.Permission()
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Description = y.Description,
                    }).ToList()
                })
                .ToList(),
        };
        
        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(returnList.Resources, results.Value.Resources);
    }
}

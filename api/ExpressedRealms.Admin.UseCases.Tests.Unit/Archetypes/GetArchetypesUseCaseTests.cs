using ExpressedRealms.Admin.UseCases.Archetypes.GetArchetypes;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.DTOs;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Admin.UseCases.Tests.Unit.Roles;

public class GetArchetypesUseCaseTests
{
    private readonly GetArchetypesUseCase _useCase;
    private readonly List<ArchetypeDto> _dbModel;

    public GetArchetypesUseCaseTests()
    {
        _dbModel = new List<ArchetypeDto>
        {
            new()
            {
                Id = 1,
                Name = "Test Event 1",
                Description = "Location 1",
                ExpressionName = "Test Expression 1",
            },
            new()
            {
                Id = 2,
                Name = "Test Event 2",
                Description = "Location 2",
                ExpressionName = "Test Expression 2",
            },
        };

        var repository = A.Fake<ICharacterRepository>();

        A.CallTo(() => repository.GetBasicArchetypeListAsync()).Returns(_dbModel);

        _useCase = new GetArchetypesUseCase(repository);
    }

    [Fact]
    public async Task UseCase_WillReturnAllItems()
    {
        var returnList = _dbModel.Select(x => new ArchetypeDto()
        {
            ExpressionName = x.ExpressionName,
            Description = x.Description,
            Id = x.Id,
            Name = x.Name,
        }).ToList();
        var results = await _useCase.ExecuteAsync();

        Assert.Equivalent(returnList, results.Value.Archetypes);
    }
}

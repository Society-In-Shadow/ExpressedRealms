using ExpressedRealms.Blessings.Repository.Blessings;
using ExpressedRealms.Blessings.UseCases.Blessings.GetBlessings;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit;

public class GetBlessingsUseCaseTests
{
    private readonly GetBlessingsUseCase _useCase;
    private readonly IBlessingRepository _repository;

    private readonly List<Blessing> _blessings;

    public GetBlessingsUseCaseTests()
    {
        _blessings = new List<Blessing>()
        {
            new Blessing()
            {
                Name = "Test Name 1",
                Description = "Test Description 1",
                SubCategory = "Sub Category 1",
                Type = "Type 1",
                BlessingLevels = new List<BlessingLevel>()
                {
                    new BlessingLevel()
                    {
                        Description = "Test Description 1 Level 1",
                        Level = "2pt",
                        XpCost = 10,
                        XpGain = 10,
                    },
                    new BlessingLevel()
                    {
                        Description = "Test Description 1 Level 3",
                        Level = "4pt",
                        XpCost = 20,
                        XpGain = 20,
                    },
                    new BlessingLevel()
                    {
                        Description = "Test Description 1 Level 3",
                        Level = "6pt",
                        XpCost = 30,
                        XpGain = 0,
                    },
                },
            },
            new Blessing()
            {
                Name = "Test Name 2",
                Description = "Test Description 2",
                SubCategory = "Sub Category 2",
                Type = "Type 2",
                BlessingLevels = new List<BlessingLevel>(),
            },
        };

        _repository = A.Fake<IBlessingRepository>();

        A.CallTo(() => _repository.GetAllBlessingsAndBlessingLevels()).Returns(_blessings);

        _useCase = new GetBlessingsUseCase(_repository);
    }

    [Fact]
    public async Task UseCase_Grabs_TheBlessings()
    {
        await _useCase.ExecuteAsync();

        A.CallTo(() => _repository.GetAllBlessingsAndBlessingLevels())
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ReturnsExpectedStructure()
    {
        var results = await _useCase.ExecuteAsync();

        var blessingList = new GetBlessingsReturnModel()
        {
            Blessings = _blessings
                .Select(x => new BlessingReturnModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Type = x.Type,
                    SubCategory = x.SubCategory,
                    Levels = x
                        .BlessingLevels.Select(y => new BlessingLevelReturnModel()
                        {
                            Description = y.Description,
                            Level = y.Level,
                            XpCost = y.XpCost,
                            XpGain = y.XpGain,
                        })
                        .ToList(),
                })
                .ToList(),
        };

        Assert.Equivalent(blessingList, results.Value);
    }
}

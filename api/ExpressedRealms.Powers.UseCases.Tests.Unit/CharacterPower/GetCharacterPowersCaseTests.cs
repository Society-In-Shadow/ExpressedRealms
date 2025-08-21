using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.CharacterPower.DTO;
using ExpressedRealms.Powers.Repository.PowerPaths;
using ExpressedRealms.Powers.Repository.PowerPaths.DTOs.PowerPathToC;
using ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;
using ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers;
using ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers.ReturnModels;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;
using DetailedInformation = ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList.DetailedInformation;

namespace ExpressedRealms.Powers.UseCases.Tests.Unit.CharacterPower;

public class GetCharacterPowersUseCaseTests
{
    private readonly GetCharacterPowersUseCase _useCase;
    private readonly ICharacterPowerRepository _mappingRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IPowerPathRepository _powerPathRepository;
    private readonly GetCharacterPowersModel model;
    private readonly List<PowerPathToc> _dbModels;

    private readonly List<CharacterPowerInfo> _powerInfo;

    public GetCharacterPowersUseCaseTests()
    {
        model = new GetCharacterPowersModel() { CharacterId = 1 };

        _powerInfo = new List<CharacterPowerInfo>()
        {
            new CharacterPowerInfo() { PowerId = 1, UserNotes = "Notes" },
            new CharacterPowerInfo() { PowerId = 2, UserNotes = "Notes2" },
        };

        _mappingRepository = A.Fake<ICharacterPowerRepository>();
        _powerPathRepository = A.Fake<IPowerPathRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();

        _dbModels = new List<PowerPathToc>()
        {
            new PowerPathToc()
            {
                Id = 1,
                Name = "Path of Chi",
                Description = "Path of Chi Description",
                Powers = new List<PowerInformation>()
                {
                    new PowerInformation()
                    {
                        Name = "Power 1",
                        Description = "Description 1",
                        PowerLevel = new DetailedInformation(
                            "Power level",
                            "Power level description"
                        ),
                        AreaOfEffect = new DetailedInformation(
                            "Area of effect",
                            "Area of effect description"
                        ),
                        GameMechanicEffect = "Game Mechanic Effect",
                        PowerActivationType = new DetailedInformation(
                            "Power activation type",
                            "Power activation type description"
                        ),
                        PowerDuration = new DetailedInformation(
                            "Power duration",
                            "Power duration description"
                        ),
                        Id = 1,
                        Category = new List<DetailedInformation>()
                        {
                            new DetailedInformation("Category 1", "Category 1 description"),
                            new DetailedInformation("Category 2", "Category 2 description"),
                        },
                        Other = "Other",
                        IsPowerUse = true,
                        Cost = "Cost",
                        SortOrder = 1,
                    },
                    new PowerInformation()
                    {
                        Name = "Power 12",
                        Description = "Description 12",
                        PowerLevel = new DetailedInformation(
                            "Power level2",
                            "Power level description2"
                        ),
                        AreaOfEffect = new DetailedInformation(
                            "Area of effect2",
                            "Area of effect description"
                        ),
                        GameMechanicEffect = "Game Mechanic Effect2",
                        PowerActivationType = new DetailedInformation(
                            "Power activation type2",
                            "Power activation type 2description"
                        ),
                        PowerDuration = new DetailedInformation(
                            "Power 2duration",
                            "Power duration descr2iption"
                        ),
                        Id = 1,
                        Category = new List<DetailedInformation>()
                        {
                            new DetailedInformation("Categor2y 1", "Cate2gory 1 description"),
                            new DetailedInformation("Categ2ory 2", "Category 2 descrip2tion"),
                        },
                        Other = "Othe2r",
                        IsPowerUse = true,
                        Cost = "Co2st",
                        SortOrder = 2,
                    },
                },
            },
        };

        A.CallTo(() => _characterRepository.CharacterExistsAsync(model.CharacterId)).Returns(true);

        A.CallTo(() => _mappingRepository.GetCharacterPowerMappingInfo(model.CharacterId))
            .Returns(_powerInfo);

        var powerIds = _powerInfo.Select(x => x.PowerId).ToList();
        A.CallTo(() => _powerPathRepository.GetPowerPathAndPowers(powerIds)).Returns(_dbModels);

        var validator = new GetCharacterPowersModelValidator(_characterRepository);

        _useCase = new GetCharacterPowersUseCase(
            _mappingRepository,
            _powerPathRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(model);

        result.MustHaveValidationError(
            nameof(GetCharacterPowersModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(model.CharacterId)).Returns(false);
        var result = await _useCase.ExecuteAsync(model);

        result.MustHaveValidationError(
            nameof(GetCharacterPowersModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillGrab_ThePowers()
    {
        await _useCase.ExecuteAsync(model);
        var powerIds = _powerInfo.Select(x => x.PowerId).ToList();
        A.CallTo(() => _powerPathRepository.GetPowerPathAndPowers(powerIds))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_Powers()
    {
        var result = await _useCase.ExecuteAsync(model);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task UseCase_WillReturn_AllPowers()
    {
        var result = await _useCase.ExecuteAsync(model);

        var translatedInformation = _dbModels
            .Select(x => new PowerPathReturnModel()
            {
                Name = x.Name,
                Powers = x
                    .Powers.Select(y => new PowerReturnModel()
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Category =
                            y.Category?.Select(z => new DetailedInformationReturnModel(z)).ToList()
                            ?? new List<DetailedInformationReturnModel>(),
                        Description = y.Description,
                        GameMechanicEffect = y.GameMechanicEffect,
                        Limitation = y.Limitation,
                        PowerDuration = new DetailedInformationReturnModel(y.PowerDuration),
                        AreaOfEffect = new DetailedInformationReturnModel(y.AreaOfEffect),
                        PowerLevel = new DetailedInformationReturnModel(y.PowerLevel),
                        PowerActivationType = new DetailedInformationReturnModel(
                            y.PowerActivationType
                        ),
                        Other = y.Other,
                        IsPowerUse = y.IsPowerUse,
                        Cost = y.Cost,
                        UserNotes = _powerInfo.FirstOrDefault(z => z.PowerId == y.Id)?.UserNotes,
                        Prerequisites = y.Prerequisites is not null
                            ? new PrerequisiteDetailsReturnModel()
                            {
                                RequiredAmount = y.Prerequisites.RequiredAmount,
                                Powers = y.Prerequisites.Powers,
                            }
                            : null,
                    })
                    .ToList(),
            })
            .ToList();

        Assert.Equivalent(translatedInformation, result.Value);
    }
}

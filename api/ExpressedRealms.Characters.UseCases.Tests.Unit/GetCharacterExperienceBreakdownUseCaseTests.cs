using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.Repository.Xp.Dtos.AssignedXpInfoDtos;
using ExpressedRealms.Characters.UseCases.ExperienceBreakdown;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.Repositories.Events.Dtos;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit;

public class GetCharacterExperienceBreakdownUseCaseTests
{
    private readonly GetCharacterExperienceBreakdownUseCase _useCase;
    private readonly GetCharacterExperienceBreakdownModel _model;
    private readonly ICharacterRepository _characterRepository;
    private readonly IXpRepository _xpRepository;
    private readonly IAssignedXpMappingRepository _assignedXpRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IFeatureToggleClient _featureToggleClient;
    private readonly Guid PlayerId = Guid.NewGuid();
    private readonly List<CharacterXpView> _xpItems;

    public GetCharacterExperienceBreakdownUseCaseTests()
    {
        _model = new GetCharacterExperienceBreakdownModel() { CharacterId = 1 };

        var knowledgeRepository = A.Fake<ICharacterKnowledgeRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _xpRepository = A.Fake<IXpRepository>();
        _assignedXpRepository = A.Fake<IAssignedXpMappingRepository>();
        _featureToggleClient = A.Fake<IFeatureToggleClient>();
        _eventRepository = A.Fake<IEventRepository>();

        _xpItems = new List<CharacterXpView>()
        {
            new CharacterXpView()
            {
                SectionTypeId = 1,
                SpentXp = 1,
                SectionCap = 8,
                TotalCharacterCreationXp = 5,
                SectionName = "Knowledge XP",
            },
            new CharacterXpView()
            {
                SectionTypeId = (int)XpSectionTypes.Advantages,
                SpentXp = 4,
                SectionCap = 8,
                TotalCharacterCreationXp = 7,
                SectionName = "Advantage XP",
            },
            new CharacterXpView()
            {
                SectionTypeId = (int)XpSectionTypes.Disadvantages,
                SpentXp = 2,
                SectionCap = 8,
                TotalCharacterCreationXp = 4,
                SectionName = "Disadvantage XP",
            },
            new CharacterXpView()
            {
                SectionTypeId = (int)XpSectionTypes.Discretion,
                SpentXp = 5,
                SectionCap = 16,
                TotalCharacterCreationXp = 6,
                SectionName = "Discretionary XP",
            },
        };

        A.CallTo(() =>
                knowledgeRepository.GetExperienceSpentOnKnowledgesForCharacter(_model.CharacterId)
            )
            .Returns(1);

        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);

        A.CallTo(() => _xpRepository.GetCharacterXpMappings(_model.CharacterId)).Returns(_xpItems);

        A.CallTo(() => _xpRepository.GetAvailableDiscretionary(_model.CharacterId)).Returns(18);

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character() { PlayerId = PlayerId, IsInCharacterCreation = true });

        A.CallTo(() => _assignedXpRepository.GetAllPlayerMappingsAsync(PlayerId))
            .Returns(
                new List<XpMappingInfoDto>()
                {
                    new() { Amount = 1 },
                    new() { Amount = 3 },
                }
            );

        A.CallTo(() => _eventRepository.GetEventsWithAvailableXp())
            .Returns(
                new List<EventXpDto>()
                {
                    new() { ConExperience = 5 },
                    new() { ConExperience = 10 },
                }
            );

        var validator = new GetCharacterExperienceBreakdownModelValidator(
            knowledgeRepository,
            _characterRepository
        );

        _useCase = new GetCharacterExperienceBreakdownUseCase(
            _xpRepository,
            _characterRepository,
            _assignedXpRepository,
            _eventRepository,
            _featureToggleClient,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetCharacterExperienceBreakdownModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenIt_DoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(Task.FromResult<Character?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetCharacterExperienceBreakdownModel.CharacterId),
            "The Character Id does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectKnowledgeXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Knowledge XP");
        Assert.Equal(1, item.Total);
        Assert.Equal(8, item.Max);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectAdvantageXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Advantage XP");
        Assert.Equal(4, item.Total);
        Assert.Equal(8, item.Max);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectDisadvantageXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Disadvantage XP");
        Assert.Equal(2, item.Total);
        Assert.Equal(8, item.Max);
    }

    [Fact]
    public async Task UseCase_WillReturn1MillionXp_IfCharacteIsNotInCharacterCreation()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character() { IsInCharacterCreation = false });
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Disadvantage XP");
        Assert.Equal(2, item.Total);
        Assert.Equal(1_000_000, item.Max);
    }

    [Fact]
    public async Task UseCase_WillReturn_TotalAvailableDiscretionaryXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(18, result.Value.AvailableDiscretionary);
    }

    [Fact]
    public async Task UseCase_WithShowAssignedPanelFeatureToggle_WillSumUpAssignedXpAndEvents()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(19, result.Value.TotalAvailableXp);
    }

    [Fact]
    public async Task UseCase_WillSetMaxCapTo1Million_IfCharacterIsNotPrimaryCharacterAndNotInCreationMode()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character() { IsInCharacterCreation = false, IsPrimaryCharacter = false });

        var results = await _useCase.ExecuteAsync(_model);

        var itemCount = results.Value.ExperienceSections.Count(x => x.Max == 1_000_000);
        Assert.Equal(_xpItems.Count, itemCount);
    }

    [Fact]
    public async Task UseCase_WillSetMaxCapToSectionCap_IfCharacterIsInCreationMode()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character() { IsInCharacterCreation = true });

        var results = await _useCase.ExecuteAsync(_model);

        var maxAmounts = results.Value.ExperienceSections.Select(x => x.Max);
        var providedMaxAmounts = _xpItems.Select(x => x.SectionCap);
        Assert.Equivalent(maxAmounts, providedMaxAmounts);
    }

    /// <summary>
    ///  The XP view is grabbing all items assigned to the user, including items selected during character creation, so
    ///  when doing primary calculations, it needs to include the initial creation xp
    /// </summary>
    [Fact]
    public async Task UseCase_WillSetMaxCapToXPAvailablePlusCreationCap_IfCharacterIsPrimaryCharacterAndNotInCreationMode()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(
                new Character()
                {
                    PlayerId = PlayerId,
                    IsInCharacterCreation = false,
                    IsPrimaryCharacter = true,
                }
            );

        var results = await _useCase.ExecuteAsync(_model);

        const int totalXpAvailableToUser = 19;
        var maxAmounts = results.Value.ExperienceSections.Select(x => x.Max);
        var providedMaxAmounts = _xpItems.Select(x =>
            totalXpAvailableToUser + x.TotalCharacterCreationXp
        );
        Assert.Equivalent(maxAmounts, providedMaxAmounts);
    }
}

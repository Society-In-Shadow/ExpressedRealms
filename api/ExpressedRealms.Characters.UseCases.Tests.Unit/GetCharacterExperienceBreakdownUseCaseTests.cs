using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.UseCases.ExperienceBreakdown;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit;

public class GetCharacterExperienceBreakdownUseCaseTests
{
    private readonly GetCharacterExperienceBreakdownUseCase _useCase;
    private readonly GetCharacterExperienceBreakdownModel _model;

    public GetCharacterExperienceBreakdownUseCaseTests()
    {
        _model = new GetCharacterExperienceBreakdownModel() { CharacterId = 1 };

        var knowledgeRepository = A.Fake<ICharacterKnowledgeRepository>();
        var characterRepository = A.Fake<ICharacterRepository>();
        var xpRepository = A.Fake<IXpRepository>();

        var xpItems = new List<CharacterXpMapping>()
        {
            new CharacterXpMapping()
            {
                XpSectionTypeId = 1,
                SpentXp = 1,
                SectionCap = 8,
                XpSectionType = new XpSectionType() { Name = "Knowledge XP" }
            },
            new CharacterXpMapping()
            {
                XpSectionTypeId = (int)XpSectionTypeEnum.Advantages,
                SpentXp = 4,
                SectionCap = 8,
                XpSectionType = new XpSectionType() { Name = "Advantage XP" }
            },
            new CharacterXpMapping()
            {
                XpSectionTypeId = (int)XpSectionTypeEnum.Disadvantages,
                SpentXp = 2,
                SectionCap = 8,
                XpSectionType = new XpSectionType() { Name = "Disadvantage XP" }
            },
            new CharacterXpMapping()
            {
                XpSectionTypeId = (int)XpSectionTypeEnum.Discretion,
                SpentXp = 5,
                SectionCap = 16,
                XpSectionType = new XpSectionType() { Name = "Discretionary XP" }
            }
        };

        A.CallTo(() =>
                knowledgeRepository.GetExperienceSpentOnKnowledgesForCharacter(_model.CharacterId)
            )
            .Returns(1);

        A.CallTo(() => characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);

        A.CallTo(() => xpRepository.GetCharacterXpMappings(_model.CharacterId))
            .Returns(xpItems);

        A.CallTo(() => xpRepository.GetAvailableDiscretionary(_model.CharacterId)).Returns(18);

        var validator = new GetCharacterExperienceBreakdownModelValidator(
            knowledgeRepository,
            characterRepository
        );

        _useCase = new GetCharacterExperienceBreakdownUseCase(
            xpRepository,
            validator,
            CancellationToken.None
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
    public async Task UseCase_WillReturn_TotalAvailableDiscretionaryXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(18, result.Value.AvailableDiscretionary);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectTotalXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Total");
        Assert.Equal(5, item.Total);
        Assert.Equal(32, item.Max);
    }
}
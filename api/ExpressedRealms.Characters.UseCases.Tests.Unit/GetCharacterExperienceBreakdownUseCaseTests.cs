using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.Characters.UseCases.ExperienceBreakdown;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Powers.Repository.CharacterPower;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit;

public class GetCharacterExperienceBreakdownUseCaseTests
{
    private readonly GetCharacterExperienceBreakdownUseCase _useCase;
    private readonly GetCharacterExperienceBreakdownModel _model;
    private readonly IFeatureToggleClient _featureToggleClient;

    public GetCharacterExperienceBreakdownUseCaseTests()
    {
        _model = new GetCharacterExperienceBreakdownModel() { CharacterId = 1 };

        var knowledgeRepository = A.Fake<ICharacterKnowledgeRepository>();
        var statRepository = A.Fake<ICharacterStatRepository>();
        var skillRepository = A.Fake<ICharacterSkillRepository>();
        var characterRepository = A.Fake<ICharacterRepository>();
        var powerRepository = A.Fake<ICharacterPowerRepository>();
        var blessingRepository = A.Fake<ICharacterBlessingRepository>();
        _featureToggleClient = A.Fake<IFeatureToggleClient>();

        A.CallTo(() =>
                knowledgeRepository.GetExperienceSpentOnKnowledgesForCharacter(_model.CharacterId)
            )
            .Returns(1);
        A.CallTo(() => statRepository.GetExperienceSpentOnStatsForCharacter(_model.CharacterId))
            .Returns(2);
        A.CallTo(() => skillRepository.GetExperienceSpentOnSkillsForCharacter(_model.CharacterId))
            .Returns(3);
        A.CallTo(() => powerRepository.GetExperienceSpentOnPowersForCharacter(_model.CharacterId))
            .Returns(4);
        A.CallTo(() => blessingRepository.GetExperienceSpentOnBlessingsForCharacter(_model.CharacterId)).Returns(5);
        A.CallTo(() => blessingRepository.GetExperienceAvailableToSpendOnCharacter(_model.CharacterId)).Returns(6);
        
        A.CallTo(() => characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);
        A.CallTo(() => _featureToggleClient.HasFeatureFlag(ReleaseFlags.ManageCharacterBlessings)).Returns(false);

        var validator = new GetCharacterExperienceBreakdownModelValidator(
            knowledgeRepository,
            characterRepository
        );

        _useCase = new GetCharacterExperienceBreakdownUseCase(
            knowledgeRepository,
            statRepository,
            skillRepository,
            powerRepository,
            blessingRepository,
            _featureToggleClient,
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
        Assert.Equal(7, item.Max);
    }
    
    [Fact]
    public async Task UseCase_WillReturn_TheCorrectPowerXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Power XP");
        Assert.Equal(4, item.Total);
        Assert.Equal(20, item.Max);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectStatXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Stat XP");
        Assert.Equal(2, item.Total);
        Assert.Equal(72, item.Max);
    }
    
    [Fact]
    public async Task UseCase_WillReturn_TheCorrectSkillsXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Skills XP");
        Assert.Equal(3, item.Total);
        Assert.Equal(28, item.Max);
    }
    
    [Fact]
    public async Task UseCase_WillReturn_TheCorrectDescretionaryXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Descretionary");
        Assert.Equal(-1, item.Total);
        Assert.Equal(16, item.Max);
    }
    
    [Fact]
    public async Task UseCase_WillReturn_TheCorrectAdvantageXp()
    {
        A.CallTo(() => _featureToggleClient.HasFeatureFlag(ReleaseFlags.ManageCharacterBlessings)).Returns(true);
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Advantage XP");
        Assert.Equal(5, item.Total);
        Assert.Equal(-1, item.Max);
    }
    
    [Fact]
    public async Task UseCase_WillReturn_TheCorrectDisadvantageXp()
    {
        A.CallTo(() => _featureToggleClient.HasFeatureFlag(ReleaseFlags.ManageCharacterBlessings)).Returns(true);
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Disadvantage XP");
        Assert.Equal(-1, item.Total);
        Assert.Equal(6, item.Max);
    }
    
    [Fact]
    public async Task UseCase_WillReturn_TheCorrectTotalXp_WithBlessings()
    {
        A.CallTo(() => _featureToggleClient.HasFeatureFlag(ReleaseFlags.ManageCharacterBlessings)).Returns(true);
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Total");
        Assert.Equal(15, item.Total);
        Assert.Equal(149, item.Max);
    }
    
    [Fact]
    public async Task UseCase_WillReturn_TheCorrectTotalXp_WithoutBlessings()
    {
        var result = await _useCase.ExecuteAsync(_model);

        var item = result.Value.ExperienceSections.Single(x => x.Name == "Total");
        Assert.Equal(10, item.Total);
        Assert.Equal(143, item.Max);
    }
    
}

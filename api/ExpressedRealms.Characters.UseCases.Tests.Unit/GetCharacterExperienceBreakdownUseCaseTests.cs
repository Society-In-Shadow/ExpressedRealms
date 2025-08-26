using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.Characters.UseCases.ExperienceBreakdown;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Powers.Repository.CharacterPower;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit;

public class GetCharacterExperienceBreakdownUseCaseTests
{
    private readonly GetCharacterExperienceBreakdownUseCase _useCase;
    private readonly GetCharacterExperienceBreakdownModel _model;
    private readonly ICharacterKnowledgeRepository knowledgeRepository;
    private readonly ICharacterStatRepository statRepository;
    private readonly ICharacterSkillRepository skillRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterPowerRepository _powerRepository;

    public GetCharacterExperienceBreakdownUseCaseTests()
    {
        _model = new GetCharacterExperienceBreakdownModel() { CharacterId = 1 };

        knowledgeRepository = A.Fake<ICharacterKnowledgeRepository>();
        statRepository = A.Fake<ICharacterStatRepository>();
        skillRepository = A.Fake<ICharacterSkillRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _powerRepository = A.Fake<ICharacterPowerRepository>();

        A.CallTo(() =>
                knowledgeRepository.GetExperienceSpentOnKnowledgesForCharacter(_model.CharacterId)
            )
            .Returns(1);
        A.CallTo(() => statRepository.GetExperienceSpentOnStatsForCharacter(_model.CharacterId))
            .Returns(2);
        A.CallTo(() => skillRepository.GetExperienceSpentOnSkillsForCharacter(_model.CharacterId))
            .Returns(3);
        A.CallTo(() => _powerRepository.GetExperienceSpentOnPowersForCharacter(_model.CharacterId))
            .Returns(4);
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);

        var validator = new GetCharacterExperienceBreakdownModelValidator(
            knowledgeRepository,
            _characterRepository
        );

        _useCase = new GetCharacterExperienceBreakdownUseCase(
            knowledgeRepository,
            statRepository,
            skillRepository,
            _powerRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectKnowledgeXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(1, result.Value.KnowledgeXp);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectStatXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(2, result.Value.StatsXp);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectSkillXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(3, result.Value.SkillsXp);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectPowerXp()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(4, result.Value.PowersXp);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectKnowledgeCap()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(7, result.Value.SetupKnowledgeXp);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectStatCap()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(72, result.Value.SetupStatsXp);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectSkillCap()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(28, result.Value.SetupSkillsXp);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectPowerCap()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(20, result.Value.SetupPowersXp);
    }

    [Fact]
    public async Task UseCase_WillReturn_TheCorrectTotal()
    {
        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(10, result.Value.Total);
    }
}

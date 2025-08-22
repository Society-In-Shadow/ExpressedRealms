using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.Powers.Repository.Powers;
using ExpressedRealms.Powers.UseCases.CharacterPower.GetOptions;
using ExpressedRealms.Shared;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Powers.UseCases.Tests.Unit.CharacterPower;

public class GetKnowledgeLevelsUseCaseTests
{
    private readonly GetCharacterPowerOptionsUseCase _useCase;
    private readonly IPowerRepository _powerRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterPowerRepository _mappingRepository;
    private readonly GetCharacterPowerOptionsModel _model;

    public GetKnowledgeLevelsUseCaseTests()
    {
        _model = new GetCharacterPowerOptionsModel() { CharacterId = 1, PowerId = 2 };

        _powerRepository = A.Fake<IPowerRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _mappingRepository = A.Fake<ICharacterPowerRepository>();

        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);
        A.CallTo(() => _powerRepository.IsValidPower(_model.PowerId)).Returns(true);
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnPowersForCharacter(_model.CharacterId)
            )
            .Returns(0);

        A.CallTo(() =>
                _powerRepository.IsValidPowerForCharacter(_model.CharacterId, _model.PowerId)
            )
            .Returns(true);

        var validator = new GetCharacterPowerOptionsModelValidator(
            _characterRepository,
            _powerRepository
        );

        _useCase = new GetCharacterPowerOptionsUseCase(
            _powerRepository,
            _mappingRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetCharacterPowerOptionsModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetCharacterPowerOptionsModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItsEmpty()
    {
        _model.PowerId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetCharacterPowerOptionsModel.PowerId),
            "Power Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _powerRepository.IsValidPower(_model.PowerId)).Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetCharacterPowerOptionsModel.PowerId),
            "The Power does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_PowerId_WillFail_WhenItIsNotPartOfTheCharacterExpression()
    {
        A.CallTo(() =>
                _powerRepository.IsValidPowerForCharacter(_model.CharacterId, _model.PowerId)
            )
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetCharacterPowerOptionsModel.PowerId),
            "The Power is not part of the Expression for the Character."
        );
    }

    [Fact]
    public async Task UseCase_Returns_PowerLevelExperience()
    {
        A.CallTo(() => _powerRepository.GetPowerExperienceCost(_model.PowerId)).Returns(7);
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(7, results.Value.PowerLevelExperience);
    }

    [Fact]
    public async Task UseCase_Returns_AvailableExperience()
    {
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnPowersForCharacter(_model.CharacterId)
            )
            .Returns(5);
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(StartingExperience.StartingPowers - 5, results.Value.AvailableExperience);
    }
}

using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMapping;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Create;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit.CharacterKnowledgeMappingTests;

public class AddKnowledgeToCharacterUseCaseTests
{
    private readonly AddKnowledgeToCharacterUseCase _useCase;
    private readonly IKnowledgeRepository _knowledgeRepository;
    private readonly ICharacterKnowledgeRepository _mappingRepository;
    private readonly IKnowledgeLevelRepository _levelRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly AddModel _model;

    public AddKnowledgeToCharacterUseCaseTests()
    {
        _model = new AddModel()
        {
            KnowledgeLevelId = 1,
            CharacterId = 2,
            KnowledgeId = 3,
            Notes = "123",
        };

        _characterRepository = A.Fake<ICharacterRepository>();
        _knowledgeRepository = A.Fake<IKnowledgeRepository>();
        _mappingRepository = A.Fake<ICharacterKnowledgeRepository>();
        _levelRepository = A.Fake<IKnowledgeLevelRepository>();

        A.CallTo(() => _knowledgeRepository.IsExistingKnowledge(_model.KnowledgeId)).Returns(true);
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);
        A.CallTo(() => _levelRepository.KnowledgeLevelExists(_model.KnowledgeLevelId))
            .Returns(true);
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(_model.KnowledgeId, _model.CharacterId)
            )
            .Returns(false);
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(_model.CharacterId)
            )
            .Returns(0);
        A.CallTo(() => _knowledgeRepository.GetKnowledgeAsync(_model.KnowledgeId))
            .Returns(new Knowledge() { KnowledgeTypeId = 3 });
        A.CallTo(() => _levelRepository.GetKnowledgeLevel(_model.KnowledgeLevelId))
            .Returns(new KnowledgeEducationLevel() { GeneralXpCost = 4, UnknownXpCost = 2 });

        var validator = new AddModelValidator(
            _knowledgeRepository,
            _characterRepository,
            _mappingRepository,
            _levelRepository
        );

        _useCase = new AddKnowledgeToCharacterUseCase(
            _mappingRepository,
            _levelRepository,
            _knowledgeRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgelId_WillFail_WhenItsEmpty()
    {
        _model.KnowledgeId = 0;
        var result = await _useCase.Execute(_model);

        result.MustHaveValidationError(nameof(AddModel.KnowledgeId), "Knowledge Id is required.");
    }

    [Fact]
    public async Task ValidationFor_KnowledgeId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _knowledgeRepository.IsExistingKnowledge(_model.KnowledgeId)).Returns(false);
        var result = await _useCase.Execute(_model);

        result.MustHaveValidationError(
            nameof(AddModel.KnowledgeId),
            "The Knowledge does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.Execute(_model);

        result.MustHaveValidationError(nameof(AddModel.CharacterId), "Character Id is required.");
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId))
            .Returns(false);
        var result = await _useCase.Execute(_model);

        result.MustHaveValidationError(
            nameof(AddModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgeLevelId_WillFail_WhenItsEmpty()
    {
        _model.KnowledgeLevelId = 0;
        var result = await _useCase.Execute(_model);

        result.MustHaveValidationError(
            nameof(AddModel.KnowledgeLevelId),
            "Knowledge Level Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgeLevelId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _levelRepository.KnowledgeLevelExists(_model.KnowledgeLevelId))
            .Returns(false);
        var result = await _useCase.Execute(_model);

        result.MustHaveValidationError(
            nameof(AddModel.KnowledgeLevelId),
            "The Knowledge Level does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_DuplicateMapping_WillFail_WhenItDoesExist()
    {
        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(_model.KnowledgeId, _model.CharacterId)
            )
            .Returns(true);
        var result = await _useCase.Execute(_model);

        result.MustHaveValidationError(
            nameof(AddModel.KnowledgeId),
            "The knowledge already exists for this character."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _model.Notes = new string('x', 5001);
        var result = await _useCase.Execute(_model);
        result.MustHaveValidationError(
            nameof(AddModel.Notes),
            "Notes must be less than 5000 characters."
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ValidationFor_Notes_AreOptional(string? notes)
    {
        _model.Notes = notes;
        var result = await _useCase.Execute(_model);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UseCase_GetsExperienceSpentOnKnowledgesForCharacter()
    {
        await _useCase.Execute(_model);

        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(_model.CharacterId)
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_GetsKnowledgeLevel()
    {
        await _useCase.Execute(_model);

        A.CallTo(() => _levelRepository.GetKnowledgeLevel(_model.KnowledgeLevelId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_GetsKnowledge()
    {
        await _useCase.Execute(_model);

        A.CallTo(() => _knowledgeRepository.GetKnowledgeAsync(_model.KnowledgeId))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(2, 2)]
    [InlineData(3, 4)]
    public async Task UseCase_GrabsCorrectXp_DependingOnKnowledgeType(
        int knowledgeTypeId,
        int xpAmount
    )
    {
        A.CallTo(() => _knowledgeRepository.GetKnowledgeAsync(_model.KnowledgeId))
            .Returns(new Knowledge() { KnowledgeTypeId = knowledgeTypeId });
        A.CallTo(() => _levelRepository.GetKnowledgeLevel(_model.KnowledgeLevelId))
            .Returns(new KnowledgeEducationLevel() { GeneralXpCost = 2, UnknownXpCost = 4 });

        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(_model.CharacterId)
            )
            .Returns(100);

        var result = await _useCase.Execute(_model);

        Assert.Equal(xpAmount, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(2)]
    public async Task UseCase_CalculatesAvailableXP_Correctly(int xpAmount)
    {
        A.CallTo(() => _knowledgeRepository.GetKnowledgeAsync(_model.KnowledgeId))
            .Returns(new Knowledge() { KnowledgeTypeId = 2 });
        A.CallTo(() => _levelRepository.GetKnowledgeLevel(_model.KnowledgeLevelId))
            .Returns(new KnowledgeEducationLevel() { GeneralXpCost = 7 });
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(_model.CharacterId)
            )
            .Returns(xpAmount);

        var result = await _useCase.Execute(_model);

        Assert.Equal(7 - xpAmount, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
    }

    [Fact]
    public async Task UseCase_WillReturnNotEnoughXp_WhenOutOfXp()
    {
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(_model.CharacterId)
            )
            .Returns(7);

        var result = await _useCase.Execute(_model);
        Assert.True(result.HasError<NotEnoughXPFailure>());
        Assert.Equal(0, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
        Assert.Equal(2, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Fact]
    public async Task UseCase_AddCharacterKnowledgeMapping_WhenItHasEnoughXp()
    {
        await _useCase.Execute(_model);

        A.CallTo(() =>
                _mappingRepository.AddCharacterKnowledgeMapping(
                    A<CharacterKnowledgeMapping>.That.Matches(x =>
                        x.Notes == _model.Notes
                        && x.KnowledgeId == _model.KnowledgeId
                        && x.CharacterId == _model.CharacterId
                        && x.KnowledgeLevelId == _model.KnowledgeLevelId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_KnowledgeId_IfSuccessful()
    {
        A.CallTo(() =>
                _mappingRepository.AddCharacterKnowledgeMapping(A<CharacterKnowledgeMapping>._)
            )
            .Returns(5);
        var result = await _useCase.Execute(_model);
        Assert.Equal(5, result.Value);
    }
}

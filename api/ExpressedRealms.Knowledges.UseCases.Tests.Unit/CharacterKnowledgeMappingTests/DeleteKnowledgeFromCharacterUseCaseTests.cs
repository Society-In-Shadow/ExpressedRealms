using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Delete;
using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Edit;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit.CharacterKnowledgeMappingTests;

public class DeleteKnowledgeFromCharacterUseCaseTests
{
    private readonly DeleteKnowledgeFromCharacterUseCase _useCase;
    private readonly ICharacterKnowledgeRepository _mappingRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterFactionRepository _characterFactionRepository;
    private readonly DeleteKnowledgeFromCharacterModel _model;

    public DeleteKnowledgeFromCharacterUseCaseTests()
    {
        _model = new DeleteKnowledgeFromCharacterModel() { MappingId = 1, CharacterId = 2 };

        var dbModel = new CharacterKnowledgeMapping()
        {
            KnowledgeLevelId = 3,
            CharacterId = _model.CharacterId,
            KnowledgeId = 4,
            Notes = "123",
        };

        _mappingRepository = A.Fake<ICharacterKnowledgeRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _characterFactionRepository = A.Fake<ICharacterFactionRepository>();

        A.CallTo(() => _mappingRepository.GetCharacterKnowledgeMappingForEditing(_model.MappingId))
            .Returns(dbModel);

        A.CallTo(() => _mappingRepository.MappingAlreadyExists(_model.MappingId)).Returns(true);
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);
        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns([]);

        var validator = new DeleteKnowledgeFromCharacterModelValidator(
            _mappingRepository,
            _characterRepository
        );

        _useCase = new DeleteKnowledgeFromCharacterUseCase(
            _mappingRepository,
            _characterFactionRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_WillFail_WhenItsEmpty()
    {
        _model.MappingId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(UpdateKnowledgeForCharacterModel.MappingId),
            "Mapping Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _mappingRepository.MappingAlreadyExists(_model.MappingId)).Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(UpdateKnowledgeForCharacterModel.MappingId),
            "The Knowledge Mapping does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterId_IsEmpty()
    {
        _model.CharacterId = 0;

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(DeleteKnowledgeFromCharacterModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId))
            .Returns(false);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(DeleteKnowledgeFromCharacterModel.CharacterId),
            "This Character was not found."
        );
    }

    [Fact]
    public async Task UseCase_GetsTheKnowledgeMapping()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _mappingRepository.GetCharacterKnowledgeMappingForEditing(_model.MappingId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillGrab_TheLatestPlayerFactionLevels()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillFail_WhenFactionLevelRequires_TheKnowledge()
    {
        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns([new CharacterFactionDto() { KnowledgeId = 4 }]);

        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(DeleteKnowledgeFromCharacterModel.MappingId),
            "Your faction level prevents you from removing this knowledge"
        );
    }

    [Fact]
    public async Task UseCase_WillNotUpdateDeleteFields_WhenFactionLevelRequires_TheKnowledge()
    {
        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns([new CharacterFactionDto() { KnowledgeId = 4 }]);

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _mappingRepository.UpdateCharacterKnowledgeMapping(A<CharacterKnowledgeMapping>._)
            )
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillUpdateDeleteFields()
    {
        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _mappingRepository.UpdateCharacterKnowledgeMapping(
                    A<CharacterKnowledgeMapping>.That.Matches(x => x.IsDeleted)
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

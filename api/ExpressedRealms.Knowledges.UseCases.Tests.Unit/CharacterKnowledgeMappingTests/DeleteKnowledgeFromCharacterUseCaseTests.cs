using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
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
    private readonly DeleteKnowledgeFromCharacterModel _model;

    public DeleteKnowledgeFromCharacterUseCaseTests()
    {
        _model = new DeleteKnowledgeFromCharacterModel()
        {
            MappingId = 1
        };

        var dbModel = new CharacterKnowledgeMapping()
        {
            KnowledgeLevelId = 3,
            CharacterId = 2,
            KnowledgeId = 4,
            Notes = "123",
        };

        _mappingRepository = A.Fake<ICharacterKnowledgeRepository>();
        
        A.CallTo(() => _mappingRepository.GetCharacterKnowledgeMappingForEditing(_model.MappingId)).Returns(dbModel);

        A.CallTo(() =>
                _mappingRepository.MappingAlreadyExists(_model.MappingId)
            )
            .Returns(true);

        var validator = new DeleteKnowledgeFromCharacterModelValidator(
            _mappingRepository
        );

        _useCase = new DeleteKnowledgeFromCharacterUseCase(
            _mappingRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_WillFail_WhenItsEmpty()
    {
        _model.MappingId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(nameof(UpdateKnowledgeForCharacterModel.MappingId), "Mapping Id is required.");
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
    public async Task UseCase_GetsTheKnowledgeMapping()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _mappingRepository.GetCharacterKnowledgeMappingForEditing(_model.MappingId))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task UseCase_WillUpdateDeleteFields()
    {
        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);
        
        A.CallTo(() =>
                _mappingRepository.UpdateCharacterKnowledgeMapping(
                    A<CharacterKnowledgeMapping>.That.Matches(x =>
                        x.IsDeleted)
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

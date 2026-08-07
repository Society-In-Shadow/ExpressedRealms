using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.DeleteSpecialization;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit.KnowledgeSpecializationTests;

public class DeleteSpecializationUseCaseTests
{
    private readonly DeleteSpecializationUseCase _useCase;
    private readonly IKnowledgeSpecializationRepository _repository;
    private readonly ICharacterKnowledgeRepository _mappingRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterFactionRepository _characterFactionRepository;
    private readonly DeleteSpecializationModel _model;

    public DeleteSpecializationUseCaseTests()
    {
        _model = new DeleteSpecializationModel() { Id = 4, CharacterId = 2 };

        _repository = A.Fake<IKnowledgeSpecializationRepository>();
        _mappingRepository = A.Fake<ICharacterKnowledgeRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _characterFactionRepository = A.Fake<ICharacterFactionRepository>();

        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);
        A.CallTo(() => _repository.SpecializationExists(_model.Id)).Returns(true);
        A.CallTo(() => _repository.GetSpecialization(_model.Id))
            .Returns(
                new CharacterKnowledgeSpecialization()
                {
                    Id = _model.Id,
                    Name = "Forgery",
                    KnowledgeMappingId = 8,
                }
            );
        A.CallTo(() => _mappingRepository.GetCharacterKnowledgeMappingForEditing(8))
            .Returns(new CharacterKnowledgeMapping() { KnowledgeId = 12 });
        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns([]);

        var validator = new DeleteSpecializationModelValidator(_repository, _characterRepository);

        _useCase = new DeleteSpecializationUseCase(
            _repository,
            _characterFactionRepository,
            _mappingRepository,
            validator,
            CancellationToken.None
        );
    }

    // ... existing code ...

    [Fact]
    public async Task UseCase_WillGrab_TheKnowledge()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetSpecialization(_model.Id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillGrab_TheKnowledgeMapping()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _mappingRepository.GetCharacterKnowledgeMappingForEditing(8))
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
    public async Task UseCase_WillFail_WhenFactionLevelRequires_TheKnowledgeSpecialization()
    {
        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns(
                [
                    new CharacterFactionDto()
                    {
                        KnowledgeId = 12,
                        KnowledgeSpecialization = "Forgery",
                    },
                ]
            );

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(DeleteSpecializationModel.Id),
            "Your faction level prevents you from removing this knowledge specialization"
        );
    }

    [Fact]
    public async Task UseCase_WillNotSoftDelete_WhenFactionLevelRequires_TheKnowledgeSpecialization()
    {
        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns(
                [
                    new CharacterFactionDto()
                    {
                        KnowledgeId = 12,
                        KnowledgeSpecialization = "Forgery",
                    },
                ]
            );

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.UpdateSpecialization(A<CharacterKnowledgeSpecialization>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillAllowSoftDelete_WhenFactionLevelRequires_SameSpecializationName_ForDifferentKnowledge()
    {
        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns(
                [
                    new CharacterFactionDto()
                    {
                        KnowledgeId = 13,
                        KnowledgeSpecialization = "Forgery",
                    },
                ]
            );

        var results = await _useCase.ExecuteAsync(_model);

        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task UseCase_WillSoftDelete_TheKnowledge()
    {
        var knowledge = new Knowledge() { Id = _model.Id, IsDeleted = true };

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _repository.UpdateSpecialization(
                    A<CharacterKnowledgeSpecialization>.That.Matches(k =>
                        k.Id == knowledge.Id && k.IsDeleted == knowledge.IsDeleted
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

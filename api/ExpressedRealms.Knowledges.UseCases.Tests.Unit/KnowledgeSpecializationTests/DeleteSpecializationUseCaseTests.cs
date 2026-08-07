using ExpressedRealms.Characters.Repository;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;
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
    private readonly ICharacterRepository _characterRepository;
    private readonly ICharacterFactionRepository _characterFactionRepository;
    private readonly DeleteSpecializationModel _model;

    public DeleteSpecializationUseCaseTests()
    {
        _model = new DeleteSpecializationModel() { Id = 4, CharacterId = 2 };

        _repository = A.Fake<IKnowledgeSpecializationRepository>();
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
                }
            );
        A.CallTo(() => _characterFactionRepository.GetLatestPlayerFactionLevels(_model.CharacterId))
            .Returns([]);

        var validator = new DeleteSpecializationModelValidator(_repository, _characterRepository);

        _useCase = new DeleteSpecializationUseCase(
            _repository,
            _characterFactionRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteSpecializationModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_KnowledgeDoesNotExist()
    {
        A.CallTo(() => _repository.SpecializationExists(_model.Id)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(DeleteSpecializationModel.Id),
            "This Specialization was not found."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterId_IsEmpty()
    {
        _model.CharacterId = 0;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(DeleteSpecializationModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenCharacterDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(DeleteSpecializationModel.CharacterId),
            "This Character was not found."
        );
    }

    [Fact]
    public async Task UseCase_WillGrab_TheKnowledge()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetSpecialization(_model.Id)).MustHaveHappenedOnceExactly();
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
                        KnowledgeSpecialization = "Forgery",
                    },
                ]
            );

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _repository.UpdateSpecialization(A<CharacterKnowledgeSpecialization>._)
            )
            .MustNotHaveHappened();
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
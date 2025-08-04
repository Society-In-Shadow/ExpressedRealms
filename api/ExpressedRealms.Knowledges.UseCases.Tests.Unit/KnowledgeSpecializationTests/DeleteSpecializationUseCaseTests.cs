using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
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
    private readonly DeleteSpecializationModel _model;

    public DeleteSpecializationUseCaseTests()
    {
        _model = new DeleteSpecializationModel() { Id = 4 };

        _repository = A.Fake<IKnowledgeSpecializationRepository>();

        A.CallTo(() => _repository.SpecializationExists(_model.Id)).Returns(true);
        A.CallTo(() => _repository.GetSpecialization(_model.Id))
            .Returns(new CharacterKnowledgeSpecialization() { Id = _model.Id });

        var validator = new DeleteSpecializationModelValidator(_repository);

        _useCase = new DeleteSpecializationUseCase(_repository, validator, CancellationToken.None);
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
    public async Task UseCase_WillGrab_TheKnowledge()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetSpecialization(_model.Id))
            .MustHaveHappenedOnceExactly();
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

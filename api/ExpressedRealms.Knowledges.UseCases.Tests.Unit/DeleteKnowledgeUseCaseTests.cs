using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;
using ExpressedRealms.Knowledges.UseCases.Knowledges.EditKnowledge;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit;

public class DeleteKnowledgeUseCaseTests
{
    private readonly DeleteKnowledgeUseCase _useCase;
    private readonly IKnowledgeRepository _repository;
    private readonly DeleteKnowledgeModel _model;

    public DeleteKnowledgeUseCaseTests()
    {
        _model = new DeleteKnowledgeModel() { Id = 4 };

        _repository = A.Fake<IKnowledgeRepository>();

        A.CallTo(() => _repository.IsExistingKnowledge(_model.Id)).Returns(true);
        A.CallTo(() => _repository.GetKnowledgeForEditingAsync(_model.Id))
            .Returns(new Knowledge() { Id = _model.Id });

        var validator = new DeleteKnowledgeModelValidator(_repository);

        _useCase = new DeleteKnowledgeUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditKnowledgeModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_KnowledgeDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingKnowledge(_model.Id)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditKnowledgeModel.Id),
            "This knowledge was not found."
        );
    }

    [Fact]
    public async Task UseCase_WillGrab_TheKnowledge()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetKnowledgeForEditingAsync(_model.Id))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillSoftDelete_TheKnowledge()
    {
        var knowledge = new Knowledge() { Id = _model.Id, IsDeleted = true };

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _repository.EditKnowledgeAsync(
                    A<Knowledge>.That.Matches(k =>
                        k.Id == knowledge.Id && k.IsDeleted == knowledge.IsDeleted
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

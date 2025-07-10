using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.UseCases.Knowledges.GetEditKnowledge;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit;

public class GetEditKnowledgeUseCaseTests
{
    private readonly GetEditKnowledgeUseCase _useCase;
    private readonly IKnowledgeRepository _repository;
    private readonly GetEditKnowledgeModel _model;

    private Knowledge KnowledgeDbModel { get; set; }

    public GetEditKnowledgeUseCaseTests()
    {
        _model = new GetEditKnowledgeModel() { Id = 4 };
        KnowledgeDbModel = new Knowledge()
        {
            Id = _model.Id,
            Name = "Test Knowledge",
            Description = "Test Description",
            KnowledgeTypeId = 1,
        };

        _repository = A.Fake<IKnowledgeRepository>();

        A.CallTo(() => _repository.IsExistingKnowledge(_model.Id)).Returns(true);
        A.CallTo(() => _repository.GetKnowledgeAsync(_model.Id)).Returns(KnowledgeDbModel);

        var validator = new GetEditKnowledgeModelValidator(_repository);

        _useCase = new GetEditKnowledgeUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenId_IsEmpty()
    {
        _model.Id = 0;
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(GetEditKnowledgeModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_KnowledgeDoesNotExist()
    {
        A.CallTo(() => _repository.IsExistingKnowledge(_model.Id)).Returns(false);
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetEditKnowledgeModel.Id),
            "This knowledge was not found."
        );
    }

    [Fact]
    public async Task UseCase_Grabs_TheKnowledge()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _repository.GetKnowledgeAsync(_model.Id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_Returns_AllPropertiesForTheKnowledge()
    {
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equal(KnowledgeDbModel.Name, results.Value.Name);
        Assert.Equal(KnowledgeDbModel.Description, results.Value.Description);
        Assert.Equal(KnowledgeDbModel.KnowledgeTypeId, results.Value.KnowledgeTypeId);
        Assert.Equal(KnowledgeDbModel.Id, results.Value.Id);
    }
}

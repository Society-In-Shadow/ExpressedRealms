using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.UseCases.Knowledges.CreateKnowledge;
using ExpressedRealms.Knowledges.UseCases.Knowledges.EditKnowledge;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit;

public class EditKnowledgeUseCaseTests
{
    private readonly EditKnowledgeUseCase _useCase;
    private readonly IKnowledgeRepository _repository;
    private readonly EditKnowledgeModel _model;

    public EditKnowledgeUseCaseTests()
    {
        _model = new EditKnowledgeModel()
        {
            Id = 4,
            Name = "Test Knowledge",
            Description = "Test Description",
            KnowledgeTypeId = 1,
        };

        _repository = A.Fake<IKnowledgeRepository>();

        A.CallTo(() => _repository.HasDuplicateName(_model.Name)).Returns(false);
        A.CallTo(() => _repository.KnowledgeTypeExists(_model.KnowledgeTypeId)).Returns(true);

        var validator = new EditKnowledgeModelValidator(_repository);

        _useCase = new EditKnowledgeUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsEmpty()
    {
        _model.Name = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditKnowledgeModel.Name), "Name is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsOver150Characters()
    {
        _model.Name = new string('x', 151);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditKnowledgeModel.Name),
            "Name must be between 1 and 150 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_AlreadyExists()
    {
        A.CallTo(() => _repository.HasDuplicateName(_model.Name)).Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditKnowledgeModel.Name),
            "Knowledge with this name already exists."
        );
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItsEmpty()
    {
        _model.Description = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditKnowledgeModel.Description),
            "Description is required."
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgeTypeId_WillFail_WhenItsEmpty()
    {
        _model.KnowledgeTypeId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditKnowledgeModel.KnowledgeTypeId),
            "Knowledge Type is required."
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgeTypeId_WillFail_WhenTheKnowledge_DoesNotExist()
    {
        A.CallTo(() => _repository.KnowledgeTypeExists(_model.KnowledgeTypeId)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditKnowledgeModel.KnowledgeTypeId),
            "The Knowledge Type does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillEditTheKnowledge()
    {
        var knowledge = new Knowledge()
        {
            Id = _model.Id,
            Name = _model.Name,
            Description = _model.Description,
            KnowledgeTypeId = _model.KnowledgeTypeId,
        };

        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditKnowledgeAsync(
                    A<Knowledge>.That.Matches(k =>
                        k.Id == knowledge.Id &&
                        k.Name == knowledge.Name && 
                        k.Description == knowledge.Description &&
                        k.KnowledgeTypeId == knowledge.KnowledgeTypeId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

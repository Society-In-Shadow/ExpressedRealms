using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Knowledges.UseCases.Knowledges.CreateKnowledge;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit;

public class CreateKnowledgeUseCaseTests
{
    private readonly CreateKnowledgeUseCase _useCase;
    private readonly IKnowledgeRepository _repository;
    private readonly CreateKnowledgeModel _model;

    public CreateKnowledgeUseCaseTests()
    {
        _model = new CreateKnowledgeModel() { Name = "Test Knowledge", Description = "Test Description", KnowledgeTypeId = 1 };

        _repository = A.Fake<IKnowledgeRepository>();

        A.CallTo(() => _repository.HasDuplicateName(_model.Name)).Returns(false);
        A.CallTo(() => _repository.KnowledgeTypeExists(_model.KnowledgeTypeId)).Returns(true);
        
        var validator = new CreateKnowledgeModelValidator(_repository);

        _useCase = new CreateKnowledgeUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsEmpty()
    {
        _model.Name = string.Empty;
        
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateKnowledgeModel.Name),
            "Name is required."
        );
    }
    
    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsOver150Characters()
    {
        _model.Name = new string('x', 151);
        
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateKnowledgeModel.Name),
            "Name must be between 1 and 150 characters."
        );
    }
    
    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_AlreadyExists()
    {
        A.CallTo(() => _repository.HasDuplicateName(_model.Name)).Returns(true);
        
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateKnowledgeModel.Name),
            "Knowledge with this name already exists."
        );
    }
    
    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItsEmpty()
    {
        _model.Description = string.Empty;
        
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateKnowledgeModel.Description),
            "Description is required."
        );
    }
    
    [Fact]
    public async Task ValidationFor_KnowledgeTypeId_WillFail_WhenItsEmpty()
    {
        _model.KnowledgeTypeId = 0;
        
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateKnowledgeModel.KnowledgeTypeId),
            "Knowledge Type is required."
        );
    }
    
    [Fact]
    public async Task ValidationFor_KnowledgeTypeId_WillFail_WhenTheKnowledge_DoesNotExist()
    {
        A.CallTo(() => _repository.KnowledgeTypeExists(_model.KnowledgeTypeId)).Returns(false);
        
        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateKnowledgeModel.KnowledgeTypeId),
            "The Knowledge Type does not exist."
        );
    }
}
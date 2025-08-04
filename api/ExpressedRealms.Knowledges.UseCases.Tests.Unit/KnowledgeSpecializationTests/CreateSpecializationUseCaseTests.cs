using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;
using ExpressedRealms.Knowledges.Repository.KnowledgeSpecializations;
using ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.CreateSpecialization;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit.KnowledgeSpecializationTests;

public class CreateSpecializationUseCaseTests
{
    private readonly CreateSpecializationUseCase _useCase;
    private readonly IKnowledgeSpecializationRepository _specializationRepository;
    private readonly ICharacterKnowledgeRepository _mappingRepository;
    private readonly CreateSpecializationModel _model;
    private readonly CharacterKnowledgeMapping _dbModel;

    public CreateSpecializationUseCaseTests()
    {
        _model = new CreateSpecializationModel()
        {
            Name = "Test Knowledge",
            Description = "Test Description",
            Notes = "567",
            KnowledgeMappingId = 1,
        };

        _dbModel = new CharacterKnowledgeMapping() { CharacterId = 1, KnowledgeId = 2 };

        _specializationRepository = A.Fake<IKnowledgeSpecializationRepository>();
        _mappingRepository = A.Fake<ICharacterKnowledgeRepository>();

        A.CallTo(() =>
                _mappingRepository.HasExistingSpecializationForMapping(
                    _model.KnowledgeMappingId,
                    _model.Name
                )
            )
            .Returns(false);
        A.CallTo(() => _mappingRepository.MappingAlreadyExists(_model.KnowledgeMappingId))
            .Returns(true);
        A.CallTo(() =>
                _mappingRepository.GetCharacterKnowledgeMappingForEditing(_model.KnowledgeMappingId)
            )
            .Returns(_dbModel);
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(_dbModel.CharacterId)
            )
            .Returns(0);
        A.CallTo(() =>
                _mappingRepository.GetSpecializationCountForMapping(_model.KnowledgeMappingId)
            )
            .Returns(new SpecializationCountProjection() { CurrentCount = 0, MaxCount = 2 });

        var validator = new CreateSpecializationModelValidator(
            _specializationRepository,
            _mappingRepository
        );

        _useCase = new CreateSpecializationUseCase(
            _specializationRepository,
            _mappingRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsEmpty()
    {
        _model.Name = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateSpecializationModel.Name),
            "Name is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_IsOver250Characters()
    {
        _model.Name = new string('x', 251);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateSpecializationModel.Name),
            "Name must be between 1 and 250 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenName_AlreadyExists()
    {
        A.CallTo(() =>
                _mappingRepository.HasExistingSpecializationForMapping(
                    _model.KnowledgeMappingId,
                    _model.Name
                )
            )
            .Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateSpecializationModel.Name),
            "A specialization with this name already exists for the given knowledge."
        );
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItsEmpty()
    {
        _model.Description = string.Empty;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateSpecializationModel.Description),
            "Description is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Description_WillFail_WhenItIsOver_5000Characters()
    {
        _model.Description = new string('c', 5001);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateSpecializationModel.Description),
            "Description must be between 1 and 5000 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _model.Notes = new string('x', 10001);
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(CreateSpecializationModel.Notes),
            "Notes must be less than 10,000 characters."
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ValidationFor_Notes_AreOptional(string? notes)
    {
        _model.Notes = notes;
        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_KnowledgeMappingId_WillFail_WhenItsEmpty()
    {
        _model.KnowledgeMappingId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateSpecializationModel.KnowledgeMappingId),
            "Knowledge Mapping Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_KnowledgeMappingId_WillFail_WhenTheKnowledge_DoesNotExist()
    {
        A.CallTo(() => _mappingRepository.MappingAlreadyExists(_model.KnowledgeMappingId))
            .Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateSpecializationModel.KnowledgeMappingId),
            "The Knowledge Mapping does not exist."
        );
    }

    [Fact]
    public async Task UseCase_CorrectlyGrabs_TheKnowledgeMapping()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _mappingRepository.GetCharacterKnowledgeMappingForEditing(_model.KnowledgeMappingId)
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_CorrectlyGrabs_ExperienceSpentOnKnowledges_ForTheCharacter()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(_dbModel.CharacterId)
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_CorrectlyGrabs_TheSpecializationCountForTheMapping()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _mappingRepository.GetSpecializationCountForMapping(_model.KnowledgeMappingId)
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillCreateThSpecialization()
    {
        var knowledge = new CharacterKnowledgeSpecialization()
        {
            Name = _model.Name,
            Description = _model.Description,
            KnowledgeMappingId = _model.KnowledgeMappingId,
            Notes = _model.Notes,
        };

        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _specializationRepository.CreateSpecialization(
                    A<CharacterKnowledgeSpecialization>.That.Matches(k =>
                        k.Name == knowledge.Name
                        && k.Description == knowledge.Description
                        && k.Notes == knowledge.Notes
                        && k.KnowledgeMappingId == knowledge.KnowledgeMappingId
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_CalculatesAvailableXP_Correctly()
    {
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(_dbModel.CharacterId)
            )
            .Returns(6);

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(1, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
    }

    [Fact]
    public async Task UseCase_CalculatesCost_Correctly()
    {
        A.CallTo(() =>
                _mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(_dbModel.CharacterId)
            )
            .Returns(6);

        var result = await _useCase.ExecuteAsync(_model);

        Assert.Equal(2, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Fact]
    public async Task UseCase_Fails_WhenTheyAlreadyHave_MaxAmountOfSpecializations()
    {
        A.CallTo(() =>
                _mappingRepository.GetSpecializationCountForMapping(_model.KnowledgeMappingId)
            )
            .Returns(new SpecializationCountProjection() { CurrentCount = 2, MaxCount = 2 });

        var result = await _useCase.ExecuteAsync(_model);

        Assert.False(result.IsSuccess);
        Assert.Equal(
            "You have reached the maximum number of specializations allowed for this knowledge.",
            result.Errors[0].Message
        );
    }

    [Fact]
    public async Task UseCase_WillReturn_KnowledgeId_IfSuccessful()
    {
        A.CallTo(() =>
                _specializationRepository.CreateSpecialization(
                    A<CharacterKnowledgeSpecialization>._
                )
            )
            .Returns(5);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.Equal(5, result.Value);
    }
}

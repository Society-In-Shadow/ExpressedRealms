using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;
using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetReadOnly;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Knowledges.UseCases.Tests.Unit.CharacterKnowledgeMappingTests;

public class GetReadOnlyKnowledgesForCharacterUseCaseTests
{
    private readonly GetKnowledgesForCharacterUseCase _useCase;
    private readonly ICharacterKnowledgeRepository _mappingRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly GetKnowledgesForCharacterModel _model;
    private readonly List<CharacterKnowledgeProjection> _dbModel;

    public GetReadOnlyKnowledgesForCharacterUseCaseTests()
    {
        _model = new GetKnowledgesForCharacterModel() { CharacterId = 1 };

        _dbModel = new List<CharacterKnowledgeProjection>()
        {
            new CharacterKnowledgeProjection()
            {
                MappingId = 1,
                Knowledge = new KnowledgeProjection()
                {
                    Id = 29,
                    Name = "Knowledge",
                    Description = "Knowledge Description",
                    Type = "Type",
                },
                LevelName = "LevelName",
                Level = 2,
                StoneModifier = 3,
                Notes = "Notes",
                LevelId = 3,
                SpecializationCount = 20,
                Specializations = new List<SpecializationProjection>()
                {
                    new SpecializationProjection()
                    {
                        Name = "Specialization",
                        Description = "Specialization Description",
                        Notes = "Notes",
                        Id = 1,
                    },
                    new SpecializationProjection()
                    {
                        Name = "Specialization 2",
                        Description = "Specialization Description 2",
                        Notes = null,
                        Id = 2,
                    },
                },
            },
            new CharacterKnowledgeProjection()
            {
                MappingId = 4,
                Knowledge = new KnowledgeProjection()
                {
                    Id = 30,
                    Name = "Knowledge 2",
                    Description = "Knowledge Description 2",
                    Type = "Type 2",
                },
                LevelName = "LevelName 2",
                LevelId = 8,
                SpecializationCount = 3,
                Level = 5,
                StoneModifier = 6,
                Specializations = new List<SpecializationProjection>(),
            },
        };

        _mappingRepository = A.Fake<ICharacterKnowledgeRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();

        A.CallTo(() => _mappingRepository.GetKnowledgesForCharacter(_model.CharacterId))
            .Returns(_dbModel);

        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);

        var validator = new GetKnowledgesForCharacterModelValidator(_characterRepository);

        _useCase = new GetKnowledgesForCharacterUseCase(
            _mappingRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetKnowledgesForCharacterModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_MappingId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetKnowledgesForCharacterModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task UseCase_GetsTheKnowledgeMapping()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _mappingRepository.GetKnowledgesForCharacter(_model.CharacterId))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_ReturnsKnowledgesForCharacter()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);

        var knowledges = _dbModel
            .Select(x => new CharacterKnowledgeReturnModel()
            {
                MappingId = x.MappingId,
                Knowledge = new KnowledgeReturnModel()
                {
                    Id = x.Knowledge.Id,
                    Name = x.Knowledge.Name,
                    Description = x.Knowledge.Description,
                    Type = x.Knowledge.Type,
                },
                StoneModifier = x.StoneModifier,
                LevelName = x.LevelName,
                Level = x.Level,
                Notes = x.Notes,
                SpecializationCount = x.SpecializationCount,
                LevelId = x.LevelId,
                Specializations = x
                    .Specializations.Select(y => new SpecializationReturnModel()
                    {
                        Name = y.Name,
                        Description = y.Description,
                        Id = y.Id,
                        Notes = y.Notes,
                    })
                    .ToList(),
            })
            .ToList();

        Assert.Equivalent(knowledges, results.Value);
    }
}

using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.UseCases.AssignedXp.Delete;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.AssignedXp;

public class DeleteAssignedXpMappingUseCaseTests
{
    private readonly DeleteAssignedXpMappingUseCase _useCase;
    private readonly IAssignedXpMappingRepository _repository;
    private readonly ICharacterRepository _characterRepository;

    private readonly DeleteAssignedXpMappingModel _model;
    private readonly AssignedXpMapping _dbModel;

    public DeleteAssignedXpMappingUseCaseTests()
    {
        _model = new DeleteAssignedXpMappingModel() { Id = 1, CharacterId = 4 };

        _dbModel = new AssignedXpMapping()
        {
            Id = 1,
            Amount = 10,
            Reason = "They are awesome!",
            PlayerId = Guid.NewGuid(),
            EventId = 4,
            AssignedXpTypeId = 3,
            AssignedByUserId = "foo",
        };

        _repository = A.Fake<IAssignedXpMappingRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();

        A.CallTo(() => _repository.FindAsync<AssignedXpMapping>(_model.Id)).Returns(_dbModel);
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character());

        var validator = new DeleteAssignedXpMappingModelValidator(_characterRepository, _repository);

        _useCase = new DeleteAssignedXpMappingUseCase(
            _repository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItsEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteAssignedXpMappingModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenIt_DoesNotExist()
    {
        A.CallTo(() => _repository.FindAsync<AssignedXpMapping>(_model.Id))
            .Returns(Task.FromResult<AssignedXpMapping?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(DeleteAssignedXpMappingModel.Id),
            "The Id does not exist."
        );
    }
    
    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(DeleteAssignedXpMappingModel.CharacterId), "Character Id is required.");
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenIt_DoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(Task.FromResult<Character?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(DeleteAssignedXpMappingModel.CharacterId),
            "The Character Id does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillDeleteTheAssignedXpMapping()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditAsync(A<AssignedXpMapping>.That.Matches(k => k.IsDeleted == true))
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_PassesThrough_TheDbAssignedXpMapping()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _repository.EditAsync(A<AssignedXpMapping>.That.IsSameAs(_dbModel)))
            .MustHaveHappenedOnceExactly();
    }
}

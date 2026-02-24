using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.UseCases.AssignedXp.Edit;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;
using ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpTypeModels;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.AssignedXp;

public class EditAssignedXpMappingUseCaseTests
{
    private readonly EditAssignedXpMappingUseCase _useCase;
    private readonly IAssignedXpMappingRepository _repository;
    private readonly IEventRepository _eventRepository;
    private readonly ICharacterRepository _characterRepository;

    private readonly EditAssignedXpMappingModel _model;
    private readonly AssignedXpMapping _dbModel;

    public EditAssignedXpMappingUseCaseTests()
    {
        _model = new EditAssignedXpMappingModel()
        {
            AssignedXpTypeId = 2,
            EventId = 3,
            Reason = "They are awesome asdf!",
            Amount = 110,
            Id = 1,
            CharacterId = 4,
        };

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
        _eventRepository = A.Fake<IEventRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();

        A.CallTo(() => _eventRepository.FindEventAsync(_model.EventId))
            .Returns(
                new Event()
                {
                    Id = 1,
                    Name = "Test Event",
                    EndDate = DateOnly.MaxValue,
                    Location = "Foo",
                    StartDate = DateOnly.MaxValue,
                    WebsiteName = "foo",
                    WebsiteUrl = "foo",
                    TimeZoneId = "Foo",
                }
            );

        A.CallTo(() => _repository.FindAsync<AssignedXpMapping>(_model.Id)).Returns(_dbModel);

        A.CallTo(() => _repository.FindAsync<AssignedXpType>(_model.AssignedXpTypeId))
            .Returns(new AssignedXpType() { Name = "Foo" });

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character());

        var validator = new EditAssignedXpMappingModelValidator(
            _eventRepository,
            _characterRepository,
            _repository
        );

        _useCase = new EditAssignedXpMappingUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_Reason_WillFail_WhenItIs_IsOver1500Characters()
    {
        _model.Reason = new string('x', 1501);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.Reason),
            "Reason must be between 1 and 1500 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_AssignedXpMappingTypeId_WillFail_WhenItsEmpty()
    {
        _model.AssignedXpTypeId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.AssignedXpTypeId),
            "Assigned Xp Type Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_AssignedXpMappingTypeId_WillFail_WhenTheAssignedXpMapping_DoesNotExist()
    {
        A.CallTo(() => _repository.FindAsync<AssignedXpType>(_model.AssignedXpTypeId))
            .Returns(Task.FromResult<AssignedXpType?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.AssignedXpTypeId),
            "The Assigned Xp Type Id does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenItsEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.EventId),
            "Event Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenIt_DoesNotExist()
    {
        A.CallTo(() => _eventRepository.FindEventAsync(_model.EventId))
            .Returns(Task.FromResult<Event?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.EventId),
            "The Event Id does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItsEmpty()
    {
        _model.Id = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(nameof(EditAssignedXpMappingModel.Id), "Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenIt_DoesNotExist()
    {
        A.CallTo(() => _repository.FindAsync<AssignedXpMapping>(_model.Id))
            .Returns(Task.FromResult<AssignedXpMapping?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.Id),
            "The Id does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenIt_DoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(Task.FromResult<Character?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.CharacterId),
            "The Character Id does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_Amount_WillFail_WhenItsEmpty()
    {
        _model.Amount = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.Amount),
            "Amount is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Amount_WillFail_WhenItsNegative()
    {
        _model.Amount = -1;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(EditAssignedXpMappingModel.Amount),
            "Amount must be greater than 0."
        );
    }

    [Fact]
    public async Task UseCase_WillEditTheAssignedXpMapping()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.EditAsync(
                    A<AssignedXpMapping>.That.Matches(k =>
                        k.AssignedXpTypeId == _model.AssignedXpTypeId
                        && k.EventId == _model.EventId
                        && k.Reason == _model.Reason
                        && k.Amount == _model.Amount
                    )
                )
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

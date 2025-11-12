using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Players;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.UseCases.AssignedXp.Create;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpMappingModels;
using ExpressedRealms.DB.Characters.AssignedXp.AssignedXpTypeModels;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Repositories.Shared.ExternalDependencies;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.AssignedXp;

public class CreateAssignedXpMappingUseCaseTests
{
    private readonly CreateAssignedXpMappingUseCase _useCase;
    private readonly IAssignedXpMappingRepository _repository;
    private readonly TimeProvider _timeProvider;
    private readonly IEventRepository _eventRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IUserContext _userContext;
    private readonly IPlayerRepository _playerRepository;

    private readonly CreateAssignedXpMappingModel _model;
    private readonly Character _characterModel;

    public CreateAssignedXpMappingUseCaseTests()
    {
        _model = new CreateAssignedXpMappingModel()
        {
            AssignedXpTypeId = 1,
            CharacterId = 2,
            EventId = 3,
            Reason = "They are awesome!",
            Amount = 10
        };

        _characterModel = new Character() { PlayerId = Guid.NewGuid() };

        _repository = A.Fake<IAssignedXpMappingRepository>();
        _eventRepository = A.Fake<IEventRepository>();
        _timeProvider = A.Fake<TimeProvider>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _userContext = A.Fake<IUserContext>();
        _playerRepository = A.Fake<IPlayerRepository>();

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

        A.CallTo(() => _repository.FindAsync<AssignedXpType>(_model.AssignedXpTypeId))
            .Returns(new AssignedXpType() { Name = "Foo" });

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(_characterModel);

        var validator = new CreateAssignedXpMappingModelValidator(
            _characterRepository,
            _eventRepository,
            _playerRepository,
            _repository
        );

        _useCase = new CreateAssignedXpMappingUseCase(
            _repository,
            validator,
            _characterRepository,
            _userContext,
            _timeProvider,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Reason_WillFail_WhenItIs_IsOver1500Characters()
    {
        _model.Reason = new string('x', 1501);

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateAssignedXpMappingModel.Reason),
            "Reason must be between 1 and 1500 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_AssignedXpMappingTypeId_WillFail_WhenItsEmpty()
    {
        _model.AssignedXpTypeId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateAssignedXpMappingModel.AssignedXpTypeId),
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
            nameof(CreateAssignedXpMappingModel.AssignedXpTypeId),
            "The Assigned Xp Type Id does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenItsEmpty()
    {
        _model.EventId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateAssignedXpMappingModel.EventId),
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
            nameof(CreateAssignedXpMappingModel.EventId),
            "The Event Id does not exist."
        );
    }
    
    [Fact]
    public async Task ValidationFor_Amount_WillFail_WhenItsEmpty()
    {
        _model.Amount = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateAssignedXpMappingModel.Amount),
            "Amount is required."
        );
    }

    [Fact]
    public async Task ValidationFor_Amount_WillFail_WhenItsNegative()
    {
        _model.Amount = -1;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateAssignedXpMappingModel.Amount),
            "Amount must be greater than 0."
        );
    }
    
    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(CreateAssignedXpMappingModel.CharacterId),
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
            nameof(CreateAssignedXpMappingModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillCreateTheAssignedXpMapping()
    {
        var currentDate = DateTime.UtcNow;
        var currentUserId = "Foo";
        A.CallTo(() => _timeProvider.GetUtcNow()).Returns(currentDate);
        A.CallTo(() => _userContext.CurrentUserId()).Returns(currentUserId);

        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.AddAsync(
                    A<AssignedXpMapping>.That.Matches(k =>
                        k.PlayerId == _characterModel.PlayerId
                        && k.AssignedXpTypeId == _model.AssignedXpTypeId
                        && k.Timestamp == currentDate
                        && k.EventId == _model.EventId
                        && k.Reason == _model.Reason
                        && k.CharacterId == _model.CharacterId
                        && k.AssignedByUserId == currentUserId
                        && k.Amount == _model.Amount
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_AssignedXpMappingId_IfSuccessful()
    {
        A.CallTo(() => _repository.AddAsync(A<AssignedXpMapping>._)).Returns(5);

        var result = await _useCase.ExecuteAsync(_model);
        Assert.Equal(5, result.Value);
    }
}

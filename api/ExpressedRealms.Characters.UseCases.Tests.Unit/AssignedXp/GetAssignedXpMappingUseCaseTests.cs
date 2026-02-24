using Bogus;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.Repository.Xp.Dtos.AssignedXpInfoDtos;
using ExpressedRealms.Characters.UseCases.AssignedXp.Get;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.Repositories.Events.Dtos;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;
using BasicInfo = ExpressedRealms.Characters.Repository.Xp.Dtos.AssignedXpInfoDtos.BasicInfo;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.AssignedXp;

public class GetAssignedXpMappingUseCaseTests
{
    private readonly GetAssignedXpMappingUseCase _useCase;
    private readonly IAssignedXpMappingRepository _repository;
    private readonly ICharacterRepository _characterRepository;

    private readonly GetAssignedXpMappingModel _model;
    private readonly IEventRepository _eventRepository;
    private readonly List<XpMappingInfoDto> _xpMappingInfoDtos;
    private readonly List<EventXpDto> _eventXpDtos;

    public GetAssignedXpMappingUseCaseTests()
    {
        Randomizer.Seed = new Random(12345);
        _model = new GetAssignedXpMappingModel() { CharacterId = 4, EventId = 0 };
        var basicFaker = new Faker<BasicInfo>()
            .RuleFor(b => b.Id, f => f.IndexFaker + 1)
            .RuleFor(b => b.Name, f => f.Random.Word());

        var startDate = new DateOnly(2025, 1, 1);
        var dateTimeOffset = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var faker = new Faker<XpMappingInfoDto>()
            .RuleForType(typeof(int), f => f.IndexFaker + 1)
            .RuleForType(typeof(string), f => f.Random.AlphaNumeric(8))
            .RuleForType(typeof(Guid), f => f.Random.Guid())
            .RuleFor(
                x => x.DateAssigned,
                f => dateTimeOffset.AddDays(f.IndexFaker).AddTicks(f.Random.Long(0, 1000000))
            )
            .RuleFor(x => x.Player, basicFaker)
            .RuleFor(x => x.Assigner, basicFaker)
            .RuleFor(x => x.Event, basicFaker)
            .RuleFor(x => x.XpType, basicFaker)
            .RuleFor(x => x.Character, basicFaker);

        var eventFaker = new Faker<EventXpDto>()
            .RuleForType(typeof(int), f => f.IndexFaker + 1)
            .RuleForType(typeof(string), f => f.Random.AlphaNumeric(8))
            .RuleForType(typeof(Guid), f => f.Random.Guid())
            .RuleFor(x => x.StartDate, f => startDate.AddDays(f.IndexFaker));

        _xpMappingInfoDtos = faker.Generate(3);
        _eventXpDtos = eventFaker.Generate(3);

        _repository = A.Fake<IAssignedXpMappingRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();
        _eventRepository = A.Fake<IEventRepository>();

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character());
        A.CallTo(() => _eventRepository.FindEventAsync(_model.EventId))
            .Returns(new Faker<Event>().Generate());

        A.CallTo(() => _repository.GetAllEventMappingsAsync(_model.EventId))
            .Returns(_xpMappingInfoDtos);

        var playerId = Guid.NewGuid();
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character() { PlayerId = playerId });

        A.CallTo(() => _repository.GetAllPlayerMappingsAsync(playerId)).Returns(_xpMappingInfoDtos);

        A.CallTo(() => _eventRepository.GetEventsWithAvailableXp()).Returns(_eventXpDtos);

        var validator = new GetAssignedXpMappingModelValidator(
            _characterRepository,
            _eventRepository,
            _repository
        );

        _useCase = new GetAssignedXpMappingUseCase(
            _repository,
            _characterRepository,
            _eventRepository,
            validator,
            CancellationToken.None
        );
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(4, 4)]
    public async Task ValidationFor_CharacterIdAndEventId_WillFail_WhenBothEventAndCharacterIdAreEmpty(
        int characterId,
        int eventId
    )
    {
        _model.CharacterId = characterId;
        _model.EventId = eventId;

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetAssignedXpMappingModel.CharacterId),
            "Either Character Id or Event Id must be specified."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenIt_DoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(Task.FromResult<Character?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetAssignedXpMappingModel.CharacterId),
            "The Character Id does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_EventId_WillFail_WhenIt_DoesNotExist()
    {
        _model.EventId = 4;
        A.CallTo(() => _eventRepository.FindEventAsync(_model.EventId))
            .Returns(Task.FromResult<Event?>(null));

        var results = await _useCase.ExecuteAsync(_model);
        results.MustHaveValidationError(
            nameof(GetAssignedXpMappingModel.EventId),
            "The Event Id does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WithCharacterId_PassesThrough_AllAssignedXpAndAllEventXp_ForSaidCharacter()
    {
        var results = await _useCase.ExecuteAsync(_model);

        var events = _eventXpDtos
            .Select(x => new XpMappingInfoReturnModel
            {
                Id = -1,
                Amount = x.ConExperience,
                Character = new UseCases.AssignedXp.Get.BasicInfo { Name = "System" },
                Assigner = new UseCases.AssignedXp.Get.BasicInfo { Name = "System" },
                Player = new UseCases.AssignedXp.Get.BasicInfo { Name = "System" },
                DateAssigned = x.StartDate.ToDateTime(TimeOnly.MinValue),
                Event = new UseCases.AssignedXp.Get.BasicInfo
                {
                    Id = x.Id,
                    Name = $"{x.Name} ({x.StartDate.Year})",
                },
                Notes = "",
                XpType = new UseCases.AssignedXp.Get.BasicInfo { Id = 1, Name = "Event XP" },
            })
            .ToList();

        events.AddRange(
            _xpMappingInfoDtos
                .Select(x => new XpMappingInfoReturnModel
                {
                    Id = x.Id,
                    DateAssigned = x.DateAssigned,
                    Notes = x.Notes,
                    Amount = x.Amount,
                    Player = new UseCases.AssignedXp.Get.BasicInfo
                    {
                        Id = x.Player.Id,
                        Name = x.Player.Name,
                    },
                    Character = new UseCases.AssignedXp.Get.BasicInfo
                    {
                        Id = x.Character.Id,
                        Name = x.Character.Name,
                    },
                    Event = new UseCases.AssignedXp.Get.BasicInfo
                    {
                        Id = x.Event.Id,
                        Name = x.Event.Name,
                    },
                    XpType = new UseCases.AssignedXp.Get.BasicInfo
                    {
                        Id = x.XpType.Id,
                        Name = x.XpType.Name,
                    },
                    Assigner = new UseCases.AssignedXp.Get.BasicInfo
                    {
                        Id = x.Assigner.Id,
                        Name = x.Assigner.Name,
                    },
                })
                .ToList()
        );

        Assert.Equivalent(events.OrderByDescending(x => x.DateAssigned), results.Value);
    }

    [Fact]
    public async Task UseCase_WithEventId_PassesThrough_AllAssignedXp_ForSaidEvent()
    {
        _model.CharacterId = 0;
        _model.EventId = 4;

        A.CallTo(() => _repository.GetAllEventMappingsAsync(_model.EventId))
            .Returns(_xpMappingInfoDtos);

        var results = await _useCase.ExecuteAsync(_model);

        var expectedResults = _xpMappingInfoDtos
            .Select(x => new XpMappingInfoReturnModel()
            {
                Id = x.Id,
                DateAssigned = x.DateAssigned,
                Notes = x.Notes,
                Amount = x.Amount,
                Player = new UseCases.AssignedXp.Get.BasicInfo()
                {
                    Id = x.Player.Id,
                    Name = x.Player.Name,
                },
                Character = new UseCases.AssignedXp.Get.BasicInfo()
                {
                    Id = x.Character.Id,
                    Name = x.Character.Name,
                },
                Event = new UseCases.AssignedXp.Get.BasicInfo()
                {
                    Id = x.Event.Id,
                    Name = x.Event.Name,
                },
                XpType = new UseCases.AssignedXp.Get.BasicInfo()
                {
                    Id = x.XpType.Id,
                    Name = x.XpType.Name,
                },
                Assigner = new UseCases.AssignedXp.Get.BasicInfo()
                {
                    Id = x.Assigner.Id,
                    Name = x.Assigner.Name,
                },
            })
            .OrderByDescending(x => x.DateAssigned)
            .ToList();

        Assert.Equivalent(expectedResults, results.Value);
    }
}

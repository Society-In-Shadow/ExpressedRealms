using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Characters.Repository.Contacts.Dtos;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.UseCases.Contacts.GetContacts;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Contacts;

public class GetContactsUseCaseTests
{
    private readonly GetContactsUseCase _useCase;
    private readonly ICharacterRepository _characterRepository;
    private readonly IContactRepository _contactRepository;
    private readonly GetContactsModel _model;
    private readonly List<ContactListDto> _dbModel;

    public GetContactsUseCaseTests()
    {
        _model = new GetContactsModel() { CharacterId = 2 };

        _dbModel = new List<ContactListDto>()
        {
            new ()
            {
                Name = "Luffy",
                KnowledgeLevel = "Associate (3)",
                Knowledge = "Computer Science",
                Id = 3,
                IsApproved = false,
                UsesPerWeek = 1,
            },
            new ()
            {
                Name = "Zoro",
                KnowledgeLevel = "Bachalors (5)",
                Knowledge = "Navigation",
                Id = 2,
                IsApproved = true,
                UsesPerWeek = 3,
            }
        };

        _characterRepository = A.Fake<ICharacterRepository>();
        _contactRepository = A.Fake<IContactRepository>();

        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);
        A.CallTo(() => _characterRepository.GetCharacterState(_model.CharacterId))
            .Returns(
                new CharacterStatusDto()
                {
                    IsInCharacterCreation = false,
                    IsPrimaryCharacter = true,
                }
            );

        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character());

        A.CallTo(() => _contactRepository.GetContactsForCharacter(_model.CharacterId)).Returns(_dbModel);

        var validator = new GetContactsModelValidator(_characterRepository);

        _useCase = new GetContactsUseCase(
            _contactRepository,
            _characterRepository,
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
            nameof(GetContactsModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(Task.FromResult<Character?>(null));
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetContactsModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillReturn_Error_WhenCharacterIsInCharacterCreationMode()
    {
        A.CallTo(() => _characterRepository.GetCharacterState(_model.CharacterId))
            .Returns(
                new CharacterStatusDto() { IsInCharacterCreation = true, IsPrimaryCharacter = true }
            );

        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsFailed);
        Assert.Equal(
            "You cannot edit contacts while in character creation mode.",
            result.Errors[0].Message
        );
    }

    [Fact]
    public async Task UseCase_GetsContacts_WhenItHasEnoughXp()
    {
        var contacts = _dbModel.Select(x => new ContactListReturnModel()
        {
            KnowledgeLevel = x.KnowledgeLevel,
            Name = x.Name,
            Knowledge = x.Knowledge,
            Id = x.Id,
            IsApproved = x.IsApproved,
            UsesPerWeek = x.UsesPerWeek
        }).OrderBy(x => x.Name).ToList();
        
        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(contacts, results.Value);
    }
}

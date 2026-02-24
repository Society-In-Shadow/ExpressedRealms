using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Characters.Repository.Contacts.Dtos;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.UseCases.Contacts.GetContactsForCharacterSheet;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;
using ContactListReturnModel = ExpressedRealms.Characters.UseCases.Contacts.GetContactsForCharacterSheet.ContactListReturnModel;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Contacts;

public class GetContactsForCharacterSheetUseCaseTests
{
    private readonly GetContactsForCharacterSheetUseCase _useCase;
    private readonly ICharacterRepository _characterRepository;
    private readonly IContactRepository _contactRepository;
    private readonly GetContactsForCharacterSheetModel _model;
    private readonly List<ContactListCharacterSheetDto> _dbModel;

    public GetContactsForCharacterSheetUseCaseTests()
    {
        _model = new GetContactsForCharacterSheetModel() { CharacterId = 2 };

        _dbModel = new List<ContactListCharacterSheetDto>()
        {
            new()
            {
                Name = "Luffy",
                KnowledgeLevel = "Associate (3)",
                Knowledge = "Computer Science",
                Id = 3,
                IsApproved = false,
                UsesPerWeek = 1,
                KnowledgeDescription = "Knowledge about computers.",
                Notes = "Notes about Luffy.",
            },
            new()
            {
                Name = "Zoro",
                KnowledgeLevel = "Bachalors (5)",
                Knowledge = "Navigation",
                Id = 2,
                IsApproved = true,
                UsesPerWeek = 3,
                KnowledgeDescription = "Knowledge about navigation.",
            },
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

        A.CallTo(() => _contactRepository.GetContactsForCharacterSheet(_model.CharacterId))
            .Returns(_dbModel);

        var validator = new GetContactsForCharacterSheetModelValidator(_characterRepository);

        _useCase = new GetContactsForCharacterSheetUseCase(
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
            nameof(GetContactsForCharacterSheetModel.CharacterId),
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
            nameof(GetContactsForCharacterSheetModel.CharacterId),
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
            "You cannot get contacts while in character creation mode.",
            result.Errors[0].Message
        );
    }

    [Fact]
    public async Task UseCase_GetsContacts_WhenItHasEnoughXp()
    {
        var contacts = _dbModel
            .Select(x => new ContactListReturnModel()
            {
                KnowledgeLevel = x.KnowledgeLevel,
                Name = x.Name,
                Knowledge = x.Knowledge,
                Id = x.Id,
                IsApproved = x.IsApproved,
                UsesPerWeek = x.UsesPerWeek,
                KnowledgeDescription = x.KnowledgeDescription,
                Notes = x.Notes,
            })
            .OrderBy(x => x.Name)
            .ToList();

        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(contacts, results.Value);
    }
}

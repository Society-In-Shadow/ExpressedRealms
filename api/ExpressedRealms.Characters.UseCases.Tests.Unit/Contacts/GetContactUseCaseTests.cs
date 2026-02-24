using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.UseCases.Contacts.GetContact;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Contacts;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Contacts;

public class GetContactUseCaseTests
{
    private readonly GetContactUseCase _useCase;
    private readonly ICharacterRepository _characterRepository;
    private readonly IContactRepository _contactRepository;
    private readonly GetContactModel _model;
    private readonly Contact _dbModel;

    public GetContactUseCaseTests()
    {
        _model = new GetContactModel() { Id = 2, CharacterId = 2 };

        _dbModel = new Contact()
        {
            Name = "Luffy",
            Notes = "123",
            KnowledgeLevelId = 4,
            Frequency = 1,
            SpentXp = 6,
            CharacterId = 2,
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

        A.CallTo(() => _contactRepository.FindContactAsync(_model.Id)).Returns(_dbModel);

        var validator = new GetContactModelValidator(_characterRepository, _contactRepository);

        _useCase = new GetContactUseCase(
            _contactRepository,
            _characterRepository,
            validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItsEmpty()
    {
        _model.Id = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(nameof(GetContactModel.Id), "Contact Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _contactRepository.FindContactAsync(_model.Id))
            .Returns(Task.FromResult<Contact?>(null));
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(nameof(GetContactModel.Id), "The Contact does not exist.");
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetContactModel.CharacterId),
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
            nameof(GetContactModel.CharacterId),
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
            "You cannot view this contact while in character creation mode.",
            result.Errors[0].Message
        );
    }

    [Fact]
    public async Task UseCase_GetsContacts_WhenItHasEnoughXp()
    {
        var contacts = new ContactReturnModel()
        {
            KnowledgeLevelId = _dbModel.KnowledgeLevelId - 1, // Id to level is offset by 1
            Name = _dbModel.Name,
            KnowledgeId = _dbModel.KnowledgeId,
            Id = _dbModel.Id,
            IsApproved = _dbModel.IsApproved,
            UsesPerWeek = _dbModel.Frequency,
            Notes = _dbModel.Notes,
        };

        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(contacts, results.Value);
    }
}

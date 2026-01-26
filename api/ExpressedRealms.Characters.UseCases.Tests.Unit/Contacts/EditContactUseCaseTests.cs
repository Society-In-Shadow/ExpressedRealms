using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.Repository.Xp.Dtos;
using ExpressedRealms.Characters.UseCases.Contacts.Edit;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.DB.Models.Contacts;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Contacts;

public class EditContactUseCaseTests
{
    private readonly EditContactUseCase _useCase;
    private readonly IKnowledgeRepository _knowledgeRepository;
    private readonly ICharacterRepository _characterRepository;
    private readonly IXpRepository _xpRepository;
    private readonly IContactRepository _contactRepository;
    private readonly EditContactModel _model;
    private readonly Contact _dbModel;

    public EditContactUseCaseTests()
    {
        _model = new EditContactModel()
        {
            Id = 5,
            CharacterId = 2,
            Name = "Luffy",
            Notes = "123",
            KnowledgeLevel = 6,
            ContactFrequency = 3,
        };

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
        _knowledgeRepository = A.Fake<IKnowledgeRepository>();
        _xpRepository = A.Fake<IXpRepository>();
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

        A.CallTo(() =>
                _contactRepository.HasDuplicateName(_model.Id, _model.CharacterId, _model.Name)
            )
            .Returns(false);
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(new Character());

        A.CallTo(() => _contactRepository.FindContactAsync(_model.Id)).Returns(_dbModel);

        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(_model.CharacterId, XpSectionTypes.Contacts)
            )
            .Returns(new SectionXpDto() { AvailableXp = 30, SpentXp = 0 });

        var validator = new EditContactModelValidator(
            _knowledgeRepository,
            _characterRepository,
            _contactRepository
        );

        _useCase = new EditContactUseCase(
            _xpRepository,
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

        result.MustHaveValidationError(nameof(EditContactModel.Id), "Contact Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _contactRepository.FindContactAsync(_model.Id))
            .Returns(Task.FromResult<Contact?>(null));
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(nameof(EditContactModel.Id), "The Contact does not exist.");
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditContactModel.CharacterId),
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
            nameof(EditContactModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public async Task ValidationFor_ContactFrequency_WillFail_WhenIts_OutsideTheRange(
        byte frequency
    )
    {
        _model.ContactFrequency = frequency;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditContactModel.ContactFrequency),
            "Contact Frequency must be between 1 and 3 times per month."
        );
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public async Task ValidationFor_ContactFrequency_WillPass_WhenIts_InsideTheRange(byte frequency)
    {
        _model.ContactFrequency = frequency;
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(3)]
    [InlineData(7)]
    public async Task ValidationFor_KnowledgeLevel_WillFail_WhenIts_OutsideTheRange(byte frequency)
    {
        _model.KnowledgeLevel = frequency;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(EditContactModel.KnowledgeLevel),
            "Knowledge Level must be between level 4 and 6."
        );
    }

    [Theory]
    [InlineData(4)]
    [InlineData(6)]
    public async Task ValidationFor_KnowledgeLevel_WillPass_WhenIts_InsideTheRange(byte frequency)
    {
        _model.KnowledgeLevel = frequency;
        var result = await _useCase.ExecuteAsync(_model);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItsEmpty()
    {
        _model.Name = string.Empty;
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(nameof(EditContactModel.Name), "Name is required.");
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItsOver300CharactersLong()
    {
        _model.Name = new String('x', 301);
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(EditContactModel.Name),
            "Name must be less than 300 characters."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillFail_WhenItIsADuplicateName()
    {
        A.CallTo(() =>
                _contactRepository.HasDuplicateName(_model.Id, _model.CharacterId, _model.Name)
            )
            .Returns(true);
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(EditContactModel.Name),
            "A contact with this name already exists for this character."
        );
    }

    [Theory]
    [InlineData(" test")]
    [InlineData(" test ")]
    [InlineData("test ")]
    public async Task ValidationFor_Name_WillTrimName_ForDuplicateNameCheck(string name)
    {
        _model.Name = name;
        A.CallTo(() =>
                _contactRepository.HasDuplicateName(
                    _model.Id,
                    _model.CharacterId,
                    _model.Name.Trim()
                )
            )
            .Returns(true);
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(EditContactModel.Name),
            "A contact with this name already exists for this character."
        );
    }

    [Fact]
    public async Task ValidationFor_Name_WillNotCheckDuplication_IfCharacterIsNotFound()
    {
        A.CallTo(() => _characterRepository.FindCharacterAsync(_model.CharacterId))
            .Returns(Task.FromResult<Character?>(null));

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _contactRepository.HasDuplicateName(_model.CharacterId, _model.Name))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task ValidationFor_Notes_WillFail_WhenMaxLengthIsGreaterThan5000()
    {
        _model.Notes = new string('x', 5001);
        var result = await _useCase.ExecuteAsync(_model);
        result.MustHaveValidationError(
            nameof(EditContactModel.Notes),
            "Notes must be less than 5000 characters."
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
    public async Task UseCase_WillReturnNotEnoughXp_WhenOutOfXp()
    {
        A.CallTo(() =>
                _xpRepository.GetAvailableXpForSection(_model.CharacterId, XpSectionTypes.Contacts)
            )
            .Returns(new SectionXpDto() { AvailableXp = 0, SpentXp = _dbModel.SpentXp });

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.HasError<NotEnoughXPFailure>());
        Assert.Equal(6, ((NotEnoughXPFailure)result.Errors[0]).AvailableXP);
        Assert.Equal(26, ((NotEnoughXPFailure)result.Errors[0]).AmountTryingToSpend);
    }

    [Theory]
    [InlineData(4, 6)]
    [InlineData(5, 10)]
    [InlineData(6, 16)]
    public async Task UseCase_CalculatesKnowledgeLevel_Correctly(byte frequency, int xpCost)
    {
        _model.KnowledgeLevel = frequency;
        _model.ContactFrequency = 1; // Uses 0 XP
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _contactRepository.EditAsync(A<Contact>.That.Matches(x => x.SpentXp == xpCost))
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(2, 4)]
    [InlineData(3, 10)]
    public async Task UseCase_CalculatesFrequencyXP_Correctly(byte frequency, int xpCost)
    {
        _model.KnowledgeLevel = 4; // uses 6px
        _model.ContactFrequency = frequency;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _contactRepository.EditAsync(A<Contact>.That.Matches(x => x.SpentXp == xpCost + 6))
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_EditsContact_WhenItHasEnoughXp()
    {
        _model.Name = "Zoro";
        _model.Notes = "Notes v2";
        _model.ContactFrequency = 2;
        _model.KnowledgeLevel = 5;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _contactRepository.EditAsync(
                    A<Contact>.That.Matches(x =>
                        x.Notes == _model.Notes
                        && x.KnowledgeLevelId == _model.KnowledgeLevel + 1
                        && x.Name == _model.Name
                        && x.Frequency == _model.ContactFrequency
                        && x.SpentXp == 14
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(" test", "test")]
    [InlineData(" test ", "test")]
    [InlineData("test ", "test")]
    [InlineData(" ", null)]
    [InlineData(null, null)]
    public async Task UseCase_WillTrimNotesField(string? notes, string? savedValue)
    {
        _model.Notes = notes;

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);

        A.CallTo(() =>
                _contactRepository.EditAsync(A<Contact>.That.Matches(x => x.Notes == savedValue))
            )
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData(" test")]
    [InlineData(" test ")]
    [InlineData("test ")]
    public async Task UseCase_WillTrimNameField(string name)
    {
        _model.Name = name;

        var result = await _useCase.ExecuteAsync(_model);
        Assert.True(result.IsSuccess);

        A.CallTo(() => _contactRepository.EditAsync(A<Contact>.That.Matches(x => x.Name == "test")))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_FoundDbObject()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _contactRepository.EditAsync(A<Contact>.That.IsSameAs(_dbModel)))
            .MustHaveHappened();
    }
}

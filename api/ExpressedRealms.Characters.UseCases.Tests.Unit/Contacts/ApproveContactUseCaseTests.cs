using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Contacts;
using ExpressedRealms.Characters.Repository.DTOs;
using ExpressedRealms.Characters.UseCases.Contacts.Approve;
using ExpressedRealms.DB.Characters;
using ExpressedRealms.DB.Models.Contacts;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Characters.UseCases.Tests.Unit.Contacts;

public class ApproveContactUseCaseTests
{
    private readonly ApproveContactUseCase _useCase;
    private readonly ICharacterRepository _characterRepository;
    private readonly IContactRepository _contactRepository;
    private readonly ApproveContactModel _model;
    private readonly Contact _dbModel;

    public ApproveContactUseCaseTests()
    {
        _model = new ApproveContactModel() { Id = 5, CharacterId = 2 };

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

        var validator = new ApproveContactModelValidator(_characterRepository, _contactRepository);

        _useCase = new ApproveContactUseCase(
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

        result.MustHaveValidationError(nameof(ApproveContactModel.Id), "Contact Id is required.");
    }

    [Fact]
    public async Task ValidationFor_Id_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _contactRepository.FindContactAsync(_model.Id))
            .Returns(Task.FromResult<Contact?>(null));
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(ApproveContactModel.Id),
            "The Contact does not exist."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(ApproveContactModel.CharacterId),
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
            nameof(ApproveContactModel.CharacterId),
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

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UseCase_ApprovesContact_WhenItHasEnoughXp(bool approved)
    {
        _model.Approved = approved;
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _contactRepository.EditAsync(
                    A<Contact>.That.Matches(x => x.IsApproved == _model.Approved)
                )
            )
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

using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class ConfirmedUserInfoUseCaseTests
{
    private readonly ConfirmedUserInfoUseCase _useCase;
    private readonly ConfirmedUserInfoModelValidator _validator;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private readonly ConfirmedUserInfoModel _model;
    private const int EventId = 2;
    private Guid PlayerId = Guid.NewGuid();
    private const int CheckinId = 5;

    public ConfirmedUserInfoUseCaseTests()
    {
        _model = new ConfirmedUserInfoModel { LookupId = "ABCDEFGH" };

        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId)).Returns(true);
        A.CallTo(() => _eventCheckinRepository.GetPlayerName(_model.LookupId))
            .Returns("Test Player");
        A.CallTo(() => _eventCheckinRepository.IsFirstTimePlayer(_model.LookupId)).Returns(true);
        A.CallTo(() => _eventCheckinRepository.GetActiveEventId()).Returns(EventId);

        A.CallTo(() => _eventCheckinRepository.GetPlayerId(_model.LookupId)).Returns(PlayerId);
        A.CallTo(() => _eventCheckinRepository.GetCheckinAsync(EventId, PlayerId))
            .Returns(new Checkin { Id = CheckinId });
        A.CallTo(() => _eventCheckinRepository.GetPlayerNumber(_model.LookupId)).Returns(1);

        A.CallTo(() => _eventCheckinRepository.GetAssignedXp(PlayerId, EventId))
            .Returns(new AssignedXpTypeDto() { TypeId = 3, Amount = 10 });
        A.CallTo(() => _eventCheckinRepository.GetPrimaryCharacterInformation(PlayerId))
            .Returns(Task.FromResult<GoCheckinPrimaryCharacterInfoDto?>(null));

        _validator = new ConfirmedUserInfoModelValidator(_eventCheckinRepository);

        _useCase = new ConfirmedUserInfoUseCase(
            _eventCheckinRepository,
            _validator,
            CancellationToken.None
        );
    }

    [Fact]
    public async Task ValidationFor_LookupId_WillFail_WhenEmpty()
    {
        _model.LookupId = "";

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(nameof(_model.LookupId), "Lookup Id is required.");
    }

    [Theory]
    [InlineData("ABC")] // too short
    [InlineData("ABCDEFGHI")] // too long
    public async Task ValidationFor_LookupId_WillFail_WhenLengthIsNotEight(string invalidLookupId)
    {
        _model.LookupId = invalidLookupId;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(
            nameof(_model.LookupId),
            "Lookup Id must be 8 characters long."
        );
    }

    [Fact]
    public async Task ValidationFor_LookupId_WillFail_WhenDoesNotExistInRepository()
    {
        _model.LookupId = "ABCDEFGH";

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId))
            .Returns(false);

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveNotFoundError(nameof(_model.LookupId), "Lookup Id does not exist.");
    }

    [Fact]
    public async Task UseCase_WillReturnPlayerName()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.Equal("Test Player", results.Value.PlayerName);
    }

    [Fact]
    public async Task UseCase_WillReturn_CheckinId()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.Equal(CheckinId, results.Value.CheckinId);
    }

    [Fact]
    public async Task UseCase_WillNotCreateCheckin_IfItDoesExist()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _eventCheckinRepository.CreateCheckinAsync(A<Checkin>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task UseCase_WillCreateCheckin_IfItDoesNotExist()
    {
        A.CallTo(() => _eventCheckinRepository.GetCheckinAsync(EventId, PlayerId))
            .Returns(Task.FromResult<Checkin?>(null));

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _eventCheckinRepository.CreateCheckinAsync(
                    A<Checkin>.That.Matches(x => x.EventId == EventId && x.PlayerId == PlayerId)
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillReturn_AssignedXp()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.Equal(10, results.Value.AssignedXp!.Amount);
        Assert.Equal(3, results.Value.AssignedXp.TypeId);
    }

    [Fact]
    public async Task UseCase_WillReturn_NullablePrimaryCharacterInfo()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.Null(results.Value.PrimaryCharacterInfo);
    }

    [Fact]
    public async Task UseCase_WillReturn_PrimaryCharacterInfo()
    {
        var primaryCharacterInfo = new GoCheckinPrimaryCharacterInfoDto()
        {
            CharacterId = 3,
            CharacterName = "Test Character",
        };

        A.CallTo(() => _eventCheckinRepository.GetPrimaryCharacterInformation(PlayerId))
            .Returns(primaryCharacterInfo);

        var results = await _useCase.ExecuteAsync(_model);
        Assert.Equivalent(primaryCharacterInfo, results.Value.PrimaryCharacterInfo);
    }

    [Fact]
    public async Task UseCase_WillReturn_IfTheyAreFirstTimePlayer()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.Value.IsFirstTimeUser);
    }
}

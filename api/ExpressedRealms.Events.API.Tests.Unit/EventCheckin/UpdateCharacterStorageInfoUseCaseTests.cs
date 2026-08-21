using ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateCharacterStorageInfo;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class UpdateCharacterStorageInfoUseCaseTests
{
    private readonly UpdateCharacterStorageInfoUseCase _useCase;
    private readonly IApproveStageAndSendMessageUseCase _approveStage;
    private readonly UpdateCharacterStorageInfoModelValidator _validator;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private readonly TimeProvider _timeProvider;
    private readonly UpdateCharacterStorageInfoModel _model;

    private readonly Guid _playerId = Guid.NewGuid();
    private readonly Guid _collectorPlayerId = Guid.NewGuid();
    private readonly int _eventId = 123;
    private readonly DateTimeOffset _dateTimeNow = DateTimeOffset.UtcNow;

    public UpdateCharacterStorageInfoUseCaseTests()
    {
        _model = new UpdateCharacterStorageInfoModel
        {
            LookupId = "ABCDEFGH",
            OptedIn = true,
        };

        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();
        _approveStage = A.Fake<IApproveStageAndSendMessageUseCase>();
        _timeProvider = A.Fake<TimeProvider>();

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId)).Returns(true);

        A.CallTo(() => _eventCheckinRepository.GetPlayerId(_model.LookupId)).Returns(_playerId);

        A.CallTo(() => _eventCheckinRepository.GetActiveEventId()).Returns(_eventId);

        A.CallTo(() => _eventCheckinRepository.GetCurrentPlayerId()).Returns(_collectorPlayerId);
        
        A.CallTo(() => _eventCheckinRepository.GetCharacterStorageInfo(_playerId, _eventId)).Returns(Task.FromResult<CharacterStorageInfo?>(null));

        A.CallTo(() => _timeProvider.GetUtcNow()).Returns(_dateTimeNow);

        _validator = new UpdateCharacterStorageInfoModelValidator(_eventCheckinRepository);

        _useCase = new UpdateCharacterStorageInfoUseCase(
            _eventCheckinRepository,
            _timeProvider,
            _approveStage,
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
    public async Task UseCase_WillFail_WhenThereIsNoActiveEvent()
    {
        A.CallTo(() => _eventCheckinRepository.GetActiveEventId()).Returns(Task.FromResult<int?>(null));

        var results = await _useCase.ExecuteAsync(_model);

        Assert.True(results.IsFailed);
        Assert.Contains(
            results.Errors,
            x => x.Message == "You need an active event to update character storage."
        );

        A.CallTo(() =>
                _eventCheckinRepository.AddCharacterStorageInfo(A<CharacterStorageInfo>._)
            )
            .MustNotHaveHappened();

        A.CallTo(() => _approveStage.ExecuteAsync(A<ApproveStageAndSendMessageModel>._))
            .MustNotHaveHappened();
    }
    
    [Fact]
    public async Task UseCase_WillFail_WhenThisHasAlreadyBeenDenoted()
    {
        A.CallTo(() => _eventCheckinRepository.GetCharacterStorageInfo(_playerId, _eventId)).Returns(new CharacterStorageInfo());

        var results = await _useCase.ExecuteAsync(_model);

        Assert.True(results.IsFailed);
        Assert.Contains(
            results.Errors,
            x => x.Message == "Character Storage has already been tracked."
        );

        A.CallTo(() =>
                _eventCheckinRepository.AddCharacterStorageInfo(A<CharacterStorageInfo>._)
            )
            .MustNotHaveHappened();

        A.CallTo(() => _approveStage.ExecuteAsync(A<ApproveStageAndSendMessageModel>._))
            .MustNotHaveHappened();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UseCase_WillSave_CharacterStorageInfo(bool optedIn)
    {
        _model.OptedIn = optedIn;

        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _eventCheckinRepository.AddCharacterStorageInfo(
                    A<CharacterStorageInfo>.That.Matches(x =>
                        x.PlayerId == _playerId
                        && x.EventId == _eventId
                        && x.CollectorPlayerId == _collectorPlayerId
                        && x.Timestamp == _dateTimeNow
                        && x.OptedIn == optedIn
                        && x.Amount == 15
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillMarkCharacterStorageQuestion_StepAsComplete_AfterSaving()
    {
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() =>
                _approveStage.ExecuteAsync(
                    A<ApproveStageAndSendMessageModel>.That.Matches(x =>
                        x.LookupId == _model.LookupId
                        && x.StageId == CheckinStageEnum.CharacterStorageQuestion
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}
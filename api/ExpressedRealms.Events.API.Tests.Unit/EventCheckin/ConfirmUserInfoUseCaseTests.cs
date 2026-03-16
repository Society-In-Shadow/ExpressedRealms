using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ApproveStageAndSendMessages;
using ExpressedRealms.Events.API.UseCases.EventCheckin.ConfirmedUserInfo;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class ConfirmedUserInfoUseCaseTests
{
    private readonly ConfirmedUserInfoUseCase _useCase;
    private readonly IApproveStageAndSendMessageUseCase _approveStage;
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
        _approveStage = A.Fake<IApproveStageAndSendMessageUseCase>();

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId)).Returns(true);
        A.CallTo(() => _eventCheckinRepository.GetActiveEventId()).Returns(EventId);

        A.CallTo(() => _eventCheckinRepository.GetPlayerAsync(_model.LookupId)).Returns(new Player()
        {
            LookupId = _model.LookupId,
            Id = PlayerId,
        });
        A.CallTo(() => _eventCheckinRepository.GetCheckinAsync(EventId, PlayerId))
            .Returns(new Checkin { Id = CheckinId });
        A.CallTo(() => _eventCheckinRepository.GetPlayerNumber(_model.LookupId)).Returns(1);

        A.CallTo(() => _eventCheckinRepository.GetPrimaryCharacterInformation(PlayerId))
            .Returns(Task.FromResult<GoCheckinPrimaryCharacterInfoDto?>(null));

        _validator = new ConfirmedUserInfoModelValidator(_eventCheckinRepository);

        _useCase = new ConfirmedUserInfoUseCase(
            _eventCheckinRepository,
            _validator,
            _approveStage,
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

    /// <summary>
    /// This is needed because if the user already verified their age as 18+, it won't reprompt them to reconfirm it
    /// </summary>
    [Fact]
    public async Task UseCase_WillAutomatically_ApproveAgeCheckApprovalStage_WhenTheUserIsAnAdult()
    {
        A.CallTo(() => _eventCheckinRepository.GetPlayerAsync(_model.LookupId)).Returns(new Player()
        {
            LookupId = _model.LookupId,
            AgeGroupId = PlayerAgeGroupEnum.Adult,
            Id = PlayerId,
        });
        A.CallTo(() => _eventCheckinRepository.GetCurrentStage(CheckinId)).
            Returns(Task.FromResult<BasicInfo?>(null));
        
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _approveStage.ExecuteAsync(A<ApproveStageAndSendMessageModel>.That.Matches( x =>
                x.LookupId == _model.LookupId && 
                x.StageId == CheckinStageEnum.AgeCheckApproval)))
            .MustHaveHappenedOnceExactly();
    }
}

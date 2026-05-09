using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;
using ExpressedRealms.Events.API.UseCases.EventCheckin.GetStonePullInfo;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class GetStonePullInfoUseCaseTests
{
    private readonly GetStonePullInfoUseCase _useCase;
    private readonly GetStonePullInfoModelValidator _validator;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private readonly GetStonePullInfoModel _model;
    private const int EventId = 2;
    private Guid PlayerId = Guid.NewGuid();
    private const int CheckinId = 5;

    public GetStonePullInfoUseCaseTests()
    {
        _model = new GetStonePullInfoModel { LookupId = "ABCDEFGH" };

        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId)).Returns(true);
        A.CallTo(() => _eventCheckinRepository.IsFirstTimePlayer(_model.LookupId)).Returns(true);
        A.CallTo(() => _eventCheckinRepository.GetActiveEventId()).Returns(EventId);

        A.CallTo(() => _eventCheckinRepository.GetPlayerId(_model.LookupId)).Returns(PlayerId);
        A.CallTo(() => _eventCheckinRepository.GetCheckinAsync(EventId, PlayerId))
            .Returns(new Checkin { Id = CheckinId });
        A.CallTo(() => _eventCheckinRepository.GetPlayerNumber(_model.LookupId)).Returns(1);
        A.CallTo(() => _eventCheckinRepository.DidBringFriendToCon(CheckinId)).Returns(false);

        A.CallTo(() => _eventCheckinRepository.GetAssignedXp(PlayerId, EventId))
            .Returns(new AssignedXpTypeDto() { TypeId = 3, Amount = 10 });

        _validator = new GetStonePullInfoModelValidator(_eventCheckinRepository);

        _useCase = new GetStonePullInfoUseCase(
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
    public async Task UseCase_WillReturn_AssignedXp()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.Equal(10, results.Value.AssignedXp!.Amount);
        Assert.Equal(3, results.Value.AssignedXp.TypeId);
    }

    [Fact]
    public async Task UseCase_WillReturn_IfTheyAreFirstTimePlayer()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.Value.IsFirstTimeUser);
    }

    [Fact]
    public async Task UseCase_WillReturn_IfBroughtAFriend()
    {
        A.CallTo(() => _eventCheckinRepository.DidBringFriendToCon(CheckinId)).Returns(true);
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.Value.BroughtFriend);
    }

    [Fact]
    public async Task UseCase_WillReturn_IfStageIsComplete()
    {
        A.CallTo(() =>
                _eventCheckinRepository.GetStageStatus(CheckinId, CheckinStageEnum.AssignedXpCheck)
            )
            .Returns(true);

        var results = await _useCase.ExecuteAsync(_model);

        Assert.True(results.Value.HasCompletedStep);
    }
}

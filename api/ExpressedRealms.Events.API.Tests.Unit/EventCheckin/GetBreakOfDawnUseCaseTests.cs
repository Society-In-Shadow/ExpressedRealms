using ExpressedRealms.DB.Models.Checkins.CheckinSecondaryStatsSetup;
using ExpressedRealms.DB.Models.Checkins.CheckinSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.GetBreakOfDawnInfo;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class GetBreakOfDawnUseCaseTests
{
    private readonly GetBreakOfDawnInfoUseCase _useCase;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private readonly GetBreakOfDawnInfoModel _model;
    
    private readonly Guid _playerId = Guid.NewGuid();
    private readonly CheckinSecondaryStat _secondaryStat;

    public GetBreakOfDawnUseCaseTests()
    {
        _model = new GetBreakOfDawnInfoModel { LookupId = "ABCDEFGH" };

        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();

        _secondaryStat = new CheckinSecondaryStat()
        {
            Blood = 1,
            CheckinId = 2,
            Health = 2,
            Mortis = 3,
            Psyche = 4,
            Rwp = 5,
            Vitality = 6,
            PlayerLevel = 7,
            ExpressionId = 8
        };

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId)).Returns(true);
        A.CallTo(() => _eventCheckinRepository.GetActiveEventId())
            .Returns(3);
        A.CallTo(() => _eventCheckinRepository.GetPlayerId(_model.LookupId)).Returns(_playerId);
        A.CallTo(() => _eventCheckinRepository.GetCheckinAsync(3, _playerId))
            .Returns(new Checkin() { Id = 1 });
        A.CallTo(() => _eventCheckinRepository.GetSecondaryProficiencies(1))
            .Returns(_secondaryStat);


        var validator = new GetBreakOfDawnInfoModelValidator(_eventCheckinRepository);

        _useCase = new GetBreakOfDawnInfoUseCase(
            _eventCheckinRepository,
            validator,
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
    public async Task UseCase_Returns_BreakOfDawnInfo_With_Mapped_Proficiencies()
    {
        var results = await _useCase.ExecuteAsync(_model);

        Assert.True(results.IsSuccess);
        Assert.Equal(_secondaryStat.Vitality, results.Value.Vitality);
        Assert.Equal(_secondaryStat.Health, results.Value.Health);
        Assert.Equal(_secondaryStat.Blood, results.Value.Blood);
        Assert.Equal(_secondaryStat.Rwp, results.Value.Rwp);
        Assert.Equal(_secondaryStat.Psyche, results.Value.Psyche);
        Assert.Equal(_secondaryStat.Mortis, results.Value.Mortis);
        Assert.Equal(_secondaryStat.PlayerLevel, results.Value.CharacterLevel);
        Assert.Equal(_secondaryStat.ExpressionId, results.Value.ExpressionId);
    }
}

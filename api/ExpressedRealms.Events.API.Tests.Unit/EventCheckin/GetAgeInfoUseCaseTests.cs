using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.GetAgeInfo;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class GetAgeInfoUseCaseTests
{
    private readonly GetAgeInfoUseCase _useCase;
    private readonly GetAgeInfoModelValidator _validator;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private readonly GetAgeInfoModel _model;
    
    private readonly Player _player;

    public GetAgeInfoUseCaseTests()
    {
        _model = new GetAgeInfoModel { LookupId = "ABCDEFGH" };

        _player = new Player()
        {
            LookupId = _model.LookupId,
            LastAgeGroupCheck = new DateTime(2023, 1, 1),
            AgeGroupId = 2
        };

        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId))
            .Returns(true);
        A.CallTo(() => _eventCheckinRepository.GetActiveEventStartDate()).Returns(DateOnly.FromDateTime(new DateTime(2023, 1, 1)));
        A.CallTo(() => _eventCheckinRepository.GetPlayerAsync(_model.LookupId)).Returns(_player);


        _validator = new GetAgeInfoModelValidator(_eventCheckinRepository);

        _useCase = new GetAgeInfoUseCase(
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
    public async Task UseCase_WillReturn_AgeGroupId()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.Equal(2, results.Value.AgeGroupId);
    }

    [Fact]
    public async Task UseCase_WillNotBeVerified_IfTheyHaveNoAgeGroupChecks()
    {
        _player.LastAgeGroupCheck = null;
        var results = await _useCase.ExecuteAsync(_model);
        Assert.False(results.Value.HasBeenVerified);
    }
        
    [Fact]
    public async Task UseCase_WillNotBeVerified_IfTheyHaveBeenCheckedIn_BeforeTheStartDate()
    {;
        _player.LastAgeGroupCheck = new DateTime(2022, 1, 1);
        var results = await _useCase.ExecuteAsync(_model);
        Assert.False(results.Value.HasBeenVerified);
    }
    
    [Fact]
    public async Task UseCase_WillBeVerified_IfTheyHaveBeenCheckedIn_DayOfStartDate()
    {;
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.Value.HasBeenVerified);
    }
    
    [Fact]
    public async Task UseCase_WillBeVerified_IfTheyHaveBeenCheckedIn_LaterThenStartDate()
    {;
        _player.LastAgeGroupCheck = new DateTime(2023, 1, 3);
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.Value.HasBeenVerified);
    }

    [Fact]
    public async Task UseCase_WillBeVerified_IfTheyAreAnAdult()
    {;
        _player.AgeGroupId = PlayerAgeGroupEnum.Adult;
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.Value.HasBeenVerified);
    }
}

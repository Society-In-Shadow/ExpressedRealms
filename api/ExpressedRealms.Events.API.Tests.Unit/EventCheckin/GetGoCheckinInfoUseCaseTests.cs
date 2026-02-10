using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.GetGoCheckinInfo;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class GetGoCheckinInfoUseCaseTests
{
    private readonly GetGoCheckinInfoUseCase _useCase;
    private readonly GetGoCheckinInfoModelValidator _validator;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private GetGoCheckinInfoModel _model;

    public GetGoCheckinInfoUseCaseTests()
    {
        _model = new GetGoCheckinInfoModel
        {
            LookupId = "ABCDEFGH"
        };
        
        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();
        
        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId))
            .Returns(true);
        A.CallTo(() => _eventCheckinRepository.GetUserName(_model.LookupId)).Returns("Test Player");
        A.CallTo(() => _eventCheckinRepository.IsFirstTimePlayer(_model.LookupId)).Returns(true);

        _validator = new GetGoCheckinInfoModelValidator(_eventCheckinRepository);
        
        _useCase = new GetGoCheckinInfoUseCase(_eventCheckinRepository, _validator, CancellationToken.None);
    }
    
    
    [Fact]
    public async Task ValidationFor_LookupId_WillFail_WhenEmpty()
    {
        _model.LookupId = null;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(nameof(_model.LookupId), "Lookup Id is required.");
    }

    [Theory]
    [InlineData("ABC")]        // too short
    [InlineData("ABCDEFGHI")]  // too long
    public async Task ValidationFor_LookupId_WillFail_WhenLengthIsNotEight(string invalidLookupId)
    {
        _model.LookupId = invalidLookupId;

        var results = await _useCase.ExecuteAsync(_model);

        results.MustHaveValidationError(nameof(_model.LookupId), "Lookup Id must be 8 characters long.");
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
        Assert.Equal("Test Player", results.Value.Username);
    }
    
    [Fact]
    public async Task UseCase_WillReturn_IfTheyAreFirstTimePlayer()
    {
        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.Value.IsFirstTimeUser);
    }
}

using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using ExpressedRealms.Events.API.Repositories.EventCheckin;
using ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateAgeInformation;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace ExpressedRealms.Events.API.Tests.Unit.EventCheckin;

public class UpdateAgeInformationUseCaseTests
{
    private readonly UpdateAgeInformationUseCase _useCase;
    private readonly UpdateAgeInformationModelValidator _validator;
    private readonly IEventCheckinRepository _eventCheckinRepository;
    private readonly TimeProvider _timeProvider;
    private readonly UserManager<User> _userManager;
    private readonly UpdateAgeInformationModel _model;

    private readonly User _user = new User();
    private readonly Player _player;
    private readonly DateTime _dateTimeNow = DateTime.UtcNow;

    public UpdateAgeInformationUseCaseTests()
    {
        _model = new UpdateAgeInformationModel { LookupId = "ABCDEFGH", AgeGroupId = PlayerAgeGroupEnum.Adult};
        _player = new Player() { LookupId = _model.LookupId};
        _eventCheckinRepository = A.Fake<IEventCheckinRepository>();
        
        var store = A.Fake<IUserStore<User>>();
        _timeProvider = A.Fake<TimeProvider>();

        _userManager = A.Fake<UserManager<User>>(x => x.WithArgumentsForConstructor(
        [
            store,
            null, // IOptions<IdentityOptions>
            null, // IPasswordHasher<User>
            null, // IEnumerable<IUserValidator<User>>
            null, // IEnumerable<IPasswordValidator<User>>
            null, // ILookupNormalizer
            null, // IdentityErrorDescriber
            null, // IServiceProvider
            null  // ILogger<UserManager<User>>
        ]));

        A.CallTo(() => _eventCheckinRepository.CheckinIdExistsAsync(_model.LookupId)).Returns(true);

        A.CallTo(() => _eventCheckinRepository.GetPlayerAsync(_model.LookupId)).Returns(_player);

        A.CallTo(() => _userManager.FindByIdAsync(A<string>._)).Returns(_user);
        
        A.CallTo(() => _timeProvider.GetUtcNow()).Returns(_dateTimeNow);
        
        _validator = new UpdateAgeInformationModelValidator(_eventCheckinRepository);

        _useCase = new UpdateAgeInformationUseCase(
            _eventCheckinRepository,
            _userManager,
            _timeProvider,
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

    public static IEnumerable<object[]> LookupIds =>
        PlayerAgeGroupEnum.List.Select(id => new object[] { id });
    
    [Theory]
    [MemberData(nameof(LookupIds))]
    public async Task UseCase_WillSave_AgeGroupId(PlayerAgeGroupEnum ageGroup)
    {
        _model.AgeGroupId = ageGroup;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _eventCheckinRepository.EditAsync(A<Player>.That.Matches(k => k.AgeGroupId == ageGroup)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillSave_WaiverStatus()
    {
        _model.HasSignedConsentForm = true;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _eventCheckinRepository.EditAsync(A<Player>.That.Matches(k => k.HasSignedConsentForm)))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task UseCase_WillUpdateTimestamp_PerCall()
    {
        _model.HasSignedConsentForm = true;
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() => _eventCheckinRepository.EditAsync(A<Player>.That.Matches(k => k.LastAgeGroupCheck == _dateTimeNow)))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UseCase_WillDisableUserAccount_IfTheyAreReportedUnder13YearsOld()
    {
        _model.AgeGroupId = PlayerAgeGroupEnum.Child.Value;
        
        await _useCase.ExecuteAsync(_model);

        A.CallTo(() => _userManager.SetLockoutEndDateAsync(A<User>.That.IsSameAs(_user), DateTimeOffset.MaxValue)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _userManager.UpdateSecurityStampAsync(A<User>.That.IsSameAs(_user))).MustHaveHappenedOnceExactly();
    }
}

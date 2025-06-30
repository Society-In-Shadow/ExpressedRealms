using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.Powers.Repository.PowerPrerequisites;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.CreatePrerequisite;
using ExpressedRealms.Powers.Repository.Powers;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Powers.Repository.Tests.Unit;

public class CreatePrerequisiteTest
{
    private readonly CreatePrerequisiteUseCase _useCase;
    private readonly IPowerPrerequisitesRepository repository;
    private readonly IPowerRepository powerRepository;
    private readonly CreatePrerequisiteModelValidator validator;
    private readonly CancellationToken cancellationToken;
    
    public CreatePrerequisiteTest()
    {
        repository = A.Fake<IPowerPrerequisitesRepository>();
        powerRepository = A.Fake<IPowerRepository>();
        A.CallTo(() => powerRepository.IsValidPower(A<int>.Ignored)).Returns(true);
        A.CallTo(() => powerRepository.AreValidPowers(A<List<int>>.Ignored)).Returns(true);
        A.CallTo(() => powerRepository.RequirementAlreadyExists(A<int>.Ignored)).Returns(false);
        A.CallTo(() => repository.AddPrerequisite(A<PowerPrerequisite>.Ignored)).Returns(6);
        validator = new CreatePrerequisiteModelValidator(powerRepository);
        cancellationToken = CancellationToken.None;

        _useCase = new CreatePrerequisiteUseCase(repository, validator, cancellationToken);
    }


    [Fact]
    public async Task WillFail_WhenPrerequisiteAlreadyExists()
    {
        var model = new CreatePrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = 1,
            PowerIds = new List<int>() { 1, 2, 3 }
        };
        
        A.CallTo(() => powerRepository.RequirementAlreadyExists(A<int>.Ignored)).Returns(true);
        
        var results = await _useCase.ExecuteAsync(model);
        Assert.False(results.IsSuccess);
        Assert.Single(results.Errors);
        Assert.True(results.HasValidationError(nameof(CreatePrerequisiteModel.Id), "A Power Requirement already exists for this power."));
    }

    [Fact]
    public async Task WillFail_IfPowerDoesntExist()
    {
        var model = new CreatePrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = 1,
            PowerIds = new List<int>() { 1, 2, 3 }
        };
        
        A.CallTo(() => powerRepository.IsValidPower(A<int>.Ignored)).Returns(false);
        
        var results = await _useCase.ExecuteAsync(model);
        Assert.False(results.IsSuccess);
        Assert.Single(results.Errors);
        Assert.True(results.HasValidationError(nameof(CreatePrerequisiteModel.Id), "Invalid Power."));
    }
    
    [Fact]
    public async Task WillFail_IfAnyPowerIdDoesntExist()
    {
        var model = new CreatePrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = 1,
            PowerIds = new List<int>() { 1, 2, 3 }
        };
        
        A.CallTo(() => powerRepository.AreValidPowers(A<List<int>>.Ignored)).Returns(false);
        
        var results = await _useCase.ExecuteAsync(model);
        Assert.False(results.IsSuccess);
        Assert.Single(results.Errors);
        Assert.True(results.HasValidationError(nameof(CreatePrerequisiteModel.PowerIds), "One or more powers are invalid."));
    }
    
    [Theory]
    [InlineData(-3)]
    [InlineData(0)]
    [InlineData(-7)]
    public async Task WillFail_IfRequiredAmountIsLessThenNegativeTwo(int requiredAmount)
    {
        var model = new CreatePrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = requiredAmount,
            PowerIds = new List<int>() { 1, 2, 3 }
        };
        
        var results = await _useCase.ExecuteAsync(model);
        Assert.False(results.IsSuccess);
        Assert.Single(results.Errors);
        Assert.True(results.HasValidationError(nameof(CreatePrerequisiteModel.RequiredAmount), "Required Amount can only be a value greater then 0, or -1 or -2"));
    }
    
    [Fact]
    public async Task WillSucceed_IfRequiredAmountIsGreaterThen0()
    {
        var model = new CreatePrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = 1,
            PowerIds = new List<int>() { 1, 2, 3 }
        };
        
        var results = await _useCase.ExecuteAsync(model);
        Assert.True(results.IsSuccess);
    }
    
    [Fact]
    public async Task WillSucceed_IfSetToAnyPower()
    {
        var model = new CreatePrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = -1,
            PowerIds = new List<int>() { 1, 2, 3 }
        };
        
        var results = await _useCase.ExecuteAsync(model);
        Assert.True(results.IsSuccess);
    }
    
    [Fact]
    public async Task WillSucceed_IfSetToAllPowers()
    {
        var model = new CreatePrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = -2,
            PowerIds = new List<int>() { 1, 2, 3 }
        };
        
        var results = await _useCase.ExecuteAsync(model);
        Assert.True(results.IsSuccess);
    }
    
    
    [Fact]
    public async Task WillAdd_Prerequisite()
    {
        var model = new CreatePrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = 1,
            PowerIds = new List<int>() { 1, 2, 3 }
        };
        
        await _useCase.ExecuteAsync(model);
        A.CallTo(() => repository.AddPrerequisite(A<PowerPrerequisite>.That.Matches(p => 
            p.PowerId == model.Id && 
            p.RequiredAmount == model.RequiredAmount))
        ).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task WillAdd_PrerequisitePowers()
    {
        var model = new CreatePrerequisiteModel()
        {
            Id = 3,
            RequiredAmount = 1,
            PowerIds = new List<int>() { 1, 2, 3 }
        };
        
        await _useCase.ExecuteAsync(model);

        var requisitePowers = model.PowerIds.Select(x => new PowerPrerequisitePower()
        {
            PrerequisiteId = 6,
            PowerId = x,
        }).ToList();
        
        A.CallTo(() => repository.AddPrerequisitePowers(A<List<PowerPrerequisitePower>>.That.Matches(list => 
                    list.Count == requisitePowers.Count &&
                    list.All(item => requisitePowers.Any(req => 
                        req.PrerequisiteId == item.PrerequisiteId && 
                        req.PowerId == item.PowerId))
                ))
        ).MustHaveHappenedOnceExactly();
    }
}
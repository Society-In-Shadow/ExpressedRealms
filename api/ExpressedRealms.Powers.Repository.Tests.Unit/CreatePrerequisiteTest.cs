﻿using ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;
using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using ExpressedRealms.Powers.Repository.PowerPrerequisites;
using ExpressedRealms.Powers.Repository.PowerPrerequisites.CreatePrerequisiteUseCase;
using ExpressedRealms.Powers.Repository.Powers;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Powers.Repository.Tests.Unit;

public class CreatePrerequisiteTest
{
    private readonly CreatePrerequisiteUseCase _useCase;
    private readonly IPowerPrerequisitesRepository _repository;
    private readonly IPowerRepository _powerRepository;
    private readonly CreatePrerequisiteModel _model;
    private const int ReturnedPowerId = 6;

    public CreatePrerequisiteTest()
    {
        _model = new CreatePrerequisiteModel()
        {
            PowerId = 3,
            RequiredAmount = 1,
            PrerequisitePowerIds = [1, 2, 3],
        };

        _repository = A.Fake<IPowerPrerequisitesRepository>();
        _powerRepository = A.Fake<IPowerRepository>();

        A.CallTo(() => _powerRepository.IsValidPower(A<int>.Ignored)).Returns(true);
        A.CallTo(() => _powerRepository.AreValidPowers(A<List<int>>.Ignored)).Returns(true);
        A.CallTo(() => _powerRepository.RequirementAlreadyExists(A<int>.Ignored)).Returns(false);
        A.CallTo(() => _repository.AddPrerequisite(A<PowerPrerequisite>.Ignored))
            .Returns(ReturnedPowerId);

        var validator = new CreatePrerequisiteModelValidator(_powerRepository);

        _useCase = new CreatePrerequisiteUseCase(_repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task WillFail_WhenPrerequisiteAlreadyExists()
    {
        A.CallTo(() => _powerRepository.RequirementAlreadyExists(A<int>.Ignored)).Returns(true);

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(
            nameof(CreatePrerequisiteModel.PowerId),
            "A Power Requirement already exists for this power."
        );
    }

    [Fact]
    public async Task WillFail_IfPowerIdDoesNotExist()
    {
        A.CallTo(() => _powerRepository.IsValidPower(A<int>.Ignored)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(nameof(CreatePrerequisiteModel.PowerId), "Invalid Power.");
    }

    [Fact]
    public async Task WillFail_IfAnyPrerequisitePowersDoesNotExist()
    {
        A.CallTo(() => _powerRepository.AreValidPowers(A<List<int>>.Ignored)).Returns(false);

        var results = await _useCase.ExecuteAsync(_model);

        results.HasValidationError(
            nameof(CreatePrerequisiteModel.PrerequisitePowerIds),
            "One or more prerequisite powers are invalid."
        );
    }

    [Theory]
    [InlineData(-3)]
    [InlineData(0)]
    [InlineData(-7)]
    public async Task WillFail_IfRequiredAmountIsLessThenNegativeTwoOrZero(int requiredAmount)
    {
        _model.RequiredAmount = requiredAmount;

        var results = await _useCase.ExecuteAsync(_model);
        results.HasValidationError(
            nameof(CreatePrerequisiteModel.RequiredAmount),
            "Required Amount can only be a value greater then 0, or -1 (All) or -2 (Any)"
        );
    }

    [Fact]
    public async Task WillSucceed_IfRequiredAmountIsGreaterThen0()
    {
        _model.RequiredAmount = 1;

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task WillSucceed_IfSetToAnyPower()
    {
        _model.RequiredAmount = -1;

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task WillSucceed_IfSetToAllPowers()
    {
        _model.RequiredAmount = -2;

        var results = await _useCase.ExecuteAsync(_model);
        Assert.True(results.IsSuccess);
    }

    [Fact]
    public async Task WillAdd_Prerequisite()
    {
        await _useCase.ExecuteAsync(_model);
        A.CallTo(() =>
                _repository.AddPrerequisite(
                    A<PowerPrerequisite>.That.Matches(p =>
                        p.PowerId == _model.PowerId && p.RequiredAmount == _model.RequiredAmount
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task WillAdd_PrerequisitePowers()
    {
        await _useCase.ExecuteAsync(_model);

        var requisitePowers = _model
            .PrerequisitePowerIds.Select(x => new PowerPrerequisitePower()
            {
                PrerequisiteId = ReturnedPowerId,
                PowerId = x,
            })
            .ToList();

        A.CallTo(() =>
                _repository.AddPrerequisitePowers(
                    A<List<PowerPrerequisitePower>>.That.Matches(list =>
                        list.Count == requisitePowers.Count
                        && list.All(item =>
                            requisitePowers.Any(req =>
                                req.PrerequisiteId == item.PrerequisiteId
                                && req.PowerId == item.PowerId
                            )
                        )
                    )
                )
            )
            .MustHaveHappenedOnceExactly();
    }
}

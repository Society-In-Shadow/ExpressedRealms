using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Blessings.Repository.CharacterBlessings.dto;
using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Get;
using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Shared.UseCases.Tests.Unit;
using FakeItEasy;
using Xunit;

namespace ExpressedRealms.Blessings.UseCases.Tests.Unit.CharacterBlessingMappings;

public class GetAssignedBlessingsUseCaseTests
{
    private readonly GetAssignedBlessingsUseCase _useCase;
    private readonly ICharacterRepository _characterRepository;
    private readonly GetAssignedBlessingsModel _model;
    private readonly List<CharacterBlessingDto> _dbModel;

    public GetAssignedBlessingsUseCaseTests()
    {
        _model = new GetAssignedBlessingsModel() { CharacterId = 2 };

        _dbModel = new List<CharacterBlessingDto>()
        {
            new CharacterBlessingDto()
            {
                Description = "asdf",
                Name = "qwer",
                LevelDescription = "qwerty",
                LevelName = "asdfgh",
                BlessingId = 1,
                BlessingLevelId = 2,
            },
            new CharacterBlessingDto()
            {
                Description = "asd",
                Name = "qwer",
                LevelDescription = "dfghj",
                LevelName = "sdfg",
                BlessingId = 3,
                BlessingLevelId = 4,
            },
        };

        var repository = A.Fake<ICharacterBlessingRepository>();
        _characterRepository = A.Fake<ICharacterRepository>();

        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId)).Returns(true);
        A.CallTo(() => repository.GetBlessingsForCharacter(_model.CharacterId)).Returns(_dbModel);

        var validator = new GetAssignedBlessingsModelValidator(_characterRepository);

        _useCase = new GetAssignedBlessingsUseCase(repository, validator, CancellationToken.None);
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItsEmpty()
    {
        _model.CharacterId = 0;
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetAssignedBlessingsModel.CharacterId),
            "Character Id is required."
        );
    }

    [Fact]
    public async Task ValidationFor_CharacterId_WillFail_WhenItDoesNotExist()
    {
        A.CallTo(() => _characterRepository.CharacterExistsAsync(_model.CharacterId))
            .Returns(false);
        var result = await _useCase.ExecuteAsync(_model);

        result.MustHaveValidationError(
            nameof(GetAssignedBlessingsModel.CharacterId),
            "The Character does not exist."
        );
    }

    [Fact]
    public async Task UseCase_WillCreateTheBlessing()
    {
        var blessingList = _dbModel
            .Select(x => new CharacterBlessingReturnModel()
            {
                Description = x.Description,
                LevelDescription = x.LevelDescription,
                LevelName = x.LevelName,
                Name = x.Name,
                BlessingLevelId = x.BlessingLevelId,
                BlessingId = x.BlessingId,
            })
            .ToList();

        var results = await _useCase.ExecuteAsync(_model);

        Assert.Equivalent(blessingList, results.Value);
    }
}

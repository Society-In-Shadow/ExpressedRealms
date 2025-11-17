using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Characters.Repository.Xp.Dtos.AssignedXpInfoDtos;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.AssignedXp.Get;

internal sealed class GetAssignedXpMappingUseCase(
    IAssignedXpMappingRepository mappingRepository,
    ICharacterRepository characterRepository,
    IEventRepository eventRepository,
    GetAssignedXpMappingModelValidator validator,
    CancellationToken cancellationToken
) : IGetAssignedXpMappingUseCase
{
    public async Task<Result<List<XpMappingInfoReturnModel>>> ExecuteAsync(GetAssignedXpMappingModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        List<XpMappingInfoDto> mappings = new();
        if (model.EventId != 0)
        {
            mappings.AddRange(await mappingRepository.GetAllEventMappingsAsync(model.EventId!));
        }
        else
        {
            var character = await characterRepository.FindCharacterAsync(model.CharacterId);
            mappings.AddRange(await mappingRepository.GetAllPlayerMappingsAsync(character!.PlayerId));
            var events = await eventRepository.GetEventsWithAvailableXp();
            mappings.AddRange(events.Select(x => new XpMappingInfoDto()
            {
                Id = -1,
                Amount = x.ConExperience,
                Character = new Repository.Xp.Dtos.AssignedXpInfoDtos.BasicInfo()
                {
                    Name = "System"
                },
                Assigner = new Repository.Xp.Dtos.AssignedXpInfoDtos.BasicInfo()
                {
                    Name = "System"
                },
                Player = new Repository.Xp.Dtos.AssignedXpInfoDtos.BasicInfo()
                {
                    Name = "System"
                },
                DateAssigned = x.StartDate.ToDateTime(TimeOnly.MinValue),
                Event = new Repository.Xp.Dtos.AssignedXpInfoDtos.BasicInfo()
                {
                    Name = $"{x.Name} ({x.StartDate.Year})"
                },
                Notes = "",
                XpType = new Repository.Xp.Dtos.AssignedXpInfoDtos.BasicInfo()
                {
                    Name = "Event XP"
                }
            }));
        }
        
        return Result.Ok(mappings.Select(x => new XpMappingInfoReturnModel()
        {
            Id = x.Id,
            DateAssigned = x.DateAssigned,
            Notes = x.Notes,
            Amount = x.Amount,
            Player = new BasicInfo()
            {
                Id = x.Player.Id,
                Name = x.Player.Name
            },
            Character = new BasicInfo()
            {
                Id = x.Character.Id,
                Name = x.Character.Name
            },
            Event = new BasicInfo()
            {
                Id = x.Event.Id,
                Name = x.Event.Name
            },
            XpType = new BasicInfo()
            {
                Id = x.XpType.Id,
                Name = x.XpType.Name,
            },
            Assigner = new BasicInfo()
            {
                Id = x.Assigner.Id,
                Name = x.Assigner.Name
            }
        })
            .OrderByDescending(x => x.DateAssigned).ToList());
    }
}

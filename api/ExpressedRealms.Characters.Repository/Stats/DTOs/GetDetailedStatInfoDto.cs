using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Characters.Repository.Stats.DTOs;

public sealed record GetDetailedStatInfoDto(int CharacterId, StatType StatTypeId);

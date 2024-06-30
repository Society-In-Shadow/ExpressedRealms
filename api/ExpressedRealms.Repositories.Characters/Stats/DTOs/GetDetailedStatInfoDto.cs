using ExpressedRealms.Repositories.Characters.Stats.Enums;

namespace ExpressedRealms.Repositories.Characters.Stats.DTOs;

public sealed record GetDetailedStatInfoDto(int CharacterId, StatType StatTypeId);

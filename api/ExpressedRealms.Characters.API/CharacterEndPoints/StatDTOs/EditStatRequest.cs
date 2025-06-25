using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.StatDTOs;

public record EditStatRequest(int CharacterId, StatType StatTypeId);

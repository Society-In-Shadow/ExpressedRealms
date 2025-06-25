using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;

public record EditStatRequest(int CharacterId, StatType StatTypeId);

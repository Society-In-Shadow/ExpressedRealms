using ExpressedRealms.Characters.Repository.Stats.Enums;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.StatDTOs;

internal record EditStatRequest(int CharacterId, StatType StatTypeId);

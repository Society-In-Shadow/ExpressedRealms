using ExpressedRealms.Characters.Repository.Wealth.Dtos;

namespace ExpressedRealms.Characters.Repository.Wealth;

public interface IWealthRepository
{
    Task<WealthInfoDto> GetWealthInfoAsync(int characterId);
}
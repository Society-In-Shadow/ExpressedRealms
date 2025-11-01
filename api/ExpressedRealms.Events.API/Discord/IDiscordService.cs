using Discord;
using FluentResults;

namespace ExpressedRealms.Events.API.Discord;

public interface IDiscordService
{
    Task<Result> SendMessageToChannelAsync(
        DiscordChannel targetChannel,
        string message,
        Embed[]? embeds = null
    );

    Task<Result> CreateEventAsync(DiscordEvent discordEvent);
}

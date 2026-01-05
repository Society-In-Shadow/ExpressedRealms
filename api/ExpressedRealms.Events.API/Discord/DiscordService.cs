using Discord;
using Discord.Rest;
using ExpressedRealms.Shared.AzureKeyVault;
using ExpressedRealms.Shared.AzureKeyVault.Secrets;
using FluentResults;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExpressedRealms.Events.API.Discord;

internal sealed class DiscordService(
    IKeyVaultManager keyVaultManager,
    ILogger<DiscordService> logger,
    IHostEnvironment environment
) : IDiscordService
{
    private readonly DiscordRestClient _discordRestClient = new();
    const ulong SocietyInShadowsGuildId = 1176957503104352347;

    private async Task<bool> SetupDiscordClient()
    {
        var discordToken = await keyVaultManager.GetSecret(DiscordSettings.DiscordBotToken);

        if (string.IsNullOrWhiteSpace(discordToken) || discordToken == "Intentionally Left Blank")
        {
            logger.LogCritical(
                "No Discord Token provided. Discord Integration will be disabled.  Actions will be treated as successful."
            );
            return false;
        }

        if (_discordRestClient.LoginState == LoginState.LoggedOut)
        {
            await _discordRestClient.LoginAsync(
                TokenType.Bot,
                await keyVaultManager.GetSecret(DiscordSettings.DiscordBotToken)
            );
        }

        return true;
    }

    public async Task<Result> SendMessageToChannelAsync(
        DiscordChannel targetChannel,
        string message,
        Embed[]? embeds = null
    )
    {
        var enabled = await SetupDiscordClient();

        if (!enabled)
        {
            return Result.Ok();
        }

        // If using locally, always use the dummy channel
        if (environment.IsDevelopment())
        {
            targetChannel = DiscordChannel.DevTestingChannel;
        }

        if (
            await _discordRestClient.GetChannelAsync((ulong)targetChannel)
            is not IMessageChannel channel
        )
        {
            logger.LogError("Unable to find {Channel}", targetChannel.ToString());
            return Result.Fail($"Unable to find {targetChannel.ToString()}");
        }

        await channel.SendMessageAsync(message, embeds: embeds);
        return Result.Ok();
    }

    public async Task<Result> CreateEventAsync(DiscordEvent discordEvent)
    {
        var enabled = await SetupDiscordClient();

        if (!enabled)
        {
            return Result.Ok();
        }

        if (environment.IsDevelopment())
        {
            // Disable Event Creation locally all together
            return Result.Ok();
        }

        var guild = await _discordRestClient.GetGuildAsync(SocietyInShadowsGuildId);

        await guild.CreateEventAsync(
            discordEvent.Name,
            discordEvent.StartDate,
            GuildScheduledEventType.External,
            location: discordEvent.Location,
            endTime: discordEvent.EndDate
        );

        return Result.Ok();
    }
}

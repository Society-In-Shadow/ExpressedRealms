using ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Shared.AzureKeyVault.Secrets;

public static class DiscordSettings
{
    public static readonly KeyVaultSecret DiscordBotToken = new("DISCORD-BOT-TOKEN");
}

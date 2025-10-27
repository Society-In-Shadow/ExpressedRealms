using ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

namespace ExpressedRealms.Authentication.AzureKeyVault.Secrets;

public static class DiscordSettings
{
    public static readonly KeyVaultSecret DiscordBotToken = new("DISCORD-BOT-TOKEN");
}

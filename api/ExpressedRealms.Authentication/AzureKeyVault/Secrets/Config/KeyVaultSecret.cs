namespace ExpressedRealms.Authentication.AzureKeyVault.Secrets.Config;

public sealed record KeyVaultSecret(string Name) : IKeyVaultSecret;

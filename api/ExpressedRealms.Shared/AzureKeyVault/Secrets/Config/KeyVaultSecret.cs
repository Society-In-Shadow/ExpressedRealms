namespace ExpressedRealms.Shared.AzureKeyVault.Secrets.Config;

public sealed record KeyVaultSecret(string Name) : IKeyVaultSecret;

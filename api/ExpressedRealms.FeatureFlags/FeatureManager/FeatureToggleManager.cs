using System.Net.Http.Json;
using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using ExpressedRealms.FeatureFlags.FeatureManager.ApiModels;

namespace ExpressedRealms.FeatureFlags.FeatureManager;

public class FeatureToggleManager : IFeatureToggleManager
{
    private HttpClient _httpClient;
    private readonly IKeyVaultManager _keyVaultManager;
    private const string FlagUrl = "/api/v1/namespaces/default/flags";

    public FeatureToggleManager(IKeyVaultManager keyVaultManager)
    {
        _keyVaultManager = keyVaultManager;
    }

    private async Task SetupClient()
    {
        _httpClient = new()
        {
            BaseAddress = new Uri(
                await _keyVaultManager.GetSecret(FeatureFlagSettings.FeatureFlagUrl)
            ),
        };
    }

    private async Task<List<Flag>> GetFeatureFlags()
    {
        var response = await _httpClient.GetAsync(FlagUrl);
        response.EnsureSuccessStatusCode();

        var root = await response.Content.ReadFromJsonAsync<Root>();
        return root!.Flags;
    }

    private async Task AddFeatureFlags(List<ReleaseFlags> codeSideFlags, List<Flag> hostSideFlags)
    {
        var addedFlags = codeSideFlags.Where(x => !hostSideFlags.Any(y => y.Key == x.Value));
        foreach (var addedFlag in addedFlags)
        {
            var response = await _httpClient.PostAsJsonAsync(
                FlagUrl,
                new
                {
                    name = addedFlag.Name,
                    key = addedFlag.Value,
                    description = addedFlag.Description,
                    type = "BOOLEAN_FLAG_TYPE",
                    enabled = false,
                }
            );
            response.EnsureSuccessStatusCode();
        }
    }

    private async Task RemoveFeatureFlags(
        List<ReleaseFlags> codeSideFlags,
        List<Flag> hostSideFlags
    )
    {
        var removedFlags = hostSideFlags.Where(x => !codeSideFlags.Any(y => y.Value == x.Key));
        foreach (var removedFlag in removedFlags)
        {
            var response = await _httpClient.DeleteAsync($"{FlagUrl}/{removedFlag.Key}");
            response.EnsureSuccessStatusCode();
        }
    }

    private async Task UpdateFeatureFlags(
        List<ReleaseFlags> codeSideFlags,
        List<Flag> hostSideFlags
    )
    {
        var matchingFlags = hostSideFlags.Where(x => codeSideFlags.Any(y => y.Value == x.Key));

        foreach (var matchingFlag in matchingFlags)
        {
            var codeSideFlag = codeSideFlags.First(x => x.Value == matchingFlag.Key);

            if (
                codeSideFlag.Name == matchingFlag.Name
                && codeSideFlag.Description == matchingFlag.Description
            )
                continue;

            matchingFlag.Name = codeSideFlag.Name;
            matchingFlag.Description = codeSideFlag.Description;

            var response = await _httpClient.PutAsJsonAsync(
                $"{FlagUrl}/{matchingFlag.Key}",
                matchingFlag
            );
            response.EnsureSuccessStatusCode();
        }
    }

    /// <summary>
    /// This is in here to make sure that the feature flag instance reflects what the codebase needs.
    /// It will automatically add and remove feature flags, in addition, it will make sure that the name and description
    /// stay consistent with the codebase.
    /// </summary>
    public async Task UpdateFeatureToggles()
    {
        await SetupClient();

        var codeSideFlags = ReleaseFlags.List.ToList();
        var hostSideFlags = await GetFeatureFlags();

        await AddFeatureFlags(codeSideFlags, hostSideFlags);
        await RemoveFeatureFlags(codeSideFlags, hostSideFlags);
        await UpdateFeatureFlags(codeSideFlags, hostSideFlags);

        _httpClient.Dispose();
    }
}

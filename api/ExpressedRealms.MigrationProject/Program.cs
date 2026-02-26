using ExpressedRealms.DB.Configuration;
using ExpressedRealms.Shared.AzureKeyVault;

var builder = WebApplication.CreateBuilder(args);

KeyVaultManager.Initialize(builder.Configuration);
builder.AddExpressedRealmsDbContext();

var app = builder.Build();

app.UseHttpsRedirection();

await app.RunAsync();

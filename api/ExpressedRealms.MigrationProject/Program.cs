using ExpressedRealms.DB;
using ExpressedRealms.Shared.AzureKeyVault;

var builder = WebApplication.CreateBuilder(args);

KeyVaultManager.Initialize(builder.Configuration);
builder.Services.AddDbContext<ExpressedRealmsDbContext>();

var app = builder.Build();

app.UseHttpsRedirection();

await app.RunAsync();

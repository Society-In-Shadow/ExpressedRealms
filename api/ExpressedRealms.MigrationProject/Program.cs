using ExpressedRealms.DB;
using ExpressedRealms.DB.Configuration;
using ExpressedRealms.Shared.AzureKeyVault;

var builder = WebApplication.CreateBuilder(args);

KeyVaultManager.Initialize(builder.Configuration);
builder.Services.AddDbContext<ExpressedRealmsDbContext>(options =>
    options.AddExpressedRealmsDbConnection()
);

var app = builder.Build();

app.UseHttpsRedirection();

await app.RunAsync();

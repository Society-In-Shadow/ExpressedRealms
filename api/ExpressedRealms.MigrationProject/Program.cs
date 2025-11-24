using ExpressedRealms.DB;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ExpressedRealmsDbContext>(
    (_, options) =>
    {
        options.UseNpgsql(
            "foo",
            postgresOptions =>
            {
                postgresOptions.MigrationsHistoryTable("_EfMigrations", "efcore");
            }
        );
    }
);

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();

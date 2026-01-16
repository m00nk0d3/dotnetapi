using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    public static void MigrateDB(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        context.Database.Migrate();
    }

    public static void AddGamesStoreDB(this WebApplicationBuilder builder)
    {
        var conString  = builder.Configuration.GetConnectionString("GamesStore"); 
        builder.Services.AddSqlite<GameStoreContext>(
            conString, 
            optionsAction:
                options => options.UseSeeding((context,_) =>
                {
                    if(!context.Set<Genre>().Any())
                    {
                        context.Set<Genre>().AddRange(
                            new Genre { Name = "Action" },
                            new Genre { Name = "Adventure" },
                            new Genre { Name = "RPG" },
                            new Genre { Name = "Strategy" },
                            new Genre { Name = "Simulation" }
                        );
                        context.SaveChanges();
                    }
                })
        );
    }

}

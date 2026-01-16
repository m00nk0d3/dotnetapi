using GameStore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        // GET /genres
        group.MapGet("/", async (GameStoreContext dbContext) =>
        {
           return await dbContext.Genres
                .Select(g => new Dtos.GenreDto(g.Id, g.Name))
                .AsNoTracking()
                .ToListAsync();
        });
    }
}

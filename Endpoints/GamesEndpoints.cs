using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    const string GetGameByIdRouteName = "GetGameById";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        // GET /games
        group.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games.Include(game => game.Genre).Select(game => new GameSummaryDto(
            Id: game.Id,
            Title: game.Title,
            Genre: game.Genre!.Name,
            Price: game.Price,
            ReleaseDate: game.ReleaseDate
        )).AsNoTracking().ToListAsync());

        //GET /games/{id}
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
           var game = await dbContext.Games.FindAsync(id);
            return game is not null ? Results.Ok(new GameDetailsDto(
                game.Id, 
                game.Title, 
                game.GenreId, 
                game.Price, 
                game.ReleaseDate)
                ) : Results.NotFound();
        }).WithName(GetGameByIdRouteName);

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new(){
                Title = newGame.Title,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            GameDetailsDto gameDto = new(
                Id: game.Id,
                Title: game.Title,
                GenreId: game.GenreId,
                Price: game.Price,
                ReleaseDate: game.ReleaseDate
            );
            return Results.CreatedAtRoute(GetGameByIdRouteName, new { id = gameDto.Id }, gameDto);
        });

        //PUT /games/{id}
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }

            game.Title = updatedGame.Title;
            game.GenreId = updatedGame.GenreId;
            game.Price = updatedGame.Price;
            game.ReleaseDate = updatedGame.ReleaseDate; 

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });

        // DELETE /games/{id}
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games.Where(game => game.Id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });
    }


}

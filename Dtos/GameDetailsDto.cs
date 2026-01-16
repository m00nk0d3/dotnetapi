namespace GameStore.Api.Dtos;

//A DTO is a contract between the API and its clients since it defines the shape of the data that will be sent over the network.
public record class GameDetailsDto(
    int Id,
    string Title,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);
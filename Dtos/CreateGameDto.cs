using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class CreateGameDto(
    [Required] [StringLength(50)] string Title,
    [Range(1, 50)]  int GenreId,
    [Required] [Range(1, 100)] decimal Price,
    [Required] DateOnly ReleaseDate
);
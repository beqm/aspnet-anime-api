namespace Application.Dtos;

public class AnimeDto
{
    public int ID { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Description { get; set; }
}
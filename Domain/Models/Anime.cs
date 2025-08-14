namespace Domain.Models;

public class Anime
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }

    private Anime(string title, string author, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        }

        Title = title;
        Author = author;
        Description = description;
    }

    public static Anime Create(string title, string author, string description)
    {
        return new Anime(title, author, description);
    }
}
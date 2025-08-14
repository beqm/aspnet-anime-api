namespace Domain.Models;

class Anime
{
    public Guid ID { get; set; }
    public required String Nome { get; set; }
    public required String Diretor { get; set; }
    public required String Resumo { get; set; }
}
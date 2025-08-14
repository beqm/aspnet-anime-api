using Domain.Models;
using Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static void SeedAnimeData(DatabaseContext context)
    {
        if (context.Animes.Any()) return;

        var animes = new List<Anime>
        {
            Anime.Create("Naruto", "Masashi Kishimoto", "Naruto Uzumaki, um jovem ninja rejeitado por sua vila, sonha em se tornar Hokage para provar seu valor. Enfrentando desafios, amizades e inimigos poderosos, sua jornada é marcada por perseverança, redenção e o poder dos laços que criou."),
            Anime.Create("One Piece", "Eiichiro Oda", "Monkey D. Luffy, um jovem com poderes de borracha após comer uma Akuma no Mi, parte em uma jornada épica para encontrar o lendário tesouro One Piece e se tornar o Rei dos Piratas. Com sua tripulação diversificada, ele enfrenta marinhas, outros piratas e mistérios de um mundo vasto e perigoso."),
            Anime.Create("Attack on Titan", "Hajime Isayama", "Em um mundo onde a humanidade vive enclausurada em cidades muradas para se proteger dos Titãs, Eren Yeager e seus amigos se juntam à luta contra essas criaturas aterrorizantes. A medida que avançam, descobrem segredos sombrios sobre o mundo, a guerra e a própria natureza dos Titãs."),
            Anime.Create("Death Note", "Tsugumi Ohba", "Light Yagami, um estudante genial, encontra um caderno sobrenatural chamado Death Note, capaz de matar qualquer pessoa cujo nome seja escrito nele. Decidido a criar um mundo sem crime, ele inicia um jogo mortal de inteligência contra o detetive L, enquanto forças sombrias observam o conflito."),
            Anime.Create("Fullmetal Alchemist", "Hiromu Arakawa", "Os irmãos Edward e Alphonse Elric pagam um preço terrível ao tentar trazer sua mãe de volta usando alquimia. Agora, Edward, com membros mecânicos, e Alphonse, com sua alma presa a uma armadura, buscam a Pedra Filosofal para recuperar seus corpos, desvendando conspirações e enfrentando os horrores da ambição humana.")
        };

        context.Animes.AddRange(animes);
        context.SaveChanges();
    }
}

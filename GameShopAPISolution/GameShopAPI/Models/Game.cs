using GameShopAPI.Models.Enums;

namespace GameShopAPI.Models
{
    public class Game
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public GameCategory Category { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? Publisher { get; set; } 
        public bool IsExpansion { get; set; } // Indique si c'est une extension 
        public int? BaseGameId { get; set; } // Clé étrangère vers le jeu de base
        public Game? BaseGame { get; set; } // Référence de navigation vers le jeu de base
        public ICollection<Game>? Expansions { get; set; } // Liste des extensions de ce jeu
    }
}

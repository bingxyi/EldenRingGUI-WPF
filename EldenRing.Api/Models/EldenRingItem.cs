using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EldenRing.Api.Models
{
    // Defino o modelo principal para os itens do jogo
    public class EldenRingItem
    {
        public int Id {get; set; }

        // Configuro o nome como obrigatório e com limite de caracteres
        [Required]
        [MaxLength(120)]
        public string Name {get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Rarity {get; set; } = null!; // Por exemplo, Comum, Rara, Lendaria, etc...
        
        // Garanto que o preço (em Runas) seja sempre um valor positivo
        [Range(0, int.MaxValue)]
        public int Price {get; set; } // Por que utilizei int ? Pois o preço é em Runas

        [MaxLength(1200)]
        public string? Description {get; set;}

        [Required]
        public int ItemCategoryId {get; set; }

        // Propriedade de navegação para a categoria, ignorada na serialização JSON para evitar ciclos
        [JsonIgnore]
        public ItemCategory? ItemCategory {get; set; }
    }
}

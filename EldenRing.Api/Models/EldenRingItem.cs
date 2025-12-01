using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EldenRing.Api.Models
{
    public class EldenRingItem
    {
        public int Id {get; set; }

        [Required]
        [MaxLength(120)]
        public string Name {get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Rarity {get; set; } = null!; // Por exemplo, Comum, Rara, Lendaria, etc...
        
        [Range(0, int.MaxValue)]
        public int Price {get; set; } // Por que utilizei int ? Pois o preço é em Runas

        [MaxLength(1200)]
        public string? Description {get; set;}

        [Required]
        public int ItemCategoryId {get; set; }

        // Propriedade de navegação
        [JsonIgnore]
        public ItemCategory? ItemCategory {get; set; }
    }
}

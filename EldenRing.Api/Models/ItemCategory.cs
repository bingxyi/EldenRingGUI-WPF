using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EldenRing.Api.Models
{
    // Defino o modelo que representa as categorias de itens
    public class ItemCategory
    {
        public int Id {get; set; }

        [Required]
        [MaxLength(60)]
        public string Name {get; set; } = null!;
        
        // Relacionamento com itens: uma categoria pode ter vários itens
        [JsonIgnore]
        public ICollection<EldenRingItem>? Items {get; set; }
    }
}

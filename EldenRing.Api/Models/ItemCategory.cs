using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EldenRing.Api.Models
{
    public class ItemCategory
    {
        public int Id {get; set; }

        [Required]
        [MaxLength(60)]
        public string Name {get; set; } = null!;
        
        [JsonIgnore]
        public ICollection<EldenRingItem>? Items {get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace EldenRing.Api.Models.Dto
{
    // Utilizo este DTO para transferir dados na criação e atualização de itens
    public class EldenRingItemCreateDto
    {
        // Aplico validações para garantir a integridade dos dados recebidos
        [Required]
        [MaxLength(120)]
        public string Name {get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Rarity {get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Price {get; set; }

        [MaxLength(1200)]
        public string? Description {get; set; }

        [Required]
        public int ItemCategoryId {get; set; }
    }
}

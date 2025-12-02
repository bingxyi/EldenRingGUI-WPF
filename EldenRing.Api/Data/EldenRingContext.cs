using EldenRing.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EldenRing.Api.Data
{
    // Represento o contexto do banco de dados para o Entity Framework Core
    public class EldenRingContext : DbContext
    {
        public EldenRingContext(DbContextOptions<EldenRingContext> options) : base(options) { }

        // Mapeio as tabelas do banco de dados para objetos C#
        public DbSet<ItemCategory> ItemCategories { get; set; } = null!;
        public DbSet<EldenRingItem> EldenRingItems { get; set; } = null!;

        // Configuro o modelo de dados e insiro dados iniciais (seeding) ao criar o banco
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Populo a tabela de categorias com dados iniciais
            modelBuilder.Entity<ItemCategory>().HasData(
                new ItemCategory { Id = 1, Name = "Armas" },
                new ItemCategory { Id = 2, Name = "Consumíveis" },
                new ItemCategory { Id = 3, Name = "Talismãs" },
                new ItemCategory { Id = 4, Name = "Invocações/Spirit Ashes" }
            );

            // Populo a tabela de itens com uma lista predefinida
            modelBuilder.Entity<EldenRingItem>().HasData(
                new EldenRingItem {
                    Id = 1,
                    Name = "Moonveil",
                    Rarity = "Lendária",
                    Price = 8000,
                    Description = "Katana mágica que dispara ondas de lâminas etéreas quando usada com habilidades especiais. Cortante e mortal contra espectros.",
                    ItemCategoryId = 1
                },
                new EldenRingItem {
                    Id = 2,
                    Name = "Rivers of Blood",
                    Rarity = "Lendária",
                    Price = 12000,
                    Description = "Katana infernal que causa sangramento massivo e possui habilidade especial que derrama lâminas de sangue.",
                    ItemCategoryId = 1
                },
                new EldenRingItem {
                    Id = 3,
                    Name = "Mimic Tear",
                    Rarity = "Épico",
                    Price = 15000,
                    Description = "Spirit Ash que invoca uma réplica do jogador — poderosa e versátil em lutas de chefe.",
                    ItemCategoryId = 4
                },
                new EldenRingItem {
                    Id = 4,
                    Name = "Malenia's Hand",
                    Rarity = "Lendária",
                    Price = 20000,
                    Description = "Relíquia associada à Malenia — objeto raro e altamente reverenciado entre colecionadores.",
                    ItemCategoryId = 3
                },
                new EldenRingItem {
                    Id = 5,
                    Name = "Erdtree's Favor",
                    Rarity = "Raro",
                    Price = 7000,
                    Description = "Talismã que aumenta HP, Stamina e carga de equipamento — símbolo da bênção da Árvore-Dourada.",
                    ItemCategoryId = 3
                },
                new EldenRingItem {
                    Id = 6,
                    Name = "Golden Halberd",
                    Rarity = "Raro",
                    Price = 6000,
                    Description = "Uma alabarda elegante banhada em ouro, equilibrada entre alcance e dano.",
                    ItemCategoryId = 1
                },
                new EldenRingItem {
                    Id = 7,
                    Name = "Flask of Crimson Tears",
                    Rarity = "Comum",
                    Price = 150,
                    Description = "Frasco que restaura HP. Essencial para qualquer aventureiro.",
                    ItemCategoryId = 2
                },
                new EldenRingItem {
                    Id = 8,
                    Name = "Flask of Cerulean Tears",
                    Rarity = "Comum",
                    Price = 150,
                    Description = "Frasco que restaura FP (energia mágica). Útil para usuários de magias.",
                    ItemCategoryId = 2
                },
                new EldenRingItem {
                    Id = 9,
                    Name = "Ritual Sword Talisman",
                    Rarity = "Raro",
                    Price = 4200,
                    Description = "Talismã que aumenta ataque físico enquanto HP estiver cheio — ideal para agressão constante.",
                    ItemCategoryId = 3
                },
                new EldenRingItem {
                    Id = 10,
                    Name = "Black Flame",
                    Rarity = "Épico",
                    Price = 9500,
                    Description = "Catalisador/arma com afinidade de chamas negras que consome inimigos com fogo sombrio.",
                    ItemCategoryId = 1
                },
                new EldenRingItem {
                    Id = 11,
                    Name = "Stonesword Key",
                    Rarity = "Raro",
                    Price = 250,
                    Description = "Chave única que pode selar ou abrir portais — item para exploração.",
                    ItemCategoryId = 2
                },
                new EldenRingItem {
                    Id = 12,
                    Name = "Great-Jar's Arsenal",
                    Rarity = "Lendária",
                    Price = 14000,
                    Description = "Coleção de armas cerimoniais — poderoso artefato com habilidades únicas.",
                    ItemCategoryId = 1
                },
                new EldenRingItem {
                    Id = 13,
                    Name = "Lord of Blood's Exultation",
                    Rarity = "Épico",
                    Price = 7800,
                    Description = "Talismã que aumenta ataque quando sangue é derramado perto do jogador — sinergia com efeito de sangramento.",
                    ItemCategoryId = 3
                },
                new EldenRingItem {
                    Id = 14,
                    Name = "Imbued Sword Key",
                    Rarity = "Raro",
                    Price = 300,
                    Description = "Chave encantada para áreas secretas — item de exploração valioso.",
                    ItemCategoryId = 2
                },
                new EldenRingItem {
                    Id = 15,
                    Name = "Dragon Communion Seal",
                    Rarity = "Raro",
                    Price = 5200,
                    Description = "Selo usado para fortalecer habilidades baseadas em dragão — amplifica magias de natureza dracônica.",
                    ItemCategoryId = 3
                },
                new EldenRingItem {
                    Id = 16,
                    Name = "Lord of Blood's Fang",
                    Rarity = "Épico",
                    Price = 10000,
                    Description = "Daga amaldiçoada que causa alto sangramento — tem narrativa ligada a grandes chefes.",
                    ItemCategoryId = 1
                },
                new EldenRingItem {
                    Id = 17,
                    Name = "Fanged Imp Ashes",
                    Rarity = "Raro",
                    Price = 1800,
                    Description = "Spirit Ash de imp com ataques rápidos — bom para distração de chefes.",
                    ItemCategoryId = 4
                },
                new EldenRingItem {
                    Id = 18,
                    Name = "Stonesword Fragment",
                    Rarity = "Comum",
                    Price = 50,
                    Description = "Fragmento de chave que pode ser combinado — material valioso para portais menores.",
                    ItemCategoryId = 2
                }
            );
        }
    }
}

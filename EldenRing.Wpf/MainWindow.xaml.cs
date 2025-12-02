using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EldenRing.Wpf
{
    // Lógica de interação da janela principal
    public partial class MainWindow : Window
    {
        private readonly HttpClient _http;
        private readonly string _apiBase;

        public MainWindow()
        {
            InitializeComponent();

            // Inicializo o cliente HTTP apontando para a API local
            _apiBase = "http://localhost:5067/api/";
            _http = new HttpClient { BaseAddress = new Uri(_apiBase) };

            // Configuro o carregamento de dados ao iniciar a janela
            Loaded += async (_, __) => await LoadDataAsync();
        }

        // Função assíncrona para buscar dados da API e preencher a interface
        private async Task LoadDataAsync()
        {
            try
            {
                // 1 — Busco a lista de categorias disponíveis
                var categories = await _http.GetFromJsonAsync<List<CategoryDto>>("Categories");

                if (categories != null)
                    CmbCategory.ItemsSource = categories;

                // 2 — Busco a lista de itens cadastrados
                var items = await _http.GetFromJsonAsync<List<ItemDto>>("Items");

                if (items == null || categories == null)
                {
                    ItemsGrid.ItemsSource = null;
                    return;
                }

                // 3 — Realizo a junção manual (merge) para associar o nome da categoria ao item
                var finalList = items.Select(i =>
                {
                    var cat = categories.FirstOrDefault(c => c.Id == i.ItemCategoryId);

                    return new ItemViewModel
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Rarity = i.Rarity,
                        Price = i.Price,
                        Description = i.Description,
                        CategoryName = cat?.Name ?? "Desconhecida"
                    };
                }).ToList();

                // Defino a fonte de dados do grid
                ItemsGrid.ItemsSource = finalList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Evento de clique para recarregar os dados manualmente
        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        // Evento de clique para adicionar um novo item
        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Valido os campos obrigatórios antes do envio
                if (string.IsNullOrWhiteSpace(TxtName.Text))
                {
                    MessageBox.Show("Nome é obrigatório.");
                    return;
                }
                if (!int.TryParse(TxtPrice.Text, out var price))
                {
                    MessageBox.Show("Preço inválido.");
                    return;
                }
                if (CmbCategory.SelectedItem == null)
                {
                    MessageBox.Show("Selecione uma categoria.");
                    return;
                }

                // Monto o objeto DTO com os dados do formulário
                var dto = new
                {
                    Name = TxtName.Text.Trim(),
                    Rarity = ((ComboBoxItem)CmbRarity.SelectedItem)?.Content?.ToString() ?? "Comum",
                    Price = price,
                    Description = TxtDescription.Text?.Trim(),
                    ItemCategoryId = ((CategoryDto)CmbCategory.SelectedItem).Id
                };

                // Envio a requisição POST para a API
                var response = await _http.PostAsJsonAsync("Items", dto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Item adicionado com sucesso.", "Sucesso",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    // Recarrego a lista e limpo os campos do formulário
                    await LoadDataAsync();

                    TxtName.Text = "";
                    TxtPrice.Text = "";
                    TxtDescription.Text = "";
                }
                else
                {
                    var txt = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Falha ao adicionar: {response.StatusCode}\n{txt}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        // Evento de clique para excluir um item selecionado
        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Verifico se o usuário selecionou um item no grid
            if (ItemsGrid.SelectedItem is not ItemViewModel item)
            {
                MessageBox.Show("Selecione um item para excluir.");
                return;
            }

            // Confirmo a intenção de exclusão
            var confirm = MessageBox.Show(
                $"Excluir '{item.Name}' (Id {item.Id})?",
                "Confirmar",
                MessageBoxButton.YesNo
            );

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                // Envio a requisição DELETE para a API
                var response = await _http.DeleteAsync($"Items/{item.Id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Excluído com sucesso.");
                    await LoadDataAsync();
                }
                else
                {
                    MessageBox.Show($"Falha ao excluir: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }
    }

    // Classe auxiliar para mapear categorias vindas da API
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    // Classe auxiliar para mapear itens vindos da API
    public class ItemDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Rarity { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }

        public int ItemCategoryId { get; set; }
    }

    // ViewModel usado no DataGrid para exibir dados combinados (Item + Nome da Categoria)
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Rarity { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }

        // Campo calculado manualmente na junção de dados
        public string? CategoryName { get; set; }
    }
}

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
    public partial class MainWindow : Window
    {
        private readonly HttpClient _http;
        private readonly string _apiBase;

        public MainWindow()
        {
            InitializeComponent();

            _apiBase = "http://localhost:5067/api/";
            _http = new HttpClient { BaseAddress = new Uri(_apiBase) };

            Loaded += async (_, __) => await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                // 1 — Carregar categorias
                var categories = await _http.GetFromJsonAsync<List<CategoryDto>>("Categories");

                if (categories != null)
                    CmbCategory.ItemsSource = categories;

                // 2 — Carregar itens
                var items = await _http.GetFromJsonAsync<List<ItemDto>>("Items");

                if (items == null || categories == null)
                {
                    ItemsGrid.ItemsSource = null;
                    return;
                }

                // 3 — Merge manual entre itens e categorias
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

                ItemsGrid.ItemsSource = finalList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}", "Erro",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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

                var dto = new
                {
                    Name = TxtName.Text.Trim(),
                    Rarity = ((ComboBoxItem)CmbRarity.SelectedItem)?.Content?.ToString() ?? "Comum",
                    Price = price,
                    Description = TxtDescription.Text?.Trim(),
                    ItemCategoryId = ((CategoryDto)CmbCategory.SelectedItem).Id
                };

                var response = await _http.PostAsJsonAsync("Items", dto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Item adicionado com sucesso.", "Sucesso",
                        MessageBoxButton.OK, MessageBoxImage.Information);

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

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsGrid.SelectedItem is not ItemViewModel item)
            {
                MessageBox.Show("Selecione um item para excluir.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Excluir '{item.Name}' (Id {item.Id})?",
                "Confirmar",
                MessageBoxButton.YesNo
            );

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
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

    // DTOs vindos da API
    public class CategoryDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class ItemDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Rarity { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }

        public int ItemCategoryId { get; set; }
    }

    // ViewModel usado no DataGrid
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Rarity { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }

        // Campo calculado manualmente
        public string? CategoryName { get; set; }
    }
}

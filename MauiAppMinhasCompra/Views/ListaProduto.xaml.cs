using System.Collections.ObjectModel;
using MauiAppMinhasCompra.Models;

namespace MauiAppMinhasCompra.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

    protected async override void OnAppearing()
    {
		List<Produto> tmp = await App.Db.GetAll();

		tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try 
		{
			Navigation.PushAsync(new Views.NovoProduto());

		} catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
    }

	private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
	{
		string q = e.NewTextValue;

		lista.Clear();

        List<Produto> tmp = await App.Db.Search(q);

        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O Total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        if (sender is MenuItem menuItem && menuItem.CommandParameter is Produto produto)
        {
            try
            {
                Console.WriteLine($"Removendo produto: {produto.Id}"); // Log antes da exclusão
                await App.Db.Delete(produto);
                Console.WriteLine($"Produto removido do banco de dados: {produto.Id}"); // Log após a exclusão do banco
                lista.Remove(produto);
                Console.WriteLine($"Produto removido da lista: {produto.Id}"); // Log após a remoção da lista
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao remover o produto: {ex.Message}", "OK");
            }
        }
    }
}
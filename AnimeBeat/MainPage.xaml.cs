using System.Collections.ObjectModel;
using System.Net.Http.Json;
using Microsoft.Maui.Controls;

namespace AnimeBeat
{
    public partial class MainPage : ContentPage
    {

        private ObservableCollection<Noticia> noticias { get; set; }

        public MainPage()
        {
            InitializeComponent();
            noticias = new ObservableCollection<Noticia> ();
            NewsListView.ItemsSource = noticias;
            LoadNoticiasAsync();
        }

        private async Task LoadNoticiasAsync()
        {
            try
            {
                using HttpClient client = new HttpClient();
                var noticiasFromApi = await client.GetFromJsonAsync<List<Noticia>>("http://172.28.41.27:8080/animebeat/v1/noticias");
                if (noticiasFromApi != null)
                {
                    noticias.Clear();
                    foreach (var noticia in noticiasFromApi)
                    {
                        noticias.Add(noticia);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show an alert)
                await DisplayAlert("Error", "Failed to load news: " + ex.Message, "OK");
            }
        }

        private async void OnViewCellTapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            var noticia = (Noticia)viewCell.BindingContext;
            await Navigation.PushAsync(new NoticiaPage(noticia));
        }
    }

}

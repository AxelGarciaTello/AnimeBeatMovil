using System.Net.Http.Json;
using Microsoft.Maui.Controls;

namespace AnimeBeat
{
    public partial class NoticiaPage : ContentPage
    {
        public NoticiaPage(Noticia noticia)
        {
            InitializeComponent();
            LoadNoticiaAsync(noticia.idNoticia);
        }

        private async Task LoadNoticiaAsync(long idNoticia)
        {
            try
            {
                using HttpClient client = new HttpClient();

                var response = await client.GetAsync("http://172.28.41.27:8080/animebeat/v1/noticias/" + idNoticia);
                if (!response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Error", $"Status: {response.StatusCode}\nContent: {content}", "OK");
                    return;
                }

                var noticiaFromAPI = await response.Content.ReadFromJsonAsync<Noticia>();
                BindingContext = noticiaFromAPI;

                if (!string.IsNullOrEmpty(noticiaFromAPI?.imagen))
                {
                    byte[] imageBytes = Convert.FromBase64String(noticiaFromAPI.imagen);
                    ImagenNoticia.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                }
                else
                {
                    ImagenNoticia.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., show an alert)
                await DisplayAlert("Error", "Failed to load news: " + ex.Message, "OK");
            }
        }
    }
}

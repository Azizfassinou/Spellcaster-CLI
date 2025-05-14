using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Dojo5_FinalProject.Services
{
    public class OpenAIService
    {
        private readonly HttpClient httpUser;
        private readonly string apiKey;
        private readonly string endpoint;

        // Constructeur
        public OpenAIService()
        {
            httpUser = new HttpClient();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            apiKey = config["Mr je dois avoir ma clé ce soir pour faire les textes; Merci d'avance !!!"];
            endpoint = "https://api.openai.com/v1/chat/completions";

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("Clé API OpenAI manquante !");
            }
        }

        // Corrige le texte en français
        public async Task<string> CorrectTextAsync(string input)
        {
            string prompt = $"Corrige les fautes en français dans :\n\"{input}\"";
            return await SendRequestAsync(prompt);
        }

        // Traduit le texte selon la locale(langue) UK ou US
        public async Task<string> TranslateTextAsync(string input, string locale)
        {
            string prompt = locale switch
            {
                "UK" => $"Traduisez ce texte en anglais britannique :\n\"{input}\"",
                "US" => $"Traduisez ce texte en anglais américain :\n\"{input}\"",
                _ => $"Traduisez ce texte en anglais (US par défaut) :\n\"{input}\""
            };

            return await SendRequestAsync(prompt);
        }

        // Méthode principale : Permet l'envoie d'une requête OpenAI

        private async Task<string> SendRequestAsync(string prompt)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpUser.DefaultRequestHeaders.Clear();
            httpUser.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await httpUser.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Erreur API : {response.StatusCode}");
                return "[Erreur API]";
            }

            var responseString = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(responseString);
            var result = doc.RootElement
                            .GetProperty("choices")[0]
                            .GetProperty("message")
                            .GetProperty("content")
                            .GetString();

            return result?.Trim() ?? "";
        }
    }
}
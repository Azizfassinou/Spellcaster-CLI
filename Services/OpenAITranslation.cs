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

            apiKey = "";
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

        // Génère du contenu HTML
        public async Task<string> GenerateWebContentAsync(string topic)
        {
            string prompt = $"Tu es un rédacteur web professionnel. " +
                $"Rédige un contenu structuré et informatif pour une page web sur le thème suivant : \"{topic}\". " +
                $"Le contenu doit avoir un titre, une introduction, 2 ou 3 sections avec titres puis une conclusion. " +
                $"T'as pas besoin d'écrire introduction ni conclusion sur la page à afficher. Propose également une mise en forme intégrée directement dans le contenu afin que si le contenu se place dans ce code HTML " +
                $"(<!DOCTYPE html>\r\n<html lang='fr'>\r\n<head>\r\n    <meta charset='UTF-8'>\r\n    <title>{{topic}}</title>\r\n    <link rel='stylesheet' href='style.css'>\r\n</head>\r\n<body>\r\n    <div class='container'>\r\n" +
                $"        <h1>{{topic}}</h1>\r\n        <article>\r\n            {{content.Replace(\"\\n\", \"<br><br>\")}}\r\n        </article>\r\n    </div>\r\n</body>\r\n</html>\";), plus précisement au niveau du content de {{content.Replace(\"\\n\", \"<br><br>\")}}, " +
                $"Faudrait surtout que la page puisse bien affichée un site claire avec une bonne mise en forme. Evite de mettre des apostrophes et ou le mot html au début et à la fin du contenu.";
            Console.Write("\n\n     Vueillez patienter pendant que la magie s'opère!!! \n \n       Vous pouvez prendre un café en attendant ...");

            return await SendRequestAsync(prompt);
        }



        // Méthode principale : Permet l'envoie d'une requête OpenAI

        private async Task<string> SendRequestAsync(string prompt)
        {
            var requestBody = new
            {
                model = "gpt-4o-mini",
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
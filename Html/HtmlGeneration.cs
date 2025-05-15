using System;
using System.Diagnostics;
using System.IO;

namespace Dojo5_FinalProject.Html
{
    public static class HtmlGenerator
    {
        
        public static void Generate(string topic, string content)
        {
            string fileName = $"Output/{topic.Replace(" ", "_")}.html";

            string html = $@"
<!DOCTYPE html>
<html lang='fr'>
<head>
    <meta charset='UTF-8'>
    <title>{topic}</title>
    <link rel='stylesheet' href='style.css'>
</head>
<body>
    <div class='container'>
        <h1>{topic}</h1>
        <article>
            {content.Replace("\n", "<br><br>")}
        </article>
    </div>
</body>
</html>";

            // Création du fichier
            File.WriteAllText(fileName, html);
            Console.WriteLine($"Fichier HTML généré : {fileName}");

            // Demander à l'utilisateur s’il veut l’ouvrir
            AskToOpenHtml(fileName);
        }

        private static void AskToOpenHtml(string filePath)
        {
            Console.Write("Souhaitez-vous ouvrir le fichier HTML ? (o/n) : ");
            string? input = Console.ReadLine()?.ToLower();
            if (input == "o" || input == "oui")
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Path.GetFullPath(filePath),
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur à l'ouverture : {ex.Message}");
                }
            }
        }
    }
}

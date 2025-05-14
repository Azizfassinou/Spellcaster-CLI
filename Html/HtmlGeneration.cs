using System;
using System.IO;
using System.Diagnostics;

namespace Dojo5_FinalProject.Html
{
    public static class HtmlGenerator
    {
        private static string OutputDir = "Output";
        private static string HtmlPath = Path.Combine(OutputDir, "result.html");
        private static string CssPath = Path.Combine(OutputDir, "style.css");

        public static void Generate(string content)
        {
            string html = GenerateHtml("Texte", WrapSection("Texte", content));
            WriteFiles(html);
            AskToOpenHtml();
        }

        private static string GenerateHtml(string title, string bodyContent)
        {
            return $@"
<!DOCTYPE html>
<html lang='fr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>{title}</title>
    <link rel='stylesheet' href='style.css'>
</head>
<body>
    <div class='container'>
        <h1>{title}</h1>
        {bodyContent}
    </div>
</body>
</html>";
        }

        private static string WrapSection(string title, string text)
        {
            return $@"
<section>
    <h2>{title}</h2>
    <pre>{System.Net.WebUtility.HtmlEncode(text)}</pre>
</section>";
        }

        private static void WriteFiles(string html)
        {
            Directory.CreateDirectory(OutputDir);// création du dossier html/CSS
            File.WriteAllText(HtmlPath, html);// crée ou écrase le fichier html
            File.WriteAllText(CssPath, GetCss()); // crée ou écrase le fichier CSS
            Console.WriteLine($"Fichier HTML généré : {HtmlPath}");
        }

        private static void AskToOpenHtml()
        {
            Console.Write("Souhaitez-vous ouvrir le fichier HTML ? (o/n) : ");
            string? input = Console.ReadLine()?.ToLower();
            if (input == "o" || input == "oui")
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Path.GetFullPath(HtmlPath),
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur à l'ouverture : {ex.Message}");
                }
            }
        }

        private static string GetCss()
        {
            return @"
body {
    font-family: 'Segoe UI', sans-serif;
    background-color: #f8f9fa;
    margin: 0;
    padding: 0;
}

.container {
    max-width: 900px;
    margin: 40px auto;
    padding: 30px;
    background: white;
    border-radius: 10px;
    box-shadow: 0 0 10px rgba(0,0,0,0.1);
}

h1 {
    text-align: center;
    color: #343a40;
    margin-bottom: 40px;
}

h2 {
    color: #007bff;
    border-bottom: 2px solid #007bff;
    padding-bottom: 5px;
}

pre {
    background: #f1f1f1;
    padding: 15px;
    border-radius: 6px;
    white-space: pre-wrap;
    word-wrap: break-word;
    line-height: 1.6;
    font-size: 16px;
}
";
        }
    }
}

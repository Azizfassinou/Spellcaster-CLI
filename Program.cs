using System;
using System.IO;
using System.Threading.Tasks;
using Dojo5_FinalProject.Services;
using Dojo5_FinalProject.Html;

namespace Dojo5_FinalProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var openAi = new OpenAIService();
            bool continuer = true;

            while (continuer)
            {
                Console.WriteLine("\n--- Menu principal ---");
                Console.WriteLine("1 - Corriger un texte");
                Console.WriteLine("2 - Traduire un texte (UK/US)");
                Console.WriteLine("3 - Générer un HTML");
                Console.WriteLine("0 - Quitter");
                Console.Write("Choisissez une Option : ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        {
                            string input = GetTextFromUser();
                            if (string.IsNullOrWhiteSpace(input)) break;

                            string corrected = await openAi.CorrectTextAsync(input);
                            Console.WriteLine("\nTexte corrigé avec succès:\n" + corrected);
                            break;
                        }

                    case "2":
                        {
                            string input = GetTextFromUser();
                            if (string.IsNullOrWhiteSpace(input)) break;

                            Console.Write("Langue (UK/US) : ");
                            string locale = Console.ReadLine()?.ToUpper() ?? "US";

                            string translated = await openAi.TranslateTextAsync(input, locale);
                            Console.WriteLine("\nTraduction :\n" + translated);
                            break;
                        }

                    case "3":
                        {
                            string input = GetTextFromUser();
                            if (string.IsNullOrWhiteSpace(input)) break;

                            HtmlGenerator.Generate(input);
                            Console.WriteLine("Fichier HTML généré !");
                            break;
                        }

                    case "0":
                        continuer = false;
                        Console.WriteLine("\n\nAziz FASSINOU vous remercie d'avoir testé son logiciel ! A bientôt ! \n\n");
                        break;

                    default:
                        Console.WriteLine("Choix invalide. Réessayez.");
                        break;
                }
            }
        }

        static string GetTextFromUser()
        {
            Console.WriteLine("1 - Saisir le texte");
            Console.WriteLine("2 - Lire un fichier dans /Input");
            string inputChoice = Console.ReadLine() ?? "";

            if (inputChoice == "1")
            {
                Console.Write("Entrez le texte : ");
                return Console.ReadLine() ?? "";
            }
            else
            {
                Console.Write("Nom du fichier (sans extension) : ");
                string fileName = Console.ReadLine() ?? "";
                string filePath = $"Input/{fileName}.txt";

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Fichier introuvable !");
                    return "";
                }

                return File.ReadAllText(filePath);
            }
        }
    }
}

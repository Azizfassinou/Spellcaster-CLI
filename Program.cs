using System;
using System.IO;
using System.Threading.Tasks;
using Dojo5_FinalProject.Services;
using Dojo5_FinalProject.Html;
using System.Diagnostics;
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
                Console.WriteLine("3 - Générer un HTML ou Lire un fichier html existant");
                Console.WriteLine("0 - Quitter");
                Console.Write("Choisissez une Option : ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        {
                            Console.Write("Entrez votre texte à corriger : \n");
                            string input = Console.ReadLine() ?? "";
                            if (string.IsNullOrWhiteSpace(input)) break;
                            string corrected = await openAi.CorrectTextAsync(input);
                            Console.WriteLine("\nTexte corrigé avec succès:\n" + corrected);
                            break;
                        }

                    case "2":
                        {
                            Console.Write("Entrez votre texte à traduire : \n");
                            string input = Console.ReadLine() ?? "";
                            if (string.IsNullOrWhiteSpace(input)) break;

                            Console.Write("Langue (UK/US) : ");
                            string locale = Console.ReadLine()?.ToUpper() ?? "US";

                            string translated = await openAi.TranslateTextAsync(input, locale);
                            Console.WriteLine("\nTraduction :\n" + translated);
                            break;
                        }

                    case "3":
                        {
                            Console.WriteLine("1 - Saisir le thème");
                            Console.WriteLine("2 - Lire plutôt un fichier html existant ? ");
                            string inputChoice = Console.ReadLine() ?? "";

                            if (inputChoice == "1")
                            {
                                Console.Write("Quel est le thème de votre page ? ");
                                string topic = Console.ReadLine() ?? "Page Web";
                                if (string.IsNullOrWhiteSpace(topic)) break;
                                string content = await openAi.GenerateWebContentAsync(topic);
                                HtmlGenerator.Generate(topic, content);
                                break;
                            }

                            else
                            {
                                Console.Write("Nom du fichier (sans extension) : ");
                                string fileName = Console.ReadLine() ?? "";
                                string filePath = $"Output/{fileName}.html";

                                if (!File.Exists(filePath))
                                {
                                    Console.WriteLine("Fichier introuvable !");
                                    break;
                                }
                                else
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
                            break;

                        }
                    case "0":
                        continuer = false;
                        Console.WriteLine("\n\nAziz FASSINOU vous remercie d'avoir testé son logiciel ! \n A bientôt ! \n\n");
                        break;

                    default:
                        Console.WriteLine("Choix invalide. Réessayez.");
                        break;
                }
            }
        }

    }
}
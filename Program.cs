using Dojo5_FinalProject.Services;
using Dojo5_FinalProject.Html;

var openAi = new OpenAIService();
bool continuer = true;

while (continuer)
{
    Console.WriteLine("\n--- Menu principal ---");
    Console.WriteLine("1 - Corriger un texte");
    Console.WriteLine("2 - Traduire un texte (UK/US)");
    Console.WriteLine("3 - Générer un HTML");
    Console.WriteLine("4 - Corriger + Traduire + Générer HTML");
    Console.WriteLine("0 - Quitter");
    Console.Write("Choisissez une Option : ");
    string choice = Console.ReadLine() ?? "";

    // Fonction interne pour obtenir du texte
    string GetTextFromUser()
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

                HtmlGenerator.Generate(input); //
                break;
            }

        case "4":
            {
                string input = GetTextFromUser();
                if (string.IsNullOrWhiteSpace(input)) break;

                string corrected = await openAi.CorrectTextAsync(input);

                Console.Write("Langue (UK/US) : ");
                string locale = Console.ReadLine()?.ToUpper() ?? "US";

                string translated = await openAi.TranslateTextAsync(corrected, locale);

                HtmlGenerator.Generate(corrected, translated);
                break;
            }

        case "0":
            continuer = false;
            Console.WriteLine("Au revoir !");
            break;

        default:
            Console.WriteLine("Choix invalide. Réessayez.");
            break;
    }
}

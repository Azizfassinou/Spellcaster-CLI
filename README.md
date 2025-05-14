                              SPELLCASTER CLI

Spellcaster CLI est une application en ligne de commande (C#) qui permet de :

- Corriger un texte en français (orthographe + grammaire)
- Traduire ce texte en anglais (UK ou US)
- Générer un fichier HTML avec le résultat


Technologies utilisées

- .NET 6 / C#
- Visual Studio 2022 
- Console (CLI)
- Fichiers HTML + CSS
- API OpenAI (GPT)


Comment lancer l’application

1. Cloner ce dépôt :

```bash
git clone https://github.com/tonpseudo/spellcaster-cli.git
cd spellcaster-cli

2. Ajouter ta clé API OpenAI :

Créer un fichier appsettings.json :
{
  "OPENAI_API_KEY": "Inserer ici votre clé API"
}

3. Assurez-vous d'avoir dotnet installé sur votre machine et lancer le programme avec la commande : dotnet run
Puis vivez votre expérience utilisateur.

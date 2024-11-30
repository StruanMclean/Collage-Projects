# User Docs

To run the program use the following command in the snakegame directory.

```
$ dotnet run
```

<span style="color: yellow">NOTE: You need to install dotnet on your chosen operating system before you can run this command. Please refer to this link to download for your platform. Today it is Sat 30th of november 2024 by the time you click this link it may not work. If it dose not work search for dotnet framework on google</span>

[Link to install](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

You will need to install SQL lite if the program dose not run. Copy the command bellow.

```
dotnet add package Microsoft.Data.Sqlite
dotnet add package SQLite
```

Once this is done run dotnet run and the program will start.

You will need to start the game every time you want to play or the game ends.

Scores are stored in ```scores.db``` this is an SQL lite database.
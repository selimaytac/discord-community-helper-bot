# Discord Community Helper Bot
***(This is my first discord bot experience, please open an issue for bugs.)***
### .Net Version: .NET CORE 3.1 ###
## Installation
```bash
git clone https://github.com/selimaytac/discord-community-helper-bot.git
```
#### In order to use the bot, you need to activate the lavalink server. You can download lavalink from [here](https://github.com/freyacodes/Lavalink). ####
## Packages & Dependencies
Name    | Version 
------------- | -------------
DSharpPlus  |   4.1.0
DSharpPlus.CommandsNext  |  4.1.0
DSharpPlus.Lavalink  |  4.1.0
## Usage
1. Enter your id and token in your Discord developer panel here.
```csharp
        public static ulong ApplicationId = 0;
        public static string Token = "";
```
2. You can edit the command icon.
```csharp
public static CommandsNextExtension GetCommandsInstance()
        {
            if (Commands == null)
                Commands = GetBotInstance().UseCommandsNext(new CommandsNextConfiguration()
                {
                    StringPrefixes = new[] { "<" }
                });
            return Commands;
        }
```
3. Edit your Lavalink server information.
```csharp
public static ConnectionEndpoint GetConnectionEndpoint()
        {
            if (Endpoint.Hostname == null)
                Endpoint =  new ConnectionEndpoint
                {
                    Hostname = "127.0.0.1",
                    Port = 2333
                };
            return Endpoint;
        }
        public static LavalinkConfiguration GetLavalinkConfiguration()
        {
            if(LavalinkConfig == null)
                LavalinkConfig =  new LavalinkConfiguration
                {
                    Password = "youshallnotpass",
                    RestEndpoint = GetConnectionEndpoint(),
                    SocketEndpoint = GetConnectionEndpoint()
                };
            return LavalinkConfig;
        }
```
#### Congratulations, you can now use the bot. ####
## Contributing ###
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
## License
[MIT](https://choosealicense.com/licenses/mit/)

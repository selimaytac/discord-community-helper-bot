using DiscordBot.Classes;
using DSharpPlus;
using DSharpPlus.Net;
using DSharpPlus.Lavalink;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            DiscordClient discord = Bot.GetBotInstance();
            var commands = Bot.GetCommandsInstance();

            var lavalinkConfig = Bot.GetLavalinkConfiguration();
            var lavalink = discord.UseLavalink();

            commands.RegisterCommands<MessageHandler>();
            commands.RegisterCommands(Assembly.GetExecutingAssembly());


            await discord.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            await Task.Delay(-1);
        }
    }
}

using DiscordBot.Classes;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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

            commands.RegisterCommands<MessageHandler>();
            commands.RegisterCommands(Assembly.GetExecutingAssembly());


            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}

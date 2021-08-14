using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Classes
{
    class Bot
    {
        private static DiscordClient Discord;
        private static CommandsNextExtension Commands;
        public static ulong ApplicationId = 0;

        public static DiscordClient GetBotInstance()
        {
            if (Discord == null)
                Discord = new DiscordClient(new DiscordConfiguration()
                {
                    Token = "",
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.AllUnprivileged,
                });

            return Discord;
        }

        public static CommandsNextExtension GetCommandsInstance()
        {
            if (Commands == null)
                Commands = Discord.UseCommandsNext(new CommandsNextConfiguration()
                {
                    StringPrefixes = new[] {"<"}
                });

            return Commands;
        }
    }


}

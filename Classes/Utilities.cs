using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Classes
{
    public class Utilities
    {
        public static async Task ClearMessages(DiscordChannel channel, int number = 2, int delay = 2000)
        {
            var dm = await channel.GetMessagesAsync(number);
            await Task.Delay(delay);
            await channel.DeleteMessagesAsync(dm);
        }
    }
}

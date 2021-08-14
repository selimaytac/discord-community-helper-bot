using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Classes
{
    class MessageHandler : BaseCommandModule
    {
        Random random = new Random();

        [Command("greet")]
        public async Task GreetCommand(CommandContext ctx, string name)
        {
            await ctx.RespondAsync("Greetings! Thank you for executing me!");
        }

        [Command("time")]
        public async Task TimeCommand(CommandContext ctx)
        {
            await ctx.RespondAsync(DateTime.Now.ToString());
        }

        [Command("Random")]
        public async Task RandomCommand(CommandContext ctx, int min, int max)
        {
            await ctx.RespondAsync($"Your random number is: {random.Next(min, max+1)}");
        }

        [Command("Dice")]
        public async Task DiceCommand(CommandContext ctx)
        {
            int number = random.Next(1, 7);
            string message;

            if (number == 6)
                message = "You rolled the dice! You're lucky it's 6!";
            else if (number == 1)
                message = "What a bad luck, it's 1.";
            else
                message = "Your rolled the dice! it's " + number;

            await ctx.RespondAsync(message);
        }
    }
}

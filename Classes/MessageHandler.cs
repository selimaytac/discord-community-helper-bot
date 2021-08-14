﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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

        [Command("hi")]
        public async Task GreetCommand(CommandContext ctx)
        {
            await ctx.RespondAsync(string.Format("hi"));
        }

        [Command("time")]
        public async Task TimeCommand(CommandContext ctx)
        {
            await ctx.RespondAsync(DateTime.Now.ToString());
        }

        [Command("Random")]
        public async Task RandomCommand(CommandContext ctx, int min, int max)
        {
            await ctx.RespondAsync($"Your random number is: {random.Next(min, max + 1)}");
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

        [Command("AddRole")]
        public async Task AddRoleCommand(CommandContext ctx, DiscordMember member, DiscordRole discordRole)
        {
            if (ctx.Member.Hierarchy >= ctx.Guild.GetMemberAsync(Bot.ApplicationId).Result.Hierarchy)
            {
                await member.GrantRoleAsync(discordRole);
                await ctx.RespondAsync($"{member.Mention} now has the {discordRole.Name} role!");
            }
            else
                await ctx.RespondAsync("You do not have enough permission.");
        }

        [Command("RemoveRole")]
        public async Task RemoveRoleCommand(CommandContext ctx, DiscordMember member, DiscordRole discordRole)
        {
            if (ctx.Member.Hierarchy >= ctx.Guild.GetMemberAsync(Bot.ApplicationId).Result.Hierarchy)
            {
                await member.RevokeRoleAsync(discordRole);
                await ctx.RespondAsync($"{member.Mention} no longer has the {discordRole.Name} role!");
            }
            else
                await ctx.RespondAsync("You do not have enough permission.");
        }

        [Command("Deaf")]
        public async Task DeafCommand(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.Hierarchy >= member.Hierarchy)
            {
                await member.SetDeafAsync(!member.IsDeafened);
                if (!member.IsDeafened)
                    await ctx.RespondAsync($"{member.Mention} deafened!");
                else
                    await ctx.RespondAsync($"{member.Mention} undeafened!");
            }
            else
                await ctx.RespondAsync("You do not have enough permission.");
        }

        [Command("Mute")]
        public async Task MuteCommand(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.Hierarchy >= member.Hierarchy)
            {
                await member.SetMuteAsync(!member.IsMuted);
                if (!member.IsMuted)
                    await ctx.RespondAsync($"{member.Mention} muted!");
                else
                    await ctx.RespondAsync($"{member.Mention} unmuted!");
            }
            else
                await ctx.RespondAsync("You do not have enough permission.");
        }

        [Command("Ban")]
        public async Task BanCommand(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.Hierarchy > member.Hierarchy)
            {
                await member.BanAsync();
                await ctx.RespondAsync($"{member.Mention} banned");
            }
            else
                await ctx.RespondAsync("You do not have enough permission.");
        }

        [Command("UnBan")]
        public async Task UnBanCommand(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.Hierarchy >= ctx.Guild.GetMemberAsync(Bot.ApplicationId).Result.Hierarchy)
            {
                await member.UnbanAsync();
                await ctx.RespondAsync($"{member.Mention} unbanned");
            }
            else
                await ctx.RespondAsync("You do not have enough permission.");
        }

        [Command("Move")]
        public async Task MoveCommand(CommandContext ctx, DiscordMember member, DiscordChannel toChannel)
        {
            DiscordChannel OldChanel = member.VoiceState.Channel;
            if (ctx.Member.Hierarchy >= ctx.Guild.GetMemberAsync(Bot.ApplicationId).Result.Hierarchy)
            {
                if (member.VoiceState.Channel == toChannel)
                    await ctx.RespondAsync($"{member.Username} is already on this channel.");
                else
                {
                    await member.PlaceInAsync(toChannel);
                    await ctx.RespondAsync($"{member.Username}  was moved from channel {OldChanel.Mention} a to channel {toChannel.Mention} by {ctx.Member.Username}.");
                }
            }
            else
                await ctx.RespondAsync("You do not have enough permission.");
        }

        [Command("MoveAll")]
        public async Task MoveAllCommand(CommandContext ctx, DiscordChannel fromChannel, DiscordChannel toChannel)
        {
            if (ctx.Member.Hierarchy >= ctx.Guild.GetMemberAsync(Bot.ApplicationId).Result.Hierarchy)
            {
                List<DiscordMember> users = await Task.Run(() => fromChannel.Users.ToList());

                if (users != null && users.Count != 0)
                    if (users.Count == users.Where(x => x.VoiceState.Channel == toChannel).ToList().Count)
                        await ctx.RespondAsync("You are already on this channel.");
                    else
                    {
                        users.ForEach(x => x.PlaceInAsync(toChannel));
                        await ctx.RespondAsync($"Everyone on channel {fromChannel.Mention} has been moved to channel {toChannel.Mention} by {ctx.Member.Username}.");
                    }
                else
                    await ctx.RespondAsync("There is no one on the channel. Please check the channel name");
            }
            else
                await ctx.RespondAsync("You do not have enough permission.");
        }

        //#region Audio
        //[Command("")]
        //public async Task Command(CommandContext ctx, DiscordMember member)
        //{

        //}
        //#endregion

        #region test
        [Command("check")]
        public async Task RoleChecker(CommandContext ctx)
        {
            await ctx.RespondAsync(ctx.Guild.GetMemberAsync(Bot.ApplicationId).Result.Mention);
        }

        // Blueprint
        [Command("3")]
        public async Task Command(CommandContext ctx, DiscordMember member)
        {
            if (ctx.Member.Hierarchy >= ctx.Guild.GetMemberAsync(Bot.ApplicationId).Result.Hierarchy)
            {
            }
            else
                await ctx.RespondAsync("You do not have enough permission.");
        }
        #endregion

    }
}

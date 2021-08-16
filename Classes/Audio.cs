using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Classes
{
    public class Audio : BaseCommandModule
    {
        List<LavalinkTrack> tracks = new List<LavalinkTrack>();
        DiscordMember GetBot(CommandContext ctx)
        {
            return ctx.Guild.GetMemberAsync(Bot.ApplicationId).Result;
        }

        [Command]
        public async Task Join(CommandContext ctx)
        {
            var lava = ctx.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("The Lavalink connection is not established");
                return;
            }

            var node = lava.ConnectedNodes.Values.First();
            var bot = GetBot(ctx);

            if (bot.VoiceState.Channel != null)
            {
                if (bot.VoiceState.Channel == ctx.Member.VoiceState.Channel)
                {
                    await ctx.RespondAsync($"{bot.Mention} is already in this channel.");
                    return;
                }
                else if (bot.VoiceState.Channel != null)
                {
                    await ctx.RespondAsync($"{bot.Mention} is in another channel.");
                    return;
                }
            }

            if (ctx.Member.VoiceState.Channel.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("Not a valid voice channel.");
                return;
            }

            await node.ConnectAsync(ctx.Member.VoiceState.Channel);
            await ctx.RespondAsync($"Joined {ctx.Member.VoiceState.Channel.Mention}!");
        }

        [Command("Join")]
        public async Task JoinWithChannelName(CommandContext ctx, DiscordChannel channel)
        {
            var lava = ctx.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("The Lavalink connection is not established");
                return;
            }

            var node = lava.ConnectedNodes.Values.First();
            var bot = GetBot(ctx);

            if (bot.VoiceState != null)
            {
                if (bot.VoiceState.Channel == channel)
                {
                    await ctx.RespondAsync($"{bot.Mention} is already in this channel.");
                    return;
                }
                else if (bot.VoiceState.Channel != null)
                {
                    await ctx.RespondAsync($"{bot.Mention} is in another voice channel.");
                    return;
                }
            }

            if (channel.Type != ChannelType.Voice)
            {
                await ctx.RespondAsync("Not a valid voice channel.");
                return;
            }

            await node.ConnectAsync(channel);
            await ctx.RespondAsync($"Joined {channel.Mention}!");
        }

        [Command]
        public async Task Play(CommandContext ctx, [RemainingText] string search)
        {
            if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
            {
                await ctx.RespondAsync("You are not in a voice channel.");
                return;
            }

            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();

            DiscordMember bot = GetBot(ctx);

            var botVoiceState = bot.VoiceState;
            if (botVoiceState == null)
                await node.ConnectAsync(ctx.Member.VoiceState.Channel);
            else
            {
                if (bot.VoiceState.Channel != ctx.Member.VoiceState.Channel && botVoiceState.Channel != null)
                {
                    await ctx.RespondAsync($"{bot.Mention} is in another voice channel.");
                    return;
                }
            }

            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null)
            {
                await ctx.RespondAsync($"{ bot.Mention} is not in a voice channel.");
                return;
            }

            var loadResult = await node.Rest.GetTracksAsync(search);

            if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed
                || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await ctx.RespondAsync($"Track search failed for {search}.");
                return;
            }


            tracks.Add(loadResult.Tracks.First());

            //var track = loadResult.Tracks.First();

            if (conn.CurrentState.CurrentTrack != null)
                if (conn.CurrentState.CurrentTrack.IsStream == false)
                    await conn.PlayAsync(tracks.First());
                else
                    conn.PlaybackFinished += Conn_PlaybackFinished;


            await ctx.RespondAsync($"Now playing {tracks.First().Title}!");
        }

        private Task Conn_PlaybackFinished(LavalinkGuildConnection sender, DSharpPlus.Lavalink.EventArgs.TrackFinishEventArgs e)
        {
            tracks.Remove(tracks.First());
            sender.PlayAsync(tracks.First());
            return Task.Delay(1000);
        }

        [Command]
        public async Task Leave(CommandContext ctx)
        {
            DiscordMember bot = GetBot(ctx);
            var lava = ctx.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await ctx.RespondAsync("The Lavalink connection is not established");
                return;
            }

            if (bot.VoiceState == null || bot.VoiceState.Channel == null)
            {
                await ctx.RespondAsync($"{bot.Mention} is not in a voice channel.");
                return;
            }

            var node = lava.ConnectedNodes.Values.First();

            if (bot.VoiceState.Channel != null)
                if (bot.VoiceState.Channel.Type != ChannelType.Voice)
                {
                    await ctx.RespondAsync("Not a valid voice channel.");
                    return;
                }

            var conn = node.GetGuildConnection(bot.VoiceState.Channel.Guild);

            if (conn == null)
            {
                await ctx.RespondAsync($"{bot.Mention} is not in any channel.");
                return;
            }

            await conn.DisconnectAsync(false);
            await ctx.RespondAsync($"Left {bot.VoiceState.Channel.Mention}!");
        }

        [Command]
        public async Task Pause(CommandContext ctx)
        {
            if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
            {
                await ctx.RespondAsync("You are not in a voice channel.");
                return;
            }

            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null)
            {
                await ctx.RespondAsync($"{GetBot(ctx).Mention} is not in a voice channel.");
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await ctx.RespondAsync("There are no tracks loaded.");
                return;
            }

            await conn.PauseAsync();
        }

        [Command]
        public async Task Resume(CommandContext ctx)
        {
            if (ctx.Member.VoiceState == null || ctx.Member.VoiceState.Channel == null)
            {
                await ctx.RespondAsync("You are not in a voice channel.");
                return;
            }

            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null)
            {
                await ctx.RespondAsync($"{GetBot(ctx).Mention} is not in a voice channel.");
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await ctx.RespondAsync("There are no tracks loaded.");
                return;
            }

            await conn.ResumeAsync();
        }

        [Command]
        public async Task Volume(CommandContext ctx, int volume)
        {
            var lava = ctx.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);

            if (conn == null)
            {
                await ctx.RespondAsync($"{GetBot(ctx).Mention} is not in a voice channel.");
                return;
            }

            //if (volume > 250)
            //    volume = 250;

            await conn.SetVolumeAsync(volume);
            await ctx.RespondAsync($"Volume set to {volume}");
        }

        [Command]
        public async Task state(CommandContext ctx)
        {
            var bot = GetBot(ctx).VoiceState;
            if (bot != null)
                if (bot.Channel != null)
                    await ctx.RespondAsync(bot.Channel.ToString());
                else
                    await ctx.RespondAsync("Clear");
        }

        [Command]
        public async Task ClearState(CommandContext ctx)
        {
            if (GetBot(ctx).VoiceState != null)
            {
                var lava = ctx.Client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                await node.ConnectAsync(ctx.Member.VoiceState.Channel);
                var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
                await conn.DisconnectAsync();
                await ctx.RespondAsync("Cleared");
            }
            else
            {
                await ctx.RespondAsync("Voicestate is already clear");
            }
        }
    }
}

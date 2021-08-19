using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Classes
{
    class Bot
    {
        private static DiscordClient Discord;
        private static CommandsNextExtension Commands;
        private static ConnectionEndpoint Endpoint;
        private static LavalinkConfiguration LavalinkConfig;


        public static ulong ApplicationId = 0;
        public static string Token = "";

        public static DiscordClient GetBotInstance()
        {
            if (Discord == null)
                Discord = new DiscordClient(new DiscordConfiguration()
                {
                    Token = Token,
                    TokenType = TokenType.Bot,
                    Intents = DiscordIntents.AllUnprivileged,
                });

            return Discord;
        }

        public static CommandsNextExtension GetCommandsInstance()
        {
            if (Commands == null)
                Commands = GetBotInstance().UseCommandsNext(new CommandsNextConfiguration()
                {
                    StringPrefixes = new[] { "<" }
                });

            return Commands;
        }

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

        public static List<LavalinkTrack> NewTrackList() {
            return new List<LavalinkTrack>();
        } 
    }
}

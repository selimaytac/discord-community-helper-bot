using DSharpPlus.Lavalink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Classes
{
    public class Events
    {
        public static Task Conn_PlaybackFinished(LavalinkGuildConnection sender, DSharpPlus.Lavalink.EventArgs.TrackFinishEventArgs e)
        {
            if (Audio.tracks != null)
                Audio.tracks.Remove(Audio.tracks.FirstOrDefault());
            if (Audio.tracks != null)
                sender.PlayAsync(Audio.tracks.FirstOrDefault());

            return Task.Delay(1000);
        }
    }
}

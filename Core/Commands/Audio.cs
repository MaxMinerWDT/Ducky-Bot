using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using System.Collections.Generic;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;


namespace Duck_Bot_.Net_Core.Core.Commands
{
    public class Audio : ModuleBase<SocketCommandContext>
    {
        public async Task playAsync(SearchMode search, params String[] query)
        {
            string output = new Addons()._out(query);
            if (output == "") { return; }
            IVoiceChannel channel = null;
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync(":x: **You must be in a voice channel to use this command!**"); return; }
            var response = await Initialization.lavalinkManager.LoadTracksAsync(output, search);
            
            if (response.Tracks.Length <= 0)
            {
                await Context.Channel.SendMessageAsync(":skull_crossbones: **No matches found**"); return;
            }

            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id) ?? await Initialization.lavalinkManager.JoinAsync<VoteLavalinkPlayer>(channel.GuildId, channel.Id);

            if (player.CurrentTrack != null && player.State == PlayerState.Playing || player.State == PlayerState.Paused)
            {
                if (response.PlaylistInfo.Name != null)
                {
                    await player.PlayAsync(response.Tracks.First());
                    for (int i = response.Tracks.Length-1; i > 0; i--)
                    {
                        await player.PlayAsync(response.Tracks[i]);
                    }
                    EmbedBuilder eb2 = new EmbedBuilder();
                    eb2.WithColor(40, 200, 150);
                    eb2.WithAuthor("Added the playlist to the queue", Context.User.GetAvatarUrl());
                    eb2.WithDescription($"**[{response.PlaylistInfo.Name}]({response.Tracks.First().Source})**");
                    eb2.AddField("Queued by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}", true);
                    eb2.AddField("Tracks in queue", player.Queue.Count - response.Tracks.Length, true);
                    eb2.WithThumbnailUrl($"http://img.youtube.com/vi/{response.Tracks.First().TrackIdentifier}/0.jpg");
                    await Context.Channel.SendMessageAsync("", false, eb2.Build());
                    return;
                }
                await player.PlayAsync(response.Tracks.First(), true);
                EmbedBuilder eb1 = new EmbedBuilder();
                eb1.WithColor(40, 200, 150);
                eb1.WithAuthor("Added to the queue", Context.User.GetAvatarUrl());
                eb1.WithDescription($"**[{response.Tracks.First().Title}]({response.Tracks.First().Source})**");
                eb1.AddField("Channel", response.Tracks.First().Author, true);
                eb1.AddField("Queued by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}", true);
                eb1.AddField("Duration", response.Tracks.First().Duration.ToString(@"hh\:mm\:ss"), true);
                eb1.AddField("Position in queue", player.Queue.Count, true);
                eb1.WithThumbnailUrl($"http://img.youtube.com/vi/{response.Tracks.First().TrackIdentifier}/0.jpg");
                await Context.Channel.SendMessageAsync("", false, eb1.Build());
                return;
            }
            if (response.PlaylistInfo.Name != null)
            {
                await player.PlayAsync(response.Tracks.First());
                for (int i = response.Tracks.Length-1; i > 0; i--)
                {
                    await player.PlayAsync(response.Tracks[i]);
                }
                EmbedBuilder eb2 = new EmbedBuilder();
                eb2.WithColor(40, 200, 150);
                eb2.WithAuthor("Playing the playlist", Context.User.GetAvatarUrl());
                eb2.WithDescription($"**[{response.PlaylistInfo.Name}]({response.Tracks.First().Source})**");
                eb2.AddField("Queued by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}", true);
                eb2.AddField("Tracks in queue", response.Tracks.Length, true);
                eb2.WithThumbnailUrl($"http://img.youtube.com/vi/{response.Tracks.First().TrackIdentifier}/0.jpg");
                await Context.Channel.SendMessageAsync("", false, eb2.Build());
                return;
            }
            await player.PlayAsync(response.Tracks.First(), false);
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(40, 200, 150);
            eb.WithAuthor("Now playing", Context.User.GetAvatarUrl());
            eb.WithDescription($"**[{player.CurrentTrack.Title}]({player.CurrentTrack.Source})**");
            eb.AddField("Channel", player.CurrentTrack.Author, true);
            eb.AddField("Queued by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}", true);
            eb.AddField("Duration", player.CurrentTrack.Duration.ToString(@"hh\:mm\:ss"), true);
            eb.AddField("Position in queue", 0, true);
            eb.WithThumbnailUrl($"http://img.youtube.com/vi/{response.Tracks.First().TrackIdentifier}/0.jpg");
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("join")]
        public async Task join()
        {
            IVoiceChannel channel = null ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync(":x: **You must be in a voice channel to use this command!**"); return; }
            VoteLavalinkPlayer player;
            if (Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id) != null)
            {
                await Context.Channel.SendMessageAsync(":x: **I'm already connected to another channel!**");
            }
            else
            {
                player = await Initialization.lavalinkManager.JoinAsync<VoteLavalinkPlayer>(channel.GuildId, channel.Id);
                await Context.Channel.SendMessageAsync(":white_check_mark: **Joined!**");
            }

        }
        [Command("leave"), Alias("leave", "disconnect")]
        public async Task leave()
        {
            IVoiceChannel channel = null ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync(":x: **You must be in a voice channel to use this command!**"); return; }
            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);
            if (player == null) { await Context.Channel.SendMessageAsync(":x: **I'm not connected to any voice channel!**"); return; }
            if (!(Context.User as IGuildUser).GuildPermissions.Administrator) { await Context.Channel.SendMessageAsync(":x: **You must be an administrator to use this command!**"); return; }
            if (channel.Id != player.VoiceChannelId) { await Context.Channel.SendMessageAsync(":x: **You must be in the same voice channel where the bot is to use this command!**"); return; }
            await player.StopAsync(true);
        }
        [Command("play"), Alias("p")]
        public async Task play(params String[] query)
        {
            await playAsync(SearchMode.YouTube, query);
        }

        [Command("skip"), Alias("s")]
        public async Task skip()
        {
            IVoiceChannel channel = null ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) {await Context.Channel.SendMessageAsync(":x: **You must be in a voice channel to use this command!**"); return; }
            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);
            if (player == null) { await Context.Channel.SendMessageAsync(":x: **I'm not connected to any voice channel!**"); return; }
            if (!(Context.User as IGuildUser).GuildPermissions.Administrator)
            {
                var skipinf = await player.VoteAsync(Context.User.Id);
                if (skipinf.WasAdded) { await Context.Channel.SendMessageAsync($":white_check_mark: **Vote submitted ({skipinf.Votes.Count}/{(skipinf.TotalUsers+1) / 2})**"); }
                if (!skipinf.WasAdded) { await Context.Channel.SendMessageAsync(":x: **You're already voted!**"); }
                if (skipinf.WasSkipped)
                {
                    await Context.Channel.SendMessageAsync($":white_check_mark: **Skipped!**");
                    if (player.CurrentTrack != null && player.State == PlayerState.Playing || player.State == PlayerState.Paused)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithColor(40, 200, 150);
                        eb.WithAuthor("Now playing", Context.User.GetAvatarUrl());
                        eb.WithDescription($"**[{player.CurrentTrack.Title}]({player.CurrentTrack.Source})**");
                        eb.AddField("Channel", player.CurrentTrack.Author, true);
                        eb.AddField("Duration", player.CurrentTrack.Duration.ToString(@"hh\:mm\:ss"), true);
                        eb.WithThumbnailUrl($"http://img.youtube.com/vi/{player.CurrentTrack.TrackIdentifier}/0.jpg");
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                    }
                }
                return;
            }        
            await player.SkipAsync();
            await Context.Channel.SendMessageAsync($":white_check_mark: **Skipped!**");
            if (player.CurrentTrack != null && player.State == PlayerState.Playing || player.State == PlayerState.Paused)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithColor(40, 200, 150);
                eb.WithAuthor("Now playing", Context.User.GetAvatarUrl());
                eb.WithDescription($"**[{player.CurrentTrack.Title}]({player.CurrentTrack.Source})**");
                eb.AddField("Channel", player.CurrentTrack.Author, true);
                eb.AddField("Duration", player.CurrentTrack.Duration.ToString(@"hh\:mm\:ss"), true);
                eb.WithThumbnailUrl($"http://img.youtube.com/vi/{player.CurrentTrack.TrackIdentifier}/0.jpg");
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
        [Command("pause")]
        public async Task pause()
        {
            IVoiceChannel channel = null ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync(":x: **You must be in a voice channel to use this command!**");  return; }
            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);
            if (player == null) {await Context.Channel.SendMessageAsync(":x: **I'm not connected to any voice channel!**"); return; }
            await player.PauseAsync();
            await Context.Channel.SendMessageAsync($":white_check_mark: **Paused!**");
        }
        [Command("resume")]
        public async Task resume()
        {
            IVoiceChannel channel = null ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync(":x: **You must be in a voice channel to use this command!**"); return; }
            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);
            if (player == null) { await Context.Channel.SendMessageAsync(":x: **I'm not connected to any voice channel!**"); return; }
            await player.ResumeAsync();
            await Context.Channel.SendMessageAsync($":white_check_mark: **Resumed!**");
        }
        [Command("volume"), Alias("vol")]
        public async Task volume(float vol)
        {
            if (vol > 100) { await Context.Channel.SendMessageAsync(":x: **Volume can't be higher then 100!**"); return; }
            IVoiceChannel channel = null ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync(":x: **You must be in a voice channel to use this command!**"); return; }
            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);
            if (player == null) { await Context.Channel.SendMessageAsync(":x: **I'm not connected to any voice channel!**"); return; }
            await player.SetVolumeAsync(vol / 100);
            await Context.Channel.SendMessageAsync($":white_check_mark: **Volume has been set to {vol}%.**");
        }
        [Command("playskip"), Alias("ps")]
        public async Task playSkip(params String[] url)
        {
            if (!(Context.User as IGuildUser).GuildPermissions.Administrator) { await Context.Channel.SendMessageAsync(":x: **You must be an administrator to use this command!**"); return; }
            string output = new Addons()._out(url);
            if (output == "") { return; }
            IVoiceChannel channel = null;
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync(":x: **You must be in a voice channel to use this command!**"); return; }
           
            var response = await Initialization.lavalinkManager.LoadTracksAsync(output, SearchMode.YouTube);

            if (response.Tracks.Length <= 0)
            {
                await Context.Channel.SendMessageAsync(":skull_crossbones: **No matches found**"); return;
            }

            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id) ?? await Initialization.lavalinkManager.JoinAsync<VoteLavalinkPlayer>(channel.GuildId, channel.Id);

            if (response.PlaylistInfo.Name != null)
            {
                await player.PlayAsync(response.Tracks.First());
                for (int i = response.Tracks.Length - 1; i > 0; i--)
                {
                    await player.PlayAsync(response.Tracks[i]);
                }
                EmbedBuilder eb2 = new EmbedBuilder();
                eb2.WithColor(40, 200, 150);
                eb2.WithAuthor("Playing the playlist", Context.User.GetAvatarUrl());
                eb2.WithDescription($"**[{response.PlaylistInfo.Name}]({response.Tracks.First().Source})**");
                eb2.AddField("Queued by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}", true);
                eb2.AddField("Tracks in queue", response.Tracks.Length, true);
                eb2.WithThumbnailUrl($"http://img.youtube.com/vi/{response.Tracks.First().TrackIdentifier}/0.jpg");
                await Context.Channel.SendMessageAsync("", false, eb2.Build());
                return;
            }

            await player.PlayAsync(response.Tracks.First(), false);
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(40, 200, 150);
            eb.WithAuthor("Now playing", Context.User.GetAvatarUrl());
            eb.WithDescription($"**[{player.CurrentTrack.Title}]({player.CurrentTrack.Source})**");
            eb.AddField("Channel", player.CurrentTrack.Author, true);
            eb.AddField("Queued by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}", true);
            eb.AddField("Duration", player.CurrentTrack.Duration, true);
            eb.AddField("Position in queue", 0, true);
            eb.WithThumbnailUrl($"http://img.youtube.com/vi/{player.CurrentTrack.TrackIdentifier}/0.jpg");
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("seek")]
        public async Task seek(int secs)
        {
            IVoiceChannel channel = null ?? (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null) { await Context.Channel.SendMessageAsync(":x: **You must be in a voice channel to use this command!**"); return; }
            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);
            if (player == null) { await Context.Channel.SendMessageAsync(":x: **I'm not connected to any voice channel!**"); return; }
            await player.SeekPositionAsync(TimeSpan.FromSeconds(secs));
            await Context.Channel.SendMessageAsync($":white_check_mark: **Track position set to {TimeSpan.FromSeconds(secs).ToString(@"hh\:mm\:ss")}**");
        }
        [Command("nowplaying"), Alias("np")]
        public async Task nowPlaying(params String[] url)
        {
            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);
            if (player == null) { await Context.Channel.SendMessageAsync(":x: **I'm not connected to any voice channel!**"); return; }
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(40, 200, 150);
            eb.WithAuthor("Now playing", Context.User.GetAvatarUrl());
            eb.WithDescription($"**[{player.CurrentTrack.Title}]({player.CurrentTrack.Source})**");
            eb.AddField("Channel", player.CurrentTrack.Author);
            eb.AddField("Current position", player.CurrentTrack.Position, true);
            eb.AddField("Duration", player.CurrentTrack.Duration, true);
            eb.WithThumbnailUrl($"http://img.youtube.com/vi/{player.CurrentTrack.TrackIdentifier}/0.jpg");
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("youtube"), Alias("yt")]
        public async Task youtube(params String[] url)
        {
            string output = new Addons()._out(url);
            IEnumerable<LavalinkTrack> response = await Initialization.lavalinkManager.GetTracksAsync(output, SearchMode.YouTube); 
            await Context.Channel.SendMessageAsync(response.First().Source);
        }
        [Command("ql"), Alias("queue")]
        public async Task queue()
        {
            VoteLavalinkPlayer player = Initialization.lavalinkManager.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);
            if (player == null) { await Context.Channel.SendMessageAsync(":x: **I'm not connected to any voice channel!**"); return; }
            if (player.Queue.Count <= 0)
            {
                await Context.Channel.SendMessageAsync(":x: **Queue is empty!**"); return;
            }
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(40, 200, 150);
            eb.WithAuthor("Tracks in queue", Context.User.GetAvatarUrl());
            string tracks = null;
            for (int i = player.Queue.Count-1; i>-1;i--)
            {
                tracks += $"**:musical_note: [{player.Queue[i].Title}]({player.Queue[i].Source})**";
                tracks += Environment.NewLine;
            }
            eb.WithDescription(tracks);
            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("scplay"), Alias("sc")]
        public async Task scplay(params String[] query)
        {
            await playAsync(SearchMode.SoundCloud, query);
        }
    }
}

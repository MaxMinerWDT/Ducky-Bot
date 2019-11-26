using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;


namespace Duck_Bot_.Net_Core.Core.Commands
{
    public class Commands : ModuleBase<SocketCommandContext> //Make next classes PUBLIC
    {
        private string[] emoji = { "🇦", "🇧", "🇨", "🇩", "🇪", "🇫", "🇬", "🇭", "🇮", "🇯", "🇰", "🇱", "🇲", "🇳", "🇴", "🇵", "🇶", "🇷", "🇸", "🇹", "🇺", "🇻", "🇼", "🇽", "🇾", "🇿" };
        [Command("info"), Alias("user")]
        public async Task UserInfo(params String[] UserL)
        {
            if (Context.IsPrivate) { return; }          
            string output = new Addons()._out(UserL);
            if (output != "")
            {

                if (output.IndexOf('@') == 1)
                {
                    try
                    {
                        output = output.Remove(0, 2);
                        output = output.Remove(output.Length - 1, 1);
                        if (output.IndexOf('!') == 0)
                        {
                            output = output.Remove(0, 1);
                        }
                        SocketGuildUser j = Context.Guild.Users.FirstOrDefault(x => x.Id == ulong.Parse(output));
                        if (j == null) { throw new Exception(); }
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"{j.Username}#{j.DiscriminatorValue}", j.GetAvatarUrl(), j.GetAvatarUrl(ImageFormat.Auto, 512));
                        eb.WithColor(40, 200, 150);
                        eb.WithThumbnailUrl(j.GetAvatarUrl());
                        eb.AddField("User ID", $"{j.Id}", true);
                        eb.AddField("Status", $"{j.Status}",true);
                        string nick = "";
                        if (j.Nickname == null) { nick = "None"; }
                        else { nick = j.Nickname; }
                        eb.AddField("Nickname", nick);
                        eb.AddField("Registered at", $"{j.CreatedAt.UtcDateTime}");
                        eb.AddField("Joined at", $"{j.JoinedAt.Value.UtcDateTime}");
                        string roles = "";
                        foreach (var role in j.Roles)
                        {
                            if (role.Name != "@everyone")
                            {
                                roles += role.Name;
                                roles += Environment.NewLine;
                            }
                        }
                        eb.AddField($"Roles [{j.Roles.Count -1}]", roles);
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                    catch (Exception)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.AddField("Exception details", $"**Can't find user with id {output} on the server.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                }
                else
                {
                    try
                    {
                        SocketGuildUser j = Context.Guild.Users.FirstOrDefault(x => x.Username == output);
                        if (j == null) { throw new Exception(); }
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"{j.Username}#{j.DiscriminatorValue}", j.GetAvatarUrl(), j.GetAvatarUrl(ImageFormat.Auto, 512));
                        eb.WithColor(40, 200, 150);
                        eb.WithThumbnailUrl(j.GetAvatarUrl());
                        eb.AddField("User ID", $"{j.Id}", true);
                        eb.AddField("Status", $"{j.Status}", true);
                        string nick = "";
                        if (j.Nickname == null) { nick = "None"; }
                        else { nick = j.Nickname; }
                        eb.AddField("Nickname", nick);
                        eb.AddField("Registered at", $"{j.CreatedAt.UtcDateTime}");
                        eb.AddField("Joined at", $"{j.JoinedAt.Value.UtcDateTime}");
                        string roles = "";
                        foreach(var role in j.Roles)
                        {
                            if (role.Name != "@everyone")
                            {
                                roles += role.Name;
                                roles += Environment.NewLine;
                            }                           
                        }
                        eb.AddField($"Roles [{j.Roles.Count -1}]", roles);
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                    catch (Exception)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.AddField("Exception details", $"**Can't find user with username \"{output}\" on the server.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                }
            }
            else
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"{Context.User.Username}#{Context.User.DiscriminatorValue}", Context.User.GetAvatarUrl(), Context.User.GetAvatarUrl(ImageFormat.Auto, 512));
                eb.WithColor(40, 200, 150);
                eb.WithThumbnailUrl(Context.User.GetAvatarUrl());
                eb.AddField("User ID", $"{Context.User.Id}",true);
                eb.AddField("Status", $"{Context.User.Status}",true);
                string nick = "";
                if (Context.Guild.Users.FirstOrDefault(x => x.Username == Context.User.Username).Nickname == null) { nick = "None"; }
                else { nick = Context.Guild.Users.FirstOrDefault(x => x.Username == Context.User.Username).Nickname; }
                eb.AddField("Nickname", nick);
                eb.AddField("Registered at", $"{Context.User.CreatedAt.UtcDateTime}");
                eb.AddField("Joined at", $"{Context.Guild.CurrentUser.JoinedAt.Value.UtcDateTime}");
                string roles = "";
                foreach (var role in Context.Guild.Users.FirstOrDefault(x => x.Username == Context.User.Username).Roles)
                {
                    if (role.Name != "@everyone")
                    {
                        roles += role.Name;
                        roles += Environment.NewLine;
                    }
                }
                eb.AddField($"Roles [{Context.Guild.CurrentUser.Roles.Count -1}]", roles);
                await Context.Channel.SendMessageAsync("", false, eb.Build());
                return;
            }
        }
        [Command("cat")]
        public async Task CatAPI()
        {
            WebRequest webRequest = WebRequest.Create("https://api.thecatapi.com/v1/images/search");
            Task<WebResponse> webResponse = webRequest.GetResponseAsync();
            using (StreamReader sr = new StreamReader(webResponse.Result.GetResponseStream()))
            {
                string json = await sr.ReadToEndAsync();
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithImageUrl(JArray.Parse(json).First().SelectToken("url").ToString());
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
        [Command("dog")]
        public async Task DogAPI()
        {
            WebRequest webRequest = WebRequest.Create("https://api.thedogapi.com/v1/images/search");
            Task<WebResponse> webResponse = webRequest.GetResponseAsync();
            using (StreamReader sr = new StreamReader(webResponse.Result.GetResponseStream()))
            {
                string json = await sr.ReadToEndAsync();
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithImageUrl(JArray.Parse(json).First().SelectToken("url").ToString());
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
        [Command("poll")]
        public async Task DuckyPoll(string theme, params String[] types)
        {
            if (Context.IsPrivate == true)
            {
                EmbedBuilder eb3 = new EmbedBuilder();
                eb3.WithAuthor("Error", Context.User.GetAvatarUrl());
                eb3.WithColor(40, 200, 150);
                eb3.WithDescription("You must use that command only in a server!");
                eb3.WithThumbnailUrl(Settings.MainThumbnailUrl);
                await Context.User.SendMessageAsync("", false, eb3.Build());
                return;
            }
            if (theme == "" || theme == null)
            {
                EmbedBuilder eb3 = new EmbedBuilder();
                eb3.WithAuthor("Error", Context.User.GetAvatarUrl());
                eb3.WithColor(40, 200, 150);
                eb3.WithDescription("You can't use empty theme for poll!");
                eb3.WithThumbnailUrl(Settings.MainThumbnailUrl);
                await Context.User.SendMessageAsync("", false, eb3.Build());
                return;
            }
            if (types.Length > 26)
            {
                EmbedBuilder eb3 = new EmbedBuilder();
                eb3.WithAuthor("Error", Context.User.GetAvatarUrl());
                eb3.WithColor(40, 200, 150);
                eb3.WithDescription("You can't use more than 26 choices!");
                eb3.WithThumbnailUrl(Settings.MainThumbnailUrl);
                await Context.User.SendMessageAsync("", false, eb3.Build());
                return;
            }
            if (types.Length <= 0)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithColor(40, 200, 150);
                eb.WithAuthor(theme);
                eb.WithThumbnailUrl(Context.Guild.IconUrl);
                var m = await Context.Channel.SendMessageAsync("", false, eb.Build());
                await m.AddReactionAsync(new Emoji("👍"));
                await m.AddReactionAsync(new Emoji("👎"));
                return;
            }
            EmbedBuilder eb1 = new EmbedBuilder();
            eb1.WithColor(40, 200, 150);
            eb1.WithAuthor(theme);
            eb1.WithThumbnailUrl(Context.Guild.IconUrl);
            for (int i = 0; i < types.Length; i++)
            {
                eb1.AddField($"{emoji[i]}", $"**{types[i]}**");
            }
            var msg = await Context.Channel.SendMessageAsync("", false, eb1.Build());
            for(int i = 0; i < types.Length; i++)
            {
                await msg.AddReactionAsync(new Emoji(emoji[i]));
            }
        }
        [Command("server")]
        public async Task ServerInfo(params String[] guildName)
        {
            if (Context.IsPrivate == true)
            {
                if (guildName.Length <= 0)
                {
                    EmbedBuilder eb1 = new EmbedBuilder();
                    eb1.WithAuthor("Error", Context.User.GetAvatarUrl());
                    eb1.WithColor(40, 200, 150);
                    eb1.WithDescription("**Please use this command only in a server to get it's info, or get an info about another server here.**");
                    eb1.WithThumbnailUrl(Settings.MainThumbnailUrl);
                    await Context.User.SendMessageAsync("", false, eb1.Build());
                    return;
                }
            }

            SocketGuild guild = Context.Guild;
            string output = new Addons()._out(guildName);

            if (guildName.Length >= 1 && guildName.ElementAt(0) == "roles")
            {
                output = output.Remove(0, 5);
                if (guildName.Length > 1)
                {
                    output = output.Remove(0, 1);
                    guild = Context.Client.Guilds.Where(x => x.Name == output).FirstOrDefault();
                }
                EmbedBuilder eb1 = new EmbedBuilder();
                eb1.WithAuthor($"{guild.Name} roles ({guild.Roles.Count})", guild.IconUrl);
                eb1.WithColor(40, 200, 150);
                string roles = "";
                for (int i = 0; i < guild.Roles.Count; i++)
                {
                    roles += guild.Roles.ElementAt(i);
                    roles += Environment.NewLine;
                }
                eb1.WithDescription(roles);
                await Context.Channel.SendMessageAsync("", false, eb1.Build());
                return;
            }

                if (output != null)
                {
                    guild = Initialization._client.Guilds.Where(x => x.Name == output).FirstOrDefault();
                } 
                if (guild == null)
                {
                    EmbedBuilder eb2 = new EmbedBuilder();
                    eb2.WithAuthor($"Error", Context.User.GetAvatarUrl());
                    eb2.WithColor(40, 200, 150);
                    eb2.WithDescription($":x: **Can't find guild with name: {output}**");
                    eb2.WithThumbnailUrl(Settings.MainThumbnailUrl);
                    await Context.Channel.SendMessageAsync("", false, eb2.Build());
                }
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"{guild.Name}", guild.IconUrl);
                eb.WithColor(40, 200, 150);
                eb.AddField($"ID: {guild.Id}", $"[Server avatar]({guild.IconUrl})");
                eb.AddField("Verification Level", $"{guild.VerificationLevel}", true);
                eb.AddField("Region",$"{guild.VoiceRegionId}",true);
                eb.AddField("Members", $"{guild.MemberCount} members",true);
                eb.AddField("Channels", $"{guild.Channels.Count}",true);
                eb.AddField("Server Owner", $"{guild.Owner.Username}#{guild.Owner.Discriminator} | ID: {guild.OwnerId}");
                eb.AddField("Created at",$"{guild.CreatedAt.UtcDateTime}");
                eb.AddField("Roles", $"{guild.Roles.Count} roles");
                eb.WithFooter("Use d!server roles to show all roles.", guild.IconUrl);
                eb.WithThumbnailUrl($"{guild.IconUrl}");
                await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("status"), Alias("stats")]
        public async Task status()
        {
            int usercount = 0;
            var g = Initialization._client.Guilds.GetEnumerator();
            g.MoveNext();
            for (int i = 0; i < Initialization._client.Guilds.Count; i++)
            {
                usercount = usercount + g.Current.Users.Count;
                g.MoveNext();
            }

            EmbedBuilder eb = new EmbedBuilder();
            eb.WithAuthor("Statistics", Context.User.GetAvatarUrl());
            eb.WithColor(40, 200, 150);
            eb.AddField("Status", $"**{Initialization._client.ConnectionState}**", true);
            eb.AddField("Gateway ping", $"**{Initialization._client.Latency} ms.**", true);
            eb.AddField("Guilds", $"**{Initialization._client.Guilds.Count}**", true);
            eb.AddField("Users", $"**{usercount}**", true);
            eb.AddField("Shards", $"**1**", true);
            eb.AddField("Users on shard", $"**{usercount}**", true);
            eb.WithFooter($"Information at: {DateTime.UtcNow} UTC");
            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("about")]
        public async Task about()
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithAuthor("About", Context.User.GetAvatarUrl());
            eb.WithColor(40, 200, 150);
            eb.WithDescription(
                "**Ducky Bot - is multifunctional bot made by Warriors of Duck Team.**" + Environment.NewLine +
                "**It has more commands and can replace another bots.**" + Environment.NewLine +
                "**Used C# and Discord.Net NuGet package for developing.**" + Environment.NewLine +
                "[WarDT discord server link](https://discordapp.com/invite/bQ7UgKZ)"
                );
            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
            await Context.User.SendMessageAsync("", false, eb.Build());
        }
        [Command("invite")]
        public async Task botlink()
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithAuthor("Invite bot", Context.User.GetAvatarUrl());
            eb.WithColor(40, 200, 150);
            eb.AddField("There is the bot invitation link", $"[Click to invite]({Settings.InviteLink})");
            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("avatar"), Alias("pp", "a")]
        public async Task getPP(params String[] UserL)
        {
            string output = new Addons()._out(UserL);
            if (output != "")
            {

                if (output.IndexOf('@') == 1)
                {
                    try
                    {
                        output = output.Remove(0, 2);
                        output = output.Remove(output.Length - 1, 1);
                        if (output.IndexOf('!') == 0)
                        {
                            output = output.Remove(0, 1);
                        }
                        SocketGuildUser j = null;
                        var n = Initialization._client.Guilds.GetEnumerator();
                        n.MoveNext();
                        var g = n.Current;
                        for (int i = 0; i < Initialization._client.Guilds.Count; i++)
                        {
                            g = n.Current;
                            n.MoveNext();
                            if (j == null)
                            {
                                j = g.Users.FirstOrDefault(x => x.Id == ulong.Parse(output));
                            }
                        }
                        if (j == null) { throw new Exception(); }
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl(), j.GetAvatarUrl(ImageFormat.Auto, 512));
                        eb.WithColor(40, 200, 150);
                        eb.AddField($"{j.Username}#{j.Discriminator}", $"[Avatar URL]({j.GetAvatarUrl(ImageFormat.Auto, 512)})", true);
                        eb.WithImageUrl(j.GetAvatarUrl());
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                    catch (Exception)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.AddField("Exception details", $"**Can't find user with id {output} in all connected to the bot servers.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                }
                else
                {
                    try
                    {
                        var n = Initialization._client.Guilds.GetEnumerator();
                        n.MoveNext();
                        var g = n.Current;
                        SocketGuildUser j = null;
                        for (int i = 0; i < Initialization._client.Guilds.Count; i++)
                        {
                            g = n.Current;
                            n.MoveNext();
                            if (j == null)
                            {
                                j = g.Users.FirstOrDefault(x => x.Username == output);
                            }
                        }
                        if (j == null) { throw new Exception(); }
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl(), j.GetAvatarUrl(ImageFormat.Auto, 512));
                        eb.WithColor(40, 200, 150);
                        eb.AddField($"{j.Username}#{j.Discriminator}", $"[Avatar URL]({j.GetAvatarUrl(ImageFormat.Auto, 512)})", true);
                        eb.WithImageUrl(j.GetAvatarUrl());
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                    catch (Exception)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.AddField("Exception details", $"**Can't find user with username \"{output}\" in all connected to the bot servers.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                }
            }
            else
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl(), Context.User.GetAvatarUrl());
                eb.WithColor(40, 200, 150);
                eb.AddField($"{Context.User.Username}#{Context.User.Discriminator}", $"[Avatar URL]({Context.User.GetAvatarUrl(ImageFormat.Auto, 512)})",true);
                eb.WithImageUrl(Context.User.GetAvatarUrl());
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
        [Command("echo"), Alias("e")]
        public async Task echo(params String[] stringArray)
        {
            var m = Context.Message;
            SocketGuildUser n = Context.Guild.GetUser(Context.User.Id);
            var j = n.Guild.GetChannel(Context.Channel.Id).GetUser(Context.User.Id);
            if (j.GetPermissions(n.Guild.GetChannel(Context.Channel.Id)).ManageMessages == false)
            {
                if (n.GuildPermissions.ManageMessages == false)
                {
                    var h = await Context.Channel.SendMessageAsync($"**{n.Mention} you don't have \"Manage Messages\" permission to use that command!**");
                    await Task.Delay(4000);
                    await h.DeleteAsync();
                    return;
                }
            }
            if (stringArray.ElementAt(0) != "")
            {
                string output = "";
                for (int i = 0; i < stringArray.Length; i++)
                {
                    output += stringArray.ElementAt(i);
                    output += " ";
                }
                await m.DeleteAsync();
                await Context.Channel.SendMessageAsync(output);
            }
        }
        [Command("dice"), Alias("roll")]
        public async Task dice(int d = 0)
        {
            if (d == 0) { d = 100; }
            if (d > 10000) { d = 10000; }
            Random rnd = new Random();
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithColor(40, 200, 150);
            eb.WithAuthor($"{Context.User.Username} rolled a dice!");
            eb.WithDescription($":diamonds: **{Context.User.Username} rolled a d{d} dice and got {rnd.Next(1, d+1)}!**");
            eb.WithThumbnailUrl(Context.User.GetAvatarUrl());
            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }
        [Command("report")]
        public async Task report(params string[] text)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithAuthor($"{Context.User.Username}#{Context.User.DiscriminatorValue}",null, Context.User.GetAvatarUrl(ImageFormat.Auto, 512));
            eb.WithColor(40, 200, 150);
            eb.WithDescription(new Addons()._out(text));
            eb.WithThumbnailUrl(Context.User.GetAvatarUrl(ImageFormat.Auto, 512));
            eb.WithFooter($"Reported at {Context.Message.Timestamp.UtcDateTime} UTC");
            var channel = (ISocketMessageChannel) Initialization._client.GetChannel(Settings.ReportChannelID);
            if (channel == null) { await Context.Channel.SendMessageAsync(":x: **Report isn't avaliable right now. Please try again later.**"); return; }
            await channel.SendMessageAsync("", false, eb.Build());
            await Context.Channel.SendMessageAsync(":white_check_mark: **Reported!**");
        }
    }
}

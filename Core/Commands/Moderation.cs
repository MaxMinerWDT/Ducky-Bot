using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Linq;

namespace Duck_Bot_.Net_Core.Core.Commands
{
    public class Moderation : ModuleBase<SocketCommandContext>
    {
        [Command("clear")]
        public async Task clear(int arg)
        {
            if (Context.Guild.Users.FirstOrDefault(x => x.Username == Context.User.Username).GuildPermissions.ManageMessages == true)
            {
                if (arg > 100)
                {
                    var m1 = await Context.Channel.SendMessageAsync("**:x: Number of messages can't be higher than 100**");
                    await Task.Delay(2000);
                    await m1.DeleteAsync();
                    return;
                }
                var items = await Context.Channel.GetMessagesAsync(++arg, CacheMode.AllowDownload).FlattenAsync();
                await ((ITextChannel)Context.Channel).DeleteMessagesAsync(items);
                var m = await ReplyAsync($":white_check_mark: **Succesfully deleted {--arg} messages**");
                await Task.Delay(2000);
                await m.DeleteAsync();
            }
            else
            {
                await Context.Channel.SendMessageAsync("**You don't have \"Manage Messages\" permission to use this command!**");
            }
        }
        [Command("ban")]
        public async Task ban(string reason, params String[] UserL)
        {
            var user = Context.Guild.Users.FirstOrDefault(x => x.Username == Context.User.Username);
            if (user.GuildPermissions.BanMembers == true)
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
                            SocketGuildUser j = Context.Guild.Users.FirstOrDefault(x => x.Id == ulong.Parse(output));
                            if (j == null) { throw new Exception(); }
                            if (j.Username == "Ducky Bot" && j.DiscriminatorValue == 5489 && j.IsBot == true)
                            {
                                await Context.Channel.SendMessageAsync("You can't ban Ducky Bot!");
                                return;
                            }
                            if (j == user)
                            {
                                await Context.Channel.SendMessageAsync("You can't ban yourself!");
                                return;
                            }
                            if (user.Hierarchy < j.Hierarchy)
                            {
                                await Context.Channel.SendMessageAsync("Your role is under the user role!");
                                return;
                            }
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Successful");
                            eb.WithColor(40, 200, 150);
                            eb.WithThumbnailUrl(j.GetAvatarUrl());
                            eb.WithDescription($"**{j.Username} has been banned**");
                            eb.AddField("Reason", reason);
                            eb.AddField("Banned by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}");
                            await Context.Guild.AddBanAsync(j, 0, reason);
                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                        catch (Discord.Net.HttpException)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb.WithColor(40, 200, 150);
                            eb.AddField("Exception details", $"**Bot role is under the user role**");
                            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                        catch (Exception)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb.WithColor(40, 200, 150);
                            eb.AddField("Exception details", $"**Can't find user with id {output} in the server.**");
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
                            if (user.Hierarchy < j.Hierarchy)
                            {
                                await Context.Channel.SendMessageAsync("Your role is under the user role!");
                                return;
                            }
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Successful");
                            eb.WithColor(40, 200, 150);
                            eb.WithThumbnailUrl(j.GetAvatarUrl());
                            eb.WithDescription($"**{j.Username} has been banned**");
                            eb.AddField("Reason", reason);
                            eb.AddField("Banned by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}");
                            await Context.Guild.AddBanAsync(j, 0, reason);
                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                        catch (Discord.Net.HttpException)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb.WithColor(40, 200, 150);
                            eb.AddField("Exception details", $"**Bot role is under the user role**");
                            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                        catch (Exception)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb.WithColor(40, 200, 150);
                            eb.AddField("Exception details", $"**Can't find user with username \"{output}\" in the server.**");
                            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                    }
                }
                else
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                    eb.WithColor(40, 200, 150);
                    eb.WithDescription("**Please use correct reason and username**");
                    eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("You don't have \"Ban Members\" permission to use this command!");
            }
        }
        [Command("kick")]
        public async Task kick(string reason, params String[] UserL)
        {
            var user = Context.Guild.Users.FirstOrDefault(x => x.Username == Context.User.Username);
            if (user.GuildPermissions.KickMembers == true)
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
                            SocketGuildUser j = Context.Guild.Users.FirstOrDefault(x => x.Id == ulong.Parse(output));
                            if (j == null) { throw new Exception(); }
                            if (j.Username == "Ducky Bot" && j.DiscriminatorValue == 5489 && j.IsBot == true)
                            {
                                await Context.Channel.SendMessageAsync("You can't kick Ducky Bot!");
                                return;
                            }
                            if (j == user)
                            {
                                await Context.Channel.SendMessageAsync("You can't kick yourself!");
                                return;
                            }
                            if (user.Hierarchy < j.Hierarchy)
                            {
                                await Context.Channel.SendMessageAsync("Your role is under the user role!");
                                return;
                            }
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Successful");
                            eb.WithColor(40, 200, 150);
                            eb.WithThumbnailUrl(j.GetAvatarUrl());
                            eb.WithDescription($"**{j.Username} has been kicked**");
                            eb.AddField("Reason", reason);
                            eb.AddField("Kicked by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}");
                            await j.KickAsync(reason);
                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                        catch (Discord.Net.HttpException)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb.WithColor(40, 200, 150);
                            eb.AddField("Exception details", $"**Bot role is under the user role**");
                            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                        catch (Exception)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb.WithColor(40, 200, 150);
                            eb.AddField("Exception details", $"**Can't find user with id {output} in the server.**");
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
                            if (user.Hierarchy < j.Hierarchy)
                            {
                                await Context.Channel.SendMessageAsync("Your role is under the user role!");
                                return;
                            }
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Successful");
                            eb.WithColor(40, 200, 150);
                            eb.WithThumbnailUrl(j.GetAvatarUrl());
                            eb.WithDescription($"**{j.Username} has been kicked**");
                            eb.AddField("Reason", reason);
                            eb.AddField("Kicked by", $"{Context.User.Username}#{Context.User.DiscriminatorValue}");
                            await j.KickAsync(reason);

                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                        catch (Discord.Net.HttpException)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb.WithColor(40, 200, 150);
                            eb.AddField("Exception details", $"**Bot role is under the user role**");
                            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                        catch (Exception)
                        {
                            EmbedBuilder eb = new EmbedBuilder();
                            eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb.WithColor(40, 200, 150);
                            eb.AddField("Exception details", $"**Can't find user with username \"{output}\" in the server.**");
                            eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb.Build());
                            return;
                        }
                    }
                }
                else
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                    eb.WithColor(40, 200, 150);
                    eb.WithDescription("**Please use correct reason and username**");
                    eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("You don't have \"Kick Members\" permission to use this command!");
            }       
        }
        [Command("mute")]
        public async Task mute(string reason, params String[] UserL)
        {

        }
    }    
}

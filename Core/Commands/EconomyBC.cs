using System;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using Discord.WebSocket;
using Duck_Bot_.Net_Core.Core.Data;
using System.Linq;

namespace Duck_Bot_.Net_Core.Core.Commands
{
    public class EconomyBC : ModuleBase<SocketCommandContext>
    {
        [Command("daily")]
        public async Task DailyCoins()
        {
            if (Context.Message.Timestamp.UtcDateTime.Day.ToString() != await DBData.getDailyTimestamp(Context.User.Id))
            {
                await DBData.saveCoins(Context.User.Id, 1000);
                await DBData.saveTimeStamp(Context.User.Id);
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithColor(40, 200, 150);
                eb.WithThumbnailUrl(Context.User.GetAvatarUrl());
                eb.WithAuthor("Successful");
                eb.WithDescription("1000 daily coins had successfully given!" + Environment.NewLine + $"Your balance is now: {DBData.getCoins(Context.User.Id)} Ducky Coins");
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
            else
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithColor(40, 200, 150);
                eb.WithThumbnailUrl(Context.User.GetAvatarUrl());
                eb.WithAuthor("Error");
                eb.WithDescription("You must wait 1 day to use that command next time!");
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
        [Command("balance"), Alias("coins", "money")]
        public async Task dCoinsShow(params String[] UserL)
        {
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
                        eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription($"**{j.Username}'s balance is: **" + DBData.getCoins(j.Id));
                        eb.WithThumbnailUrl(j.GetAvatarUrl());
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                    catch (Exception)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription($"**Can't find user with id {output} in all connected to the bot servers.**");
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
                        eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription($"**{j.Username}'s balance is: **" + DBData.getCoins(j.Id));
                        eb.WithThumbnailUrl(j.GetAvatarUrl());
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                    catch (Exception)
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Error", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription($"**Can't find user with username \"{output}\" in all connected to the bot servers.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                }
            }
            else
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl());
                eb.WithColor(40, 200, 150);
                eb.WithDescription("**Your balance is:** " + DBData.getCoins(Context.User.Id));
                eb.WithThumbnailUrl(Context.User.GetAvatarUrl());
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
        [Command("give"), Alias("pay")]
        public async Task giveDCoins(int amount = 0, params String[] UserL)
        {
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
                        if (j.IsBot)
                        {
                            EmbedBuilder eb1 = new EmbedBuilder();
                            eb1.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb1.WithColor(40, 200, 150);
                            eb1.WithDescription($"**Can't give {amount} Ducky Coins to {j.Username}, cause it's a bot!**");
                            eb1.WithThumbnailUrl(j.GetAvatarUrl());
                            await Context.Channel.SendMessageAsync("", false, eb1.Build());
                            return;
                        }
                        if (DBData.getCoins(Context.User.Id) < amount)
                        {
                            EmbedBuilder eb2 = new EmbedBuilder();
                            eb2.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb2.WithColor(40, 200, 150);
                            eb2.WithDescription($"**You don't have {amount} Ducky Coins to give them**");
                            eb2.WithThumbnailUrl(j.GetAvatarUrl());
                            await Context.Channel.SendMessageAsync("", false, eb2.Build());
                            return;
                        }
                        await DBData.saveCoins(j.Id, amount);
                        await DBData.removeCoins(Context.User.Id, amount);
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription($"**Succesfully gave {amount} Ducky Coins to {j.Username}**");
                        eb.WithThumbnailUrl(j.GetAvatarUrl());
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                    catch (Exception)
                    {
                        if (amount >= 0)
                        {
                            EmbedBuilder eb1 = new EmbedBuilder();
                            eb1.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb1.WithColor(40, 200, 150);
                            eb1.WithDescription($"**Can't find user with id {output} in all connected to the bot servers.**");
                            eb1.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb1.Build());
                        }
                        else
                        {
                            EmbedBuilder eb1 = new EmbedBuilder();
                            eb1.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb1.WithColor(40, 200, 150);
                            eb1.WithDescription($"**Please type right amount of Ducky Coins to give them.**");
                            eb1.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb1.Build());
                        }
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
                        if (j.IsBot)
                        {
                            EmbedBuilder eb1 = new EmbedBuilder();
                            eb1.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb1.WithColor(40, 200, 150);
                            eb1.WithDescription($"**Can't give {amount} Ducky Coins to {j.Username}, cause it is a bot!**");
                            eb1.WithThumbnailUrl(j.GetAvatarUrl());
                            await Context.Channel.SendMessageAsync("", false, eb1.Build());
                            return;
                        }
                        if (DBData.getCoins(Context.User.Id) < amount)
                        {
                            EmbedBuilder eb1 = new EmbedBuilder();
                            eb1.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb1.WithColor(40, 200, 150);
                            eb1.WithDescription($"**You don't have {amount} Ducky Coins to give them**");
                            eb1.WithThumbnailUrl(j.GetAvatarUrl());
                            await Context.Channel.SendMessageAsync("", false, eb1.Build());
                            return;
                        }
                        await DBData.saveCoins(j.Id, amount);
                        await DBData.removeCoins(Context.User.Id, amount);
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor($"Succesfull", Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription($"**Succesfully gave {amount} Ducky Coins to {j.Username}**");
                        eb.WithThumbnailUrl(j.GetAvatarUrl());
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                    catch (Exception)
                    {
                        if (amount >= 0)
                        {
                            EmbedBuilder eb1 = new EmbedBuilder();
                            eb1.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb1.WithColor(40, 200, 150);
                            eb1.WithDescription($"**Can't find user with id {output} in all connected to the bot servers.**");
                            eb1.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb1.Build());
                        }
                        else
                        {
                            EmbedBuilder eb1 = new EmbedBuilder();
                            eb1.WithAuthor($"Error", Context.User.GetAvatarUrl());
                            eb1.WithColor(40, 200, 150);
                            eb1.WithDescription($"**Please type right amount of Ducky Coins to give them.**");
                            eb1.WithThumbnailUrl(Settings.MainThumbnailUrl);
                            await Context.Channel.SendMessageAsync("", false, eb1.Build());
                        }
                        return;
                    }


                }

            }
        }
        [Command("slot"), Alias("slots")]
        public async Task slots(int sum)
        {
            
        }          
    }
}

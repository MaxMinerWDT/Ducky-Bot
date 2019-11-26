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
using Duck_Bot_.Net_Core.Core.Resources.Database;
using Duck_Bot_.Net_Core.Resources.Database;


namespace Duck_Bot_.Net_Core.Core.Commands
{
    public class Autorole : ModuleBase<SocketCommandContext>
    {
        [Command("autorole"), Alias("aa")]
        public async Task autorole(string command, params String[] roles)
        {
            if (Context.IsPrivate) { await Context.Channel.SendMessageAsync(":x: **You must in a guild to use that command!**"); return; }
            if (!(Context.User as IGuildUser).GuildPermissions.Administrator) { await Context.Channel.SendMessageAsync(":x: **You must be an administrator to use that command!**"); return; }
            switch (command)
            {
                case "enable":
                case "on":
                    {
                        try
                        {
                            ServerDB server = await DBServerData.getStats(Context.Guild.Id);
                            server.AutoroleSwitch = true;
                            await DBServerData.saveStats(server);
                            await Context.Channel.SendMessageAsync(":white_check_mark: **Autoroles was activated successfully.**");
                        }
                        catch (Exception)
                        {
                            await Context.Channel.SendMessageAsync(":x: **Unknown error occured. Please report to the bot creators.**");
                        }
                        break;
                    }
                case "disable":
                case "off":
                    {
                        try
                        {
                            ServerDB server = await DBServerData.getStats(Context.Guild.Id);
                            server.AutoroleSwitch = false;
                            await DBServerData.saveStats(server);
                            await Context.Channel.SendMessageAsync(":white_check_mark: **Autoroles was disabled successfully.**");
                        }
                        catch (Exception)
                        {
                            await Context.Channel.SendMessageAsync(":x: **Unknown error occured. Please report to bot creators.**");
                        }
                        break;
                    }
                case "add":
                    {
                        try
                        {
                            ServerDB server = await DBServerData.getStats(Context.Guild.Id);
                            server.Autorole += new Addons()._outRoles(roles);
                            await DBServerData.saveStats(server);
                            await Context.Channel.SendMessageAsync("Role/s was added successfully.");
                        }
                        catch (Exception)
                        {
                            await Context.Channel.SendMessageAsync(":x: **Unknown error occured. Please report to bot creators.**");
                        }
                        break;
                    }
                case "remove":
                    {
                        try
                        {
                            ServerDB server = await DBServerData.getStats(Context.Guild.Id);
                            string[] autorole = server.Autorole.Split(',');
                            var tmp = new List<string>(autorole);
                            foreach (string role in autorole)
                            {
                                foreach (string role2 in roles)
                                {
                                    if (role == role2)
                                    {
                                        tmp.Remove(role);
                                    }
                                }
                            }
                            server.Autorole = new Addons()._outRoles(tmp.ToArray());
                            await DBServerData.saveStats(server);
                            await Context.Channel.SendMessageAsync($"Removed {new Addons()._out(autorole)} from autorole successfully.");
                        }
                        catch (Exception)
                        {
                            await Context.Channel.SendMessageAsync(":x: **Unknown error occured. Please report to bot creators.**");
                        }
                        break;
                    }
                case "clear":
                    {
                        ServerDB server = await DBServerData.getStats(Context.Guild.Id);
                        server.Autorole = "";
                        await DBServerData.saveStats(server);
                        await Context.Channel.SendMessageAsync($"Successfully cleared autoroles list.");
                        break;
                    }
                default:
                    {
                        await Context.Channel.SendMessageAsync(":x: **You can't use that command without arguments.**" + Environment.NewLine +
                            "**To see the command information type d!help autorole**");
                        break;
                    }

            }
        }
        [Command("wm")]
        public async Task Welcome(string _switch, ulong channelId, params string[] message)
        {
            if (Context.IsPrivate) { await Context.Channel.SendMessageAsync(":x: **You must in a guild to use that command!**"); return; }
            if (!(Context.User as IGuildUser).GuildPermissions.Administrator) { await Context.Channel.SendMessageAsync(":x: **You must be an administrator to use that command!**"); return; }
            switch (_switch.ToLower())
            {
                case "on":
                    {
                        ServerDB server = await DBServerData.getStats(Context.Guild.Id);
                        string _message = new Addons()._out(message);
                        server.Welcome = _message;
                        server.WelcomeChannel = channelId;
                        await DBServerData.saveStats(server);
                        await Context.Channel.SendMessageAsync($"Welcome message has been successfully turned on.");
                        break;
                    }

                case "off":
                    {
                        ServerDB server = await DBServerData.getStats(Context.Guild.Id);
                        server.WelcomeChannel = 0;
                        server.Welcome = "";
                        await DBServerData.saveStats(server);
                        await Context.Channel.SendMessageAsync($"Welcome message has been successfully turned off.");
                        return;
                    }
                default:
                    {
                        await Context.Channel.SendMessageAsync(":x: **You can't use that command without arguments.**" + Environment.NewLine +
                        "**To see the command information type d!help wm**");
                        break;
                    }
            }           
        }        
    }
}

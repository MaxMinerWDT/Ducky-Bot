using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Duck_Bot_.Net_Core.Core.Commands
{
    public class Help : ModuleBase<SocketCommandContext> //Make next classes PUBLIC
    {
        const string l = "About command";
        [Command("help")]
        public async Task help(string mod = "")
        {
            if (mod == "")
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor("There are modules with commands which you can use", Context.User.GetAvatarUrl());
                eb.WithColor(40, 200, 150);
                eb.WithFooter("Type d!help [Command] for more info");
                eb.WithDescription(
                    "🛠**Main -** __**status, about, invite, report, server, user, avatar**__" + Environment.NewLine +
                    "💎**Economy -** __**balance, pay, daily**__" + Environment.NewLine +
                    "🔒**Moderation -** __**ban, kick, clear**__" + Environment.NewLine +
                    "🏷**Autorole -** __**autorole**__" + Environment.NewLine +
                    "🎉**Other -** __**echo, poll, weather, dice, youtube, wiki, cat, dog,  catfact, dogfact**__" + Environment.NewLine +
                    "🎶**Music -** __**join, leave, play, scplay, np, volume, pause, resume, seek, playskip, queue**__"+ Environment.NewLine +
                    "🕹**Game Stats -** __**owstats**__"+ Environment.NewLine+
                    "🛑**Nsfw -** __**Under Construction**__"
                    );
                eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                await Context.User.SendMessageAsync("", false, eb.Build());
                return;
            }

            switch (mod.ToLower())
            {
                case "status":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**status**__ **- A status command.**" + Environment.NewLine +
                            "**Get Ducky Bot's statistics (status, ping, guilds, etc.)**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                case "about":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**about**__ **- An about command.**" + Environment.NewLine +
                            "**Get the information about Ducky Bot.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                case "invite":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**invite**__ **- An invite command.**" + Environment.NewLine +
                            "**Get Ducky Bot's invitation link.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                case "clear":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**clear [amount]**__ **- A moderation command.**" + Environment.NewLine +
                            "**Clear specified amount of messages (max 100).**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                case "avatar":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**avatar/pp/a [username or mention]**__ **- A profile picture command.**" + Environment.NewLine +
                            "**Get user profile picture by username or mention.**" + Environment.NewLine +
                            "**You can get profile picture of any user who connected to servers where Ducky Bot is.**" + Environment.NewLine +
                            "**Type the command without arguments to get your profile picture.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                case "echo":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**echo/e [text]**__ **- An echo command.**" + Environment.NewLine +
                            "**Send a message by using Ducky Bot and delete your message.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                case "weather":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**weather/w [city] [temp type]**__ **- A weather command.**" + Environment.NewLine +
                            "**Get a weather of the city with changable metrics type.**" + Environment.NewLine +
                            "**Use F or C or K char in [temp type] to change it.**" + Environment.NewLine +
                            "**F - Fahrenheit, C - Celsius, K - Kelvin.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        return;
                    }
                case "server":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**server [name]**__ **- A server command.**" + Environment.NewLine +
                            "**Get the server information.**" + Environment.NewLine +
                            "**Type [name] to get information about another connected to the bot server.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "balance":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**balance/coins/money [Username]**__ **- A balance command.**" + Environment.NewLine +
                            "**Get the user balance (amount of Ducky Coins).**" + Environment.NewLine +
                            "**Type the command without arguments to get your balance.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "pay":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**pay/give [Amount] [Username]**__ **- A pay command.**" + Environment.NewLine +
                            "**Transfer a specified amount of Ducky Coins to the user.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "poll":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**poll [\"Theme\"] [\"Choice\"] [\"Choice 2\"] etc.**__ **- A poll command.**" + Environment.NewLine +
                            "**Create a simple (Yes/No) poll (if no choices).**" + Environment.NewLine +
                            "**Or create multi-poll with more choices (max 26).**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "dice":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**dice/roll [Max number]**__ **- A dice command.**" + Environment.NewLine +
                            "**Roll a dice with [Max number] of the dice.**" + Environment.NewLine +
                            "**Leave the [Max number] empty to roll a d100 dice.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "cat":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**cat**__ **- A cat picture command.**" + Environment.NewLine +
                            "**Get a random cat picture.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "dog":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**dog**__ **- A dog picture command.**" + Environment.NewLine +
                            "**Get a random dog picture.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "daily":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**daily**__ **- A daily command.**" + Environment.NewLine +
                            "**Claim 1000 daily coins.**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "owstats":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription(
                            "__**owstats \"[Battle.Net Username#Tag]\" [Region] [Platform]**__ **- An Overwatch statistics command.**" + Environment.NewLine +
                            "**Get user's Overwatch stats.**" + Environment.NewLine +
                            "**[Region] - eu, us, asia.**" + Environment.NewLine +
                            "**[Platform] - pc (only).**"
                            );
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "user":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**user/info [Username or mention]**__ **- An user information command.**" + Environment.NewLine + 
                        "**Get an information of specified user on the guild.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "ban":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**ban \"[Reason]\" [Username or mention]**__ **- A ban user command.**" + Environment.NewLine + 
                            "**Ban a specified user in the guild.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "kick":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**kick \"[Reason]\" [Username or mention]**__ **- A kick user command.**" + Environment.NewLine + 
                            "**Kick a specified user from the guild.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "youtube":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**yt/youtube [Name/Text]**__ **- An youtube search command.**" + Environment.NewLine + 
                            "**Search an youtube video by the name.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "join":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**join**__ **- A join command.**" + Environment.NewLine + 
                            "**Join bot to the voice channel.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "leave":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**leave**__ **- A leave command.**" + Environment.NewLine + 
                            "**Leave bot from the voice channel.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "wiki":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**wikipedia/wiki [Name]**__ **- A wikipedia command.**" + Environment.NewLine +
                            "**Search an article in the Wikipedia.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "catfact":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**catfact**__ **- A dog fact command.**" + Environment.NewLine +
                            "**Get a random cat fact.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "dogfact":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**dogfact**__ **- A dog fact command.**" + Environment.NewLine +
                            "**Get a random dog fact.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "autorole":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**autorole/aa [Command] [Params]**__ **- An autoroles manager command.**" + Environment.NewLine +
                            "**enable/on - Enable autoroles, command without params.**" + Environment.NewLine +
                            "**disable/off - Disable autoroles, command without params.**" + Environment.NewLine +
                            "**add - Add roles to autoroles list. Put roles ID in [Params] using 'space' for splitting.**" + Environment.NewLine +
                            "**remove - Remove roles from autoroles list. Put roles ID in [Params] using 'space' for splitting.**" + Environment.NewLine +
                            "**clear - Clear autorole list for current server.**");
                         eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "report":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**report [Issue]**__ **- A report command.**" + Environment.NewLine +
                            "**Report an issue to the bot creators.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "play":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**play [Url/Search queue]**__ **- A play command.**" + Environment.NewLine +
                            "**Play a video or playlist from YouTube.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "scplay":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**scplay [Url/Search queue]**__ **- A scplay command.**" + Environment.NewLine +
                            "**Play a song or playlist from SoundCloud.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "np":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**np/nowplaying**__ **- A nowplaying command.**" + Environment.NewLine +
                            "**Get current playing video/music info.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "volume":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**vol/volume [0-100]**__ **- A volume command.**" + Environment.NewLine +
                            "**Change music volume (0-100).**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "pause":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**pause**__ **- A pause command.**" + Environment.NewLine +
                            "**Pause current playing video/music.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "resume":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**resume**__ **- A pause command.**" + Environment.NewLine +
                            "**Pause current playing video/music.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "seek":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**seek [Seconds]**__ **- A seek command.**" + Environment.NewLine +
                            "**Seek current playing video/music to specified position (in seconds).**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "queue":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**queue/ql**__ **- A queue command.**" + Environment.NewLine +
                            "**Get current videos/music in queue.**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "playskip":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("__**playskip/ps [Url/Search queue]**__ **- A playskip command.**" + Environment.NewLine +
                            "**Skip current video/music and play another one.**" + Environment.NewLine +
                            "**Only for server administrators!**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
                case "wm":
                    {
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithAuthor(l, Context.User.GetAvatarUrl());
                        eb.WithColor(40, 200, 150);
                        eb.WithDescription("**Only for server administrators!**");
                        eb.WithThumbnailUrl(Settings.MainThumbnailUrl);
                        await Context.User.SendMessageAsync("", false, eb.Build());
                        break;
                    }
            }
        }
    }
}

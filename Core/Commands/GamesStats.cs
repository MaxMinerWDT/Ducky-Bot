using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using Discord;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Duck_Bot_.Net_Core.Core.Commands
{
    public class GamesStats : ModuleBase<SocketCommandContext>
    {
        [Command("owstats")]
        public async Task owStats(string nick, string reg, string platform)
        {
            try
            {
                string url = $"https://ow-api.com/v1/stats/{platform}/{reg}/{nick}/profile";
                WebRequest request = WebRequest.Create(url);
                Task<WebResponse> response = request.GetResponseAsync();
                using (StreamReader sr = new StreamReader(response.Result.GetResponseStream()))
                {
                    string json = await sr.ReadToEndAsync();
                    JObject owStat = JObject.Parse(json);
                        EmbedBuilder eb = new EmbedBuilder();
                        eb.WithColor(40, 200, 150);
                        eb.WithThumbnailUrl($"{owStat.SelectToken("icon")}");
                        eb.WithAuthor($"Overwatch statistics for {owStat.SelectToken("name")}");
                        eb.AddField("Name", $"**{owStat.SelectToken("name")}**");
                        eb.AddField("Prestige", $"**{owStat.SelectToken("prestige")}**", true);
                        eb.AddField("Level", $"**{owStat.SelectToken("level")}**",true);
                        if (Boolean.Parse(owStat.SelectToken("private").ToString()) == true) { eb.AddField("Private", $"**{owStat.SelectToken("private")}. This profile is private.**"); await Context.Channel.SendMessageAsync("", false, eb.Build()); return; }
                        eb.AddField("Total games won", $"{owStat.SelectToken("gamesWon")}",true);
                        eb.AddField("Rating", $"**{owStat.SelectToken("rating")}**",true);
                        eb.AddField("Quick play games won", $"{owStat.SelectToken("quickPlayStats.games.won")}",true);
                    eb.AddField("Competitive games won", $"{owStat.SelectToken("competitiveStats.games.won")}",true);
                    eb.WithImageUrl(owStat.SelectToken("ratingIcon").ToString());
                        await Context.Channel.SendMessageAsync("", false, eb.Build());
                }

            } catch (Exception e)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithColor(40, 200, 150);
                eb.WithAuthor("Error");
                eb.WithDescription("Please use valid data to get Overwatch stats");
                eb.WithThumbnailUrl(Context.User.GetAvatarUrl());
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
        }
    }
}

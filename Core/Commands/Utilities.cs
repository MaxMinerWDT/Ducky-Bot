using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Discord;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WikipediaNet;
using WikipediaNet.Enums;
using WikipediaNet.Misc;
using WikipediaNet.Objects;

namespace Duck_Bot_.Net_Core.Core.Commands
{
    public class Utilities : ModuleBase<SocketCommandContext>
    {
        public async Task fact(string animal)
        {
            WebRequest webReq = WebRequest.Create($"https://cat-fact.herokuapp.com/facts/random?animal_type={animal}&amount=1");
            Task<WebResponse> webRes = webReq.GetResponseAsync();
            using (StreamReader sr = new StreamReader(webRes.Result.GetResponseStream()))
            {
                string json = await sr.ReadToEndAsync();
                await Context.Channel.SendMessageAsync($"**{JObject.Parse(json).SelectToken("text")}**");
            }
        }
        [Command("wiki"), Alias("wikipedia")]
        public async Task wiki(params String[] name)
        {
            string output = new Addons()._out(name);
            Wikipedia w = new Wikipedia();
            w.UseTLS = true;
            w.Limit = 1;
            w.What = What.Text;
            QueryResult response = w.Search(output);
            if (response.Search[0].Url.ToString() != "https://en.wikipedia.org/wiki/")
            {
                await Context.Channel.SendMessageAsync(response.Search[0].Url.ToString().Replace(' ', '_'));
            }
            else{ await Context.Channel.SendMessageAsync(":x: **No articles were found.**"); }
        }

        [Command("catfact")]
        public async Task catfacts()
        {
            await fact("cat");
        }

        [Command("dogfact")]
        public async Task dogfact()
        {
            await fact("dog");
        }
        [Command("coinflip")]
        public async Task coinflip()
        {
            Random r = new Random();
            int coin = r.Next(0, 2);           
            if (coin == 0)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Coinflip Results", Context.User.GetAvatarUrl());
                eb.WithColor(40, 200, 150);
                eb.WithDescription($"{Context.User.Username} tossed a coin and got **TAILS**");
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }
            else
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithAuthor($"Coinflip Results", Context.User.GetAvatarUrl());
                eb.WithColor(40, 200, 150);
                eb.WithDescription($"{Context.User.Username} tossed a coin and got **HEADS**");
                await Context.Channel.SendMessageAsync("", false, eb.Build());
            }


        }
    }
}

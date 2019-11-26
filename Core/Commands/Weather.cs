using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;
using System.Net;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Duck_Bot_.Net_Core.Core.Resources.Datatypes;
using Duck_Bot_.Net_Core.Core;
using Discord;
using Newtonsoft.Json.Linq;

namespace Duck_Bot_.Net_Core.Core.Data
{
    public class Weather : ModuleBase<SocketCommandContext>
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        [Command("Get weather"), Alias("weather", "w"), Summary("A get weather command")]
        public async Task getWeather(string city, char degrees = ' ')
        {
            string degreesFormat = "units=metric";
            string url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&{degreesFormat}&appid={Settings.WeatherApiKey}";
            if (degrees != ' ')
            {
                switch (degrees)
                {
                    default:
                    case 'c':
                    case 'C':
                        {
                            degreesFormat = "units=metric";
                            url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&{degreesFormat}&appid={Settings.WeatherApiKey}";
                            break;
                        }
                    case 'F':
                    case 'f':
                        {
                            degreesFormat = "units=imperial";
                            url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&{degreesFormat}&appid={Settings.WeatherApiKey}";
                            break;
                        }
                    case 'K':
                    case 'k':
                        {
                            url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={Settings.WeatherApiKey}";
                            break;
                        }
                }
            }
            try
            {
                WebRequest request = WebRequest.Create(url);
                Task<WebResponse> response = request.GetResponseAsync();
                using (StreamReader sr = new StreamReader(response.Result.GetResponseStream()))
                {
                    string json = await sr.ReadToEndAsync();
                    WeatherMain weather = JsonConvert.DeserializeObject<WeatherMain>(json);
                    JObject weather2 = JObject.Parse(json);
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.AddField($"Weather in {weather.Name}, {weather.Sys.Country}", $"**State: {weather2.SelectToken("weather[0].description")}**");
                    if (degrees == ' ') { degrees = 'C'; }
                    eb.AddField("Temperature", $"**{weather.Main.Temp}°{degrees.ToString().ToUpper()}**", true);
                    eb.AddField("Humidity", $"**{weather.Main.Humidity}%**", true);
                    eb.AddField("Pressure", $"**{weather.Main.Pressure} hPa**", true);
                    eb.AddField("Wind speed", $"**{weather.Wind.Speed} m/s**", true);
                    eb.WithFooter($"Last update: {UnixTimeStampToDateTime(weather.Dt)} UTC");
                    eb.WithThumbnailUrl($"http://openweathermap.org/img/w/{(string)weather2.SelectToken("weather[0].icon")}.png");
                    eb.WithColor(40, 200, 150);
                    await Context.Channel.SendMessageAsync("", false, eb.Build());
                }
            }
            catch(JsonReaderException)
            {
                await Context.Channel.SendMessageAsync(":x: **Data of this city is unaviable. Please try again later.**");
                return;
            }
            catch (AggregateException)
            {
                await Context.Channel.SendMessageAsync(":x: **City name is incorrect! Please use a right one.**");
                return;
            }
        }

    }

}

using System;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Newtonsoft.Json;
using Duck_Bot_.Net_Core.Core.Resources.Datatypes;
using Duck_Bot_.Net_Core.Core;
using Lavalink4NET;
using System.Linq;
using Duck_Bot_.Net_Core.Resources.Database;
using Duck_Bot_.Net_Core.Core.Resources.Database;
using Lavalink4NET.DiscordNet;

namespace Duck_Bot_.Net_Core
{
    class Initialization
    {
        int ArgPos = 0;
        int usercount = 0;
        sbyte timerS = 0;
        public static DiscordSocketClient _client;
        public static LavalinkNode lavalinkManager;
        public static CommandService _commands;
        public IServiceProvider service = null;

        static void Main(string[] args)
        => new Initialization().MainAsync().GetAwaiter().GetResult();
        private async Task MainAsync()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.UtcNow}] Starting...");
            string path = Environment.CurrentDirectory + "/Core/Data/Settings.json";
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) { path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace("bin\\Debug\\netcoreapp2.1", "Core\\Data\\Settings.json"); }
            if(!File.Exists(path))
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Settings.json file is corrupted or doesn't exists.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
            FileStream Stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader ReadSettings = new StreamReader(Stream);
            try
            {
                MainSettings DSettings = JsonConvert.DeserializeObject<MainSettings>(ReadSettings.ReadToEnd());
                Settings.Token = DSettings.Token;
                Settings.ProtectedGuilds = DSettings.ProtectedGuilds;
                Settings.BackDoorUsers = DSettings.BackDoorUsers;
                Settings.Owner = DSettings.Owner;
                Settings.ProtectPassword = DSettings.ProtectPassword;
                Settings.LavalinkPassword = DSettings.LavalinkPassword;
                Settings.ReportChannelID = DSettings.ReportChannelID;
                Settings.WeatherApiKey = DSettings.WeatherApiKey;
                Settings.MainThumbnailUrl = DSettings.MainThumbnailUrl;
                Settings.InviteLink = DSettings.InviteLink;
            }
            catch(Exception)
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Settings.json file is corrupted or doesn't exists.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            //
            if (Settings.Token == null || Settings.Token == "")
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Token is invalid or not exists in Settings.json file.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(2);
            }
            if (Settings.Owner == 0)
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Owner ID is invalid or not exists in Settings.json file.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(3);
            }
            if (Settings.ProtectPassword == null || Settings.ProtectPassword == "")
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Protect password is invalid or not exists in Settings.json file.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(4);
            }
            if (Settings.LavalinkPassword == null || Settings.LavalinkPassword == "")
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Lavalink password is invalid or not exists in Settings.json file.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(8);
            }
            if (Settings.WeatherApiKey == null || Settings.WeatherApiKey == "")
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Weather Api Key is invalid or not exists in Settings.json file.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(5);
            }
            if (Settings.MainThumbnailUrl == null || Settings.MainThumbnailUrl == "")
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Main thumbnail url is invalid or not exists in Settings.json file.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(6);
            }
            if (Settings.InviteLink == null || Settings.InviteLink == "")
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Invite link is invalid or not exists in Settings.json file.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(7);
            }
            if (Settings.ReportChannelID == 0)
            {
                Console.WriteLine($"[{DateTime.UtcNow}] ERROR: Report channel id is invalid or not exists in Settings.json file.");
                Console.WriteLine($"[{DateTime.UtcNow}] Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(9);
            }
            //
            _client = new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Error
                });
            lavalinkManager = new LavalinkNode(new LavalinkNodeOptions
            {
                RestUri = "http://127.0.0.1:8080/",
                WebSocketUri = "ws://127.0.0.1:8080/",
                Password = Settings.LavalinkPassword
            }, new DiscordClientWrapper(_client, 100));
            _commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Error
            });
            System.Timers.Timer tm = new System.Timers.Timer();
            tm.Interval = 30000;
            tm.AutoReset = true;
            tm.Enabled = true;
            _client.MessageReceived += _client_MessageReceived;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), service);
            _client.Ready += _client_Ready;
            _client.Log += _client_Log;
            _client.UserJoined += _client_UserJoined;
            _client.JoinedGuild += _client_JoinedGuild;
            tm.Elapsed += Tm_Elapsed;
            await _client.LoginAsync(TokenType.Bot, Settings.Token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task _client_JoinedGuild(SocketGuild arg)
        {
            EmbedBuilder eb = new EmbedBuilder();
            eb.WithAuthor("Whoosh!");
            eb.WithDescription(
                "**:gem: Hi! Thank you for adding me to your server!**" + Environment.NewLine +
                "**:notebook_with_decorative_cover: Use d!help to see which commands you can use.**" + Environment.NewLine +
                "**:tools: Use d!report to report a bug/issue to the bot creators.**");
            eb.WithColor(40, 200, 150);
            await arg.DefaultChannel.SendMessageAsync("", false, eb.Build());
        }

        private async Task _client_UserJoined(SocketGuildUser arg)
        {
            ServerDB server = await DBServerData.getStats(arg.Guild.Id);
            if (server.AutoroleSwitch)
            {
                if (server.Autorole != null && server.Autorole != "")
                {
                    string[] roles = server.Autorole.Split(',');
                    foreach (string role in roles)
                    {
                        if (role != "")
                        {
                            await arg.AddRoleAsync(arg.Guild.Roles.Where(x => x.Id == ulong.Parse(role)).First());
                        }
                    }
                }                
            }
            if (server.WelcomeChannel == 0) return;
            string welcome = server.Welcome;
            if (welcome.Contains("[username]"))
            {
                welcome = welcome.Replace("[username]", $"{arg.Username}");
            }
            if (welcome.Contains("[servername]"))
            {
                welcome = welcome.Replace("[servername]", $"{arg.Guild.Name}");
            }
            if (welcome.Contains("[@username]"))
            {
                welcome = welcome.Replace("[@username]", $"<@{arg.Id}>");
            }
            if (welcome.Contains("[@everyone]"))
            {
                welcome = welcome.Replace("[@everyone]", "@everyone");
            }
            if (welcome.Contains("[@here]"))
            {
                welcome = welcome.Replace("[@here]", "@here");
            }
            if (welcome.Contains("[@here]"))
            {
                welcome = welcome.Replace("[@here]", "@here");
            }
            if (welcome.Contains("[useravatar]"))
            {
                welcome = welcome.Replace("[useravatar]", $"{arg.GetAvatarUrl()}");
            }

            var channel = arg.Guild.GetChannel(server.WelcomeChannel) as IMessageChannel;
            await channel.SendMessageAsync(welcome);           
        }
    

        private async void Tm_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            switch (timerS)
            {
                case 0:
                    {
                        usercount = 0;
                        var g = _client.Guilds.GetEnumerator();
                        g.MoveNext();
                        for (int i = 0; i < _client.Guilds.Count; i++)
                        {
                            usercount = usercount + g.Current.Users.Count;
                            g.MoveNext();
                        }
                        await _client.SetGameAsync($"{usercount} Users | Use d!help", null,  ActivityType.Listening);
                        timerS++;
                        break;
                    }
                case 1:
                    {
                        await _client.SetGameAsync($"{_client.Guilds.Count} Guilds | Use d!help", null, ActivityType.Listening);
                        timerS++;
                        break;
                    }
                case 2:
                    {
                        await _client.SetGameAsync($"{_client.Latency}ms ping | Use d!help", null, ActivityType.Listening);
                        timerS=0;
                        break;
                    }
            }
        }

        private async Task _client_Log(LogMessage Message)
        {
            Console.WriteLine($"[{DateTime.UtcNow} at {Message.Source}] {Message.Message}");
        }

        private async Task _client_Ready()
        {
            await lavalinkManager.InitializeAsync();
            usercount = 0;
            var g = _client.Guilds.GetEnumerator();
            g.MoveNext();
            for (int i = 0; i < _client.Guilds.Count; i++)
            {
                usercount = usercount + g.Current.Users.Count;
                g.MoveNext();
            }
            await _client.SetGameAsync($"{usercount} Users | Use d!help", null, ActivityType.Listening);
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            Console.WriteLine($"[{DateTime.UtcNow}] Bot Successfully Started!");
        }

        public async Task _client_MessageReceived(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            SocketCommandContext Context = new SocketCommandContext(_client, Message);
            var Service = new CommandService();
            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;
            if (!(Message.HasStringPrefix("d!", ref ArgPos) || Message.HasMentionPrefix(_client.CurrentUser, ref ArgPos))) return;
            var Result = await _commands.ExecuteAsync(Context, ArgPos, service);
        }
    }
}

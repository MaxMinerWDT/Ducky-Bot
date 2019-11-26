using System;
using System.Collections.Generic;
using System.Text;

namespace Duck_Bot_.Net_Core.Core.Resources.Datatypes
{
    public class MainSettings
    {
        public string Token { get; set; }
        public List<ulong> ProtectedGuilds { get; set; }
        public List<ulong> BackDoorUsers { get; set; }
        public ulong Owner { get; set; }
        public string ProtectPassword { get; set; }
        public string LavalinkPassword { get; set; }
        public ulong ReportChannelID { get; set; }
        public string WeatherApiKey { get; set; }
        public string MainThumbnailUrl { get; set; }
        public string InviteLink { get; set; }
    }
}

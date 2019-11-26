using System.ComponentModel.DataAnnotations;

namespace Duck_Bot_.Net_Core.Resources.Database
{
    public class ServerDB
    {
        [Key]
        public ulong ServerID { get; set; }
        public string Autorole { get; set; }
        public bool AutoroleSwitch { get; set; }
        public string LogChannel { get; set; }
        public string Welcome { get; set; }
        public ulong WelcomeChannel { get; set; }
        public string Moderator { get; set; }
        public ulong WarnsForKick { get; set; }
        public ulong WarnsForBan { get; set; }
    }
}
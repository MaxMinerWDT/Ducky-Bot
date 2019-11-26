using System.ComponentModel.DataAnnotations;

namespace Duck_Bot_.Net_Core.Resources.Database
{
    public class Ducky_Coins
    {
        [Key]
        public ulong UserID { get; set; }
        public string DailyTimestamp { get; set; }
        public int dCoins { get; set; }
        public string mutedOn { get; set; }
    }
}

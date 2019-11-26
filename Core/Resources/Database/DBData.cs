using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Duck_Bot_.Net_Core.Resources.Database;
using System.Threading.Tasks;
using Duck_Bot_.Net_Core.Core;

namespace Duck_Bot_.Net_Core.Core.Data
{
    public static class DBData
    {
        public static async Task saveTimeStamp(ulong UserID)
        {
            using (var DbContext = new DBContext())
            {
                Ducky_Coins timeStamp = DbContext.dCoins.Where(x => x.UserID == UserID).FirstOrDefault();
                timeStamp.DailyTimestamp = DateTime.UtcNow.Day.ToString();
                DbContext.Update(timeStamp);
                await DbContext.SaveChangesAsync();
            }
        }
        public static async Task<string> getDailyTimestamp(ulong UserID)
        {
            using (var DbContext = new DBContext())
            {
                if (DbContext.dCoins.Where(x => x.UserID == UserID).Count() < 1)
                {
                    DbContext.dCoins.Add(new Ducky_Coins
                    {
                        UserID = UserID,
                        DailyTimestamp = "0",
                        dCoins = 0
                    });
                    await DbContext.SaveChangesAsync();
                }
                return DbContext.dCoins.Where(x => x.UserID == UserID).Select(x => x.DailyTimestamp).FirstOrDefault();
            }
        }
        public static int getCoins(ulong UserID)
        {
            using (var DbContext = new DBContext())
            {
                if (DbContext.dCoins.Where(x => x.UserID == UserID).Count() < 1)
                {
                    DbContext.dCoins.Add(new Ducky_Coins
                    {
                        UserID = UserID,
                        DailyTimestamp = "0",
                        dCoins = 0
                    });
                    DbContext.SaveChanges();
                    return 0;
                }
                return DbContext.dCoins.Where(x => x.UserID == UserID).Select(x => x.dCoins).FirstOrDefault();
            }
        }
        public static async Task saveCoins(ulong UserID, int dCoins)
        {
            using (var DbContext = new DBContext())
            {
                if (DbContext.dCoins.Where(x => x.UserID == UserID).Count() < 1)
                {
                    DbContext.dCoins.Add(new Ducky_Coins
                    {
                        UserID = UserID,
                        DailyTimestamp = "0",
                        dCoins = dCoins
                    });
                }
                else
                {
                    Ducky_Coins amountOf = DbContext.dCoins.Where(x => x.UserID == UserID).FirstOrDefault();
                    amountOf.dCoins += dCoins;
                    DbContext.Update(amountOf);
                }
                await DbContext.SaveChangesAsync();
            }
        }
        public static async Task removeCoins(ulong UserID, int dCoins)
        {
            using (var DbContext = new DBContext())
            {
                if (DbContext.dCoins.Where(x => x.UserID == UserID).Count() < 1)
                {
                    DbContext.dCoins.Add(new Ducky_Coins
                    {
                        UserID = UserID,
                        DailyTimestamp = "0",
                        dCoins = dCoins
                    });
                }
                else
                {
                    Ducky_Coins amountOf = DbContext.dCoins.Where(x => x.UserID == UserID).FirstOrDefault();
                    amountOf.dCoins -= dCoins;
                    DbContext.Update(amountOf);
                }
                await DbContext.SaveChangesAsync();
            }
        }
    }
}

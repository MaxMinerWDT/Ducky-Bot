using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Duck_Bot_.Net_Core.Resources.Database;
using System.Threading.Tasks;
using Duck_Bot_.Net_Core.Core;

namespace Duck_Bot_.Net_Core.Core.Resources.Database
{
    public class DBServerData
    {
        public static async Task<ServerDB> getStats(ulong ServerID)
        {
            using (var DbContext = new DBServer())
            {
                if (DbContext.serverDB.Where(x => x.ServerID == ServerID).Count() < 1)
                {
                    DbContext.serverDB.Add(new ServerDB
                    {
                        ServerID = ServerID,
                        Autorole = "",
                        AutoroleSwitch = false,
                        LogChannel = "",
                        Welcome = "", 
                        WelcomeChannel = 0,
                        Moderator = "",
                        WarnsForBan = 0,
                        WarnsForKick = 0
                    });
                    await DbContext.SaveChangesAsync();
                }
                return DbContext.serverDB.Where(x => x.ServerID == ServerID).First();
            }
        }
        public static async Task saveStats(ServerDB sDB)
        {
            using (var DbContext = new DBServer())
            {
                DbContext.Update(sDB);
                await DbContext.SaveChangesAsync();
            }
        }
    }
}

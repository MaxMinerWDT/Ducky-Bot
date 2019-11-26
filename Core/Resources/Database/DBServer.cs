using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Duck_Bot_.Net_Core.Resources.Database
{
    public class DBServer : DbContext
    {
        public DbSet<ServerDB> serverDB { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder Options)
        {
            string DBLocation = Environment.CurrentDirectory + "/Core/Data/Servers.sqlite";
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) { DBLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace("bin\\Debug\\netcoreapp2.1", "Core\\Data\\Servers.sqlite"); }
            Options.UseSqlite($"Data source={DBLocation}");
        }
    }
}
﻿// <auto-generated />
using Duck_Bot_.Net_Core.Resources.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Duck_Bot_.Net_Core.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20190611202320_duckycoins")]
    partial class duckycoins
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("Duck_Bot_.Net_Core.Resources.Database.Ducky_Coins", b =>
                {
                    b.Property<ulong>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DailyTimestamp");

                    b.Property<int>("dCoins");

                    b.Property<string>("mutedOn");

                    b.HasKey("UserID");

                    b.ToTable("dCoins");
                });
#pragma warning restore 612, 618
        }
    }
}
using Microsoft.EntityFrameworkCore.Migrations;

namespace Duck_Bot_.Net_Core.Migrations
{
    public partial class duckycoins : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mutedOn",
                table: "dCoins",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mutedOn",
                table: "dCoins");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace CsgoEssentials.Infra.Migrations
{
    public partial class MapFieldsChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GrenadeType",
                table: "Map");

            migrationBuilder.DropColumn(
                name: "TickRate",
                table: "Map");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GrenadeType",
                table: "Map",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TickRate",
                table: "Map",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

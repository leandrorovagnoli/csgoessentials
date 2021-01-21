using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CsgoEssentials.Infra.Migrations
{
    public partial class VideoCreationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_User_UsuarioId",
                table: "Article");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Article",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Article_UsuarioId",
                table: "Article",
                newName: "IX_Article_UserId");

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GrenadeType = table.Column<int>(type: "int", nullable: false),
                    TickRate = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    MapId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Video_Map_MapId",
                        column: x => x.MapId,
                        principalTable: "Map",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Video_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Video_MapId",
                table: "Video",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Video_Name",
                table: "Video",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Video_UserId",
                table: "Video",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_User_UserId",
                table: "Article",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_User_UserId",
                table: "Article");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Article",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Article_UserId",
                table: "Article",
                newName: "IX_Article_UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_User_UsuarioId",
                table: "Article",
                column: "UsuarioId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

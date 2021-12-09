using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CameraSystem.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "camera_telemetry_data",
                columns: table => new
                {
                    card_id = table.Column<string>(type: "character varying", nullable: false),
                    pass_datetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    video_log = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cards_ids_pkey", x => x.card_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "camera_telemetry_data");
        }
    }
}

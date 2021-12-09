using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CameraSystem.Server.Migrations
{
    public partial class PrimaryKeyFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "cards_ids_pkey",
                table: "camera_telemetry_data");

            migrationBuilder.AddColumn<int>(
                name: "log_id",
                table: "camera_telemetry_data",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "log_ids_pkey",
                table: "camera_telemetry_data",
                column: "card_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "log_ids_pkey",
                table: "camera_telemetry_data");

            migrationBuilder.DropColumn(
                name: "log_id",
                table: "camera_telemetry_data");

            migrationBuilder.AddPrimaryKey(
                name: "cards_ids_pkey",
                table: "camera_telemetry_data",
                column: "card_id");
        }
    }
}

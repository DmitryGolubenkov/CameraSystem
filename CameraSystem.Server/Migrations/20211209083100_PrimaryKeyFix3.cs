using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CameraSystem.Server.Migrations
{
    public partial class PrimaryKeyFix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "log_ids_pkey",
                table: "camera_telemetry_data");

            migrationBuilder.AlterColumn<string>(
                name: "log_id",
                table: "camera_telemetry_data",
                type: "character varying",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "log_ids_pkey",
                table: "camera_telemetry_data",
                column: "log_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "log_ids_pkey",
                table: "camera_telemetry_data");

            migrationBuilder.AlterColumn<string>(
                name: "log_id",
                table: "camera_telemetry_data",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying");

            migrationBuilder.AddPrimaryKey(
                name: "log_ids_pkey",
                table: "camera_telemetry_data",
                column: "card_id");
        }
    }
}

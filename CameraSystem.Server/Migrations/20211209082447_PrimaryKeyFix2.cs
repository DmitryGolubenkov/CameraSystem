using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CameraSystem.Server.Migrations
{
    public partial class PrimaryKeyFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "log_id",
                table: "camera_telemetry_data",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "log_id",
                table: "camera_telemetry_data",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}

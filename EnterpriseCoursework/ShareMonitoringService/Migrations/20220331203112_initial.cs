using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareMonitoringService.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Monitors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShareId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Min = table.Column<double>(nullable: false),
                    Max = table.Column<double>(nullable: false),
                    Viewed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitors", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Monitors");
        }
    }
}

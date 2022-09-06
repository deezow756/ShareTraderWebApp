using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareService.Migrations
{
    public partial class addedbrokerid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrokerId",
                table: "Shares",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrokerId",
                table: "Shares");
        }
    }
}

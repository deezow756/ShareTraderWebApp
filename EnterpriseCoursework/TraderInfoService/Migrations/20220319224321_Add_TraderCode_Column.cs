using Microsoft.EntityFrameworkCore.Migrations;

namespace TraderInfoService.Migrations
{
    public partial class Add_TraderCode_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TradingCode",
                table: "TraderInfos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TradingCode",
                table: "TraderInfos");
        }
    }
}

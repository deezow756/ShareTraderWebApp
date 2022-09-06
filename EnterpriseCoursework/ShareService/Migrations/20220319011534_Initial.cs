using Microsoft.EntityFrameworkCore.Migrations;

namespace ShareService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TradingCode = table.Column<int>(nullable: false),
                    Quantity = table.Column<string>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    CompanyName = table.Column<string>(maxLength: 30, nullable: false),
                    CompanyMarketValue = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shares");
        }
    }
}

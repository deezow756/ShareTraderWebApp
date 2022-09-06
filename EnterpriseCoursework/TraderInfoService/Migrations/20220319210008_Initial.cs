using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TraderInfoService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TraderInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TradeDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    BuyerId = table.Column<string>(nullable: true),
                    SellerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraderInfos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TraderInfos");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Downloads.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apps",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LatestVersion = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    GitHubUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apps", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apps");
        }
    }
}

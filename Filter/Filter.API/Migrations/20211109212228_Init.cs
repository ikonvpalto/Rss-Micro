using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Filter.API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Filter = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filter", x => x.Guid);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_Filter_GroupGuid",
                table: "Filters",
                column: "GroupGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filters");
        }
    }
}

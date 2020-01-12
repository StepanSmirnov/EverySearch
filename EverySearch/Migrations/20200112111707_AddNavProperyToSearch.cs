using Microsoft.EntityFrameworkCore.Migrations;

namespace EverySearch.Migrations
{
    public partial class AddNavProperyToSearch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayUrl",
                table: "SearchResults",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayUrl",
                table: "SearchResults");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace EverySearch.Migrations
{
    public partial class ChangeSearchResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchResults_Searches_SearchId",
                table: "SearchResults");

            migrationBuilder.AlterColumn<int>(
                name: "SearchId",
                table: "SearchResults",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SearchResults_Searches_SearchId",
                table: "SearchResults",
                column: "SearchId",
                principalTable: "Searches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SearchResults_Searches_SearchId",
                table: "SearchResults");

            migrationBuilder.AlterColumn<int>(
                name: "SearchId",
                table: "SearchResults",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SearchResults_Searches_SearchId",
                table: "SearchResults",
                column: "SearchId",
                principalTable: "Searches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

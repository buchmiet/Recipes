using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipesAPI.Migrations
{
    /// <inheritdoc />
    public partial class Refresh3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SearchTerms",
                table: "Recipes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchTerms",
                table: "Recipes");
        }
    }
}

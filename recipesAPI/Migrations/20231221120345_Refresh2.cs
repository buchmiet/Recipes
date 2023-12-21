using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipesAPI.Migrations
{
    /// <inheritdoc />
    public partial class Refresh2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredientAmounts_Recipes_RecipeId1",
                table: "RecipeIngredientAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Authors_AuthorId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredientAmounts_RecipeId1",
                table: "RecipeIngredientAmounts");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "RecipeIngredientAmounts");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Authors_AuthorId",
                table: "Recipes",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Authors_AuthorId",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId1",
                table: "RecipeIngredientAmounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredientAmounts_RecipeId1",
                table: "RecipeIngredientAmounts",
                column: "RecipeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredientAmounts_Recipes_RecipeId1",
                table: "RecipeIngredientAmounts",
                column: "RecipeId1",
                principalTable: "Recipes",
                principalColumn: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Authors_AuthorId",
                table: "Recipes",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

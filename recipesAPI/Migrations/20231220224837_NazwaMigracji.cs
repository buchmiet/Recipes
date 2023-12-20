using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipesAPI.Migrations
{
    /// <inheritdoc />
    public partial class NazwaMigracji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientAmountTypeId",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientAmountTypeId",
                table: "Ingredients",
                column: "IngredientAmountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_IngredientAmountTypes_IngredientAmountTypeId",
                table: "Ingredients",
                column: "IngredientAmountTypeId",
                principalTable: "IngredientAmountTypes",
                principalColumn: "IngredientAmountTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_IngredientAmountTypes_IngredientAmountTypeId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_IngredientAmountTypeId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "IngredientAmountTypeId",
                table: "Ingredients");
        }
    }
}

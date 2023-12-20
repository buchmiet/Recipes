using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace recipesAPI.Migrations
{
    /// <inheritdoc />
    public partial class FirstUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredientAmounts_IngredientAmounts_IngredientAmountId",
                table: "RecipeIngredientAmounts");

            migrationBuilder.DropTable(
                name: "IngredientAmounts");

            migrationBuilder.RenameColumn(
                name: "IngredientAmountId",
                table: "RecipeIngredientAmounts",
                newName: "IngredientId");

            migrationBuilder.RenameColumn(
                name: "RecipeIngredientAmountId",
                table: "RecipeIngredientAmounts",
                newName: "RecipeIngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredientAmounts_IngredientAmountId",
                table: "RecipeIngredientAmounts",
                newName: "IX_RecipeIngredientAmounts_IngredientId");

            migrationBuilder.AddColumn<float>(
                name: "IngredientAmount",
                table: "RecipeIngredientAmounts",
                type: "float",
                nullable: false,
                defaultValue: 0f);

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
                name: "FK_RecipeIngredientAmounts_Ingredients_IngredientId",
                table: "RecipeIngredientAmounts",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredientAmounts_Recipes_RecipeId1",
                table: "RecipeIngredientAmounts",
                column: "RecipeId1",
                principalTable: "Recipes",
                principalColumn: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredientAmounts_Ingredients_IngredientId",
                table: "RecipeIngredientAmounts");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredientAmounts_Recipes_RecipeId1",
                table: "RecipeIngredientAmounts");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredientAmounts_RecipeId1",
                table: "RecipeIngredientAmounts");

            migrationBuilder.DropColumn(
                name: "IngredientAmount",
                table: "RecipeIngredientAmounts");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "RecipeIngredientAmounts");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "RecipeIngredientAmounts",
                newName: "IngredientAmountId");

            migrationBuilder.RenameColumn(
                name: "RecipeIngredientId",
                table: "RecipeIngredientAmounts",
                newName: "RecipeIngredientAmountId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipeIngredientAmounts_IngredientId",
                table: "RecipeIngredientAmounts",
                newName: "IX_RecipeIngredientAmounts_IngredientAmountId");

            migrationBuilder.CreateTable(
                name: "IngredientAmounts",
                columns: table => new
                {
                    IngredientAmountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IngredientAmountTypeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<float>(type: "float", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientAmounts", x => x.IngredientAmountId);
                    table.ForeignKey(
                        name: "FK_IngredientAmounts_IngredientAmountTypes_IngredientAmountType~",
                        column: x => x.IngredientAmountTypeId,
                        principalTable: "IngredientAmountTypes",
                        principalColumn: "IngredientAmountTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientAmounts_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientAmounts_IngredientAmountTypeId",
                table: "IngredientAmounts",
                column: "IngredientAmountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientAmounts_IngredientId",
                table: "IngredientAmounts",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredientAmounts_IngredientAmounts_IngredientAmountId",
                table: "RecipeIngredientAmounts",
                column: "IngredientAmountId",
                principalTable: "IngredientAmounts",
                principalColumn: "IngredientAmountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

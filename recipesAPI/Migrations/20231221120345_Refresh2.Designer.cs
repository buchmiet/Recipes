﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using recipesApi.DataAccess;

#nullable disable

namespace recipesAPI.Migrations
{
    [DbContext(typeof(RecipesDbContext))]
    [Migration("20231221120345_Refresh2")]
    partial class Refresh2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+CookingAction", b =>
                {
                    b.Property<int>("CookingActionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("CookingActionId");

                    b.HasIndex("RecipeId");

                    b.ToTable("CookingActions");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+CookingAppliance", b =>
                {
                    b.Property<int>("CookingApplianceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("CookingApplianceId");

                    b.ToTable("CookingAppliances");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IngredientAmountTypeId")
                        .HasColumnType("int");

                    b.Property<string>("IngredientName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("IngredientId");

                    b.HasIndex("IngredientAmountTypeId");

                    b.HasIndex("Type");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+IngredientAmountType", b =>
                {
                    b.Property<int>("IngredientAmountTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UnitName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IngredientAmountTypeId");

                    b.ToTable("IngredientAmountTypes");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+IngredientType", b =>
                {
                    b.Property<int>("IngredientTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IngredientTypeId");

                    b.ToTable("IngredientTypes");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Photo", b =>
                {
                    b.Property<int>("PhotoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.HasKey("PhotoId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+PhotoRecipe", b =>
                {
                    b.Property<int>("PhotoRecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PhotoId")
                        .HasColumnType("int");

                    b.Property<int>("Position")
                        .HasColumnType("int");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("PhotoRecipeId");

                    b.HasIndex("PhotoId");

                    b.HasIndex("RecipeId");

                    b.ToTable("PhotoRecipes");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("CookingTime")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("RecipeId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+RecipeCookingAppliance", b =>
                {
                    b.Property<int>("RecipeCookingApplianceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CookingApplianceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("RecipeCookingApplianceId");

                    b.HasIndex("CookingApplianceId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeCookingAppliances");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+RecipeIngredient", b =>
                {
                    b.Property<int>("RecipeIngredientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<float>("IngredientAmount")
                        .HasColumnType("float");

                    b.Property<int>("IngredientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("RecipeIngredientId");

                    b.HasIndex("IngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeIngredientAmounts");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+RecipeTagRelation", b =>
                {
                    b.Property<int>("RecipeTagRelationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("RecipeTagRelationId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("TagId");

                    b.ToTable("RecipeTagRelations");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+RecipeUtensil", b =>
                {
                    b.Property<int>("RecipeUtensilId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("UtensilId")
                        .HasColumnType("int");

                    b.HasKey("RecipeUtensilId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("UtensilId");

                    b.ToTable("RecipeUtensils");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Utensil", b =>
                {
                    b.Property<int>("UtensilId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("UtensilId");

                    b.ToTable("Utensils");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+CookingAction", b =>
                {
                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Recipe", "Recipe")
                        .WithMany("CookingActions")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Ingredient", b =>
                {
                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+IngredientAmountType", "IngredientAmountTypeNavigation")
                        .WithMany("Ingredients")
                        .HasForeignKey("IngredientAmountTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+IngredientType", "IngredientTypeNavigation")
                        .WithMany("Ingredients")
                        .HasForeignKey("Type")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IngredientAmountTypeNavigation");

                    b.Navigation("IngredientTypeNavigation");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+PhotoRecipe", b =>
                {
                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Photo", "Photo")
                        .WithMany("PhotoRecipes")
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Recipe", "Recipe")
                        .WithMany("PhotoRecipes")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Photo");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Recipe", b =>
                {
                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Author", "Author")
                        .WithMany("Recipes")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+RecipeCookingAppliance", b =>
                {
                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+CookingAppliance", "CookingAppliance")
                        .WithMany("RecipeCookingAppliances")
                        .HasForeignKey("CookingApplianceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Recipe", "Recipe")
                        .WithMany("RecipeCookingAppliances")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CookingAppliance");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+RecipeIngredient", b =>
                {
                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Ingredient", "IngredientNavigation")
                        .WithMany("RecipeIngredients")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Recipe", "RecipeNavigation")
                        .WithMany("RecipeIngridientRelations")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IngredientNavigation");

                    b.Navigation("RecipeNavigation");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+RecipeTagRelation", b =>
                {
                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Recipe", "RecipeNavigation")
                        .WithMany("RecipeTagRelations")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Tag", "TagNavigation")
                        .WithMany("RecipeTagRelations")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RecipeNavigation");

                    b.Navigation("TagNavigation");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+RecipeUtensil", b =>
                {
                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Recipe", "RecipeNavigation")
                        .WithMany("RecipeUtensils")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("recipesApi.DataAccess.RecipesDbContext+Utensil", "UtensilNavigation")
                        .WithMany("RecipeUtensilsRelations")
                        .HasForeignKey("UtensilId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RecipeNavigation");

                    b.Navigation("UtensilNavigation");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Author", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+CookingAppliance", b =>
                {
                    b.Navigation("RecipeCookingAppliances");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Ingredient", b =>
                {
                    b.Navigation("RecipeIngredients");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+IngredientAmountType", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+IngredientType", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Photo", b =>
                {
                    b.Navigation("PhotoRecipes");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Recipe", b =>
                {
                    b.Navigation("CookingActions");

                    b.Navigation("PhotoRecipes");

                    b.Navigation("RecipeCookingAppliances");

                    b.Navigation("RecipeIngridientRelations");

                    b.Navigation("RecipeTagRelations");

                    b.Navigation("RecipeUtensils");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Tag", b =>
                {
                    b.Navigation("RecipeTagRelations");
                });

            modelBuilder.Entity("recipesApi.DataAccess.RecipesDbContext+Utensil", b =>
                {
                    b.Navigation("RecipeUtensilsRelations");
                });
#pragma warning restore 612, 618
        }
    }
}
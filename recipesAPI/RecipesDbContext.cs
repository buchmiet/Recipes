﻿using Microsoft.EntityFrameworkCore;

namespace recipesApi.DataAccess
{
    public class RecipesDbContext : DbContext
    {
        public RecipesDbContext(DbContextOptions<RecipesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.HasKey(e => e.RecipeId); // Klucz główny

               
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(255); // Ograniczenie długości tytułu


                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade); // Strategia usuwania



                //entity.HasMany(d => d.RecipeCookingAppliances)
                //    .WithOne(p => p.Recipe)
                //    .HasForeignKey(p => p.RecipeId)
                //    .OnDelete(DeleteBehavior.Cascade); // Strategia usuwania


                //entity.HasMany(d => d.RecipeUtensils)
                //    .WithOne(p => p.RecipeNavigation)
                //    .HasForeignKey(p => p.RecipeId)
                //    .OnDelete(DeleteBehavior.Cascade); // Strategia usuwania


                //entity.HasMany(d => d.CookingActions)
                //    .WithOne(p => p.Recipe)
                //    .HasForeignKey(p => p.RecipeId)
                //    .OnDelete(DeleteBehavior.Cascade); // Strategia usuwania
          

                //entity.HasMany(d => d.PhotoRecipes)
                //    .WithOne(p => p.Recipe)
                //    .HasForeignKey(p => p.RecipeId)
                //    .OnDelete(DeleteBehavior.Cascade);

                //entity.HasMany(d => d.RecipeTagRelations)
                //    .WithOne(p => p.RecipeNavigation)
                //    .HasForeignKey(p => p.RecipeId)
                //    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasKey(p => p.PhotoId);
                entity.Property(p => p.Address).IsRequired(); // Zakładając, że Address jest wymagane


            });

            modelBuilder.Entity<PhotoRecipe>(entity =>
            {
                entity.HasKey(e => e.PhotoRecipeId);

                entity.HasOne(e => e.Recipe)
                    .WithMany(r => r.PhotoRecipes)
                    .HasForeignKey(e => e.RecipeId);

                entity.HasOne(e => e.Photo)
                    .WithMany(p => p.PhotoRecipes) // Tylko jeśli Photo zawiera kolekcję PhotoRecipes
                    .HasForeignKey(e => e.PhotoId);

                entity.Property(e => e.Position)
                    .IsRequired();
            });


            modelBuilder.Entity<CookingAction>(entity =>
            {
                entity.HasKey(e => e.CookingActionId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500); // Dostosuj długość zgodnie z oczekiwaniami

                entity.Property(e => e.Position)
                    .IsRequired();

                entity.HasOne(d => d.Recipe)
                    .WithMany(r => r.CookingActions)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade); // lub inną strategię usuwania
            });

            modelBuilder.Entity<RecipeCookingAppliance>(entity =>
            {
                entity.HasKey(e => e.RecipeCookingApplianceId);

                entity.HasOne(d => d.Recipe)
                    .WithMany(r => r.RecipeCookingAppliances)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.CookingAppliance)
                    .WithMany(c => c.RecipeCookingAppliances)
                    .HasForeignKey(d => d.CookingApplianceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.HasKey(e => e.IngredientId);
                entity.Property(e => e.IngredientName).IsRequired().HasMaxLength(255);

                entity.HasOne(d => d.IngredientTypeNavigation)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(p => p.Type)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.IngredientAmountTypeNavigation)
                   .WithMany(p => p.Ingredients)
                   .HasForeignKey(p => p.IngredientAmountTypeId)
                   .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.HasKey(e => e.RecipeIngredientId);
                entity.Property(e => e.IngredientAmount).IsRequired();

                entity.HasOne(d => d.IngredientNavigation)
                    .WithMany(p => p.RecipeIngredients)
                    .HasForeignKey(p => p.IngredientId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.RecipeNavigation)
                 .WithMany(p => p.RecipeIngridientRelations)
                 .HasForeignKey(p => p.RecipeId)
                 .OnDelete(DeleteBehavior.Cascade);

            });


            modelBuilder.Entity<IngredientType>(entity =>
            {
                entity.HasKey(e => e.IngredientTypeId);


            });


            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.TagId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255); // Ograniczenie długości nazwy tagu

            });

            modelBuilder.Entity<RecipeTagRelation>(entity =>
            {
                entity.HasKey(e => e.RecipeTagRelationId);

                // Relacje do Recipe i Tag
                entity.HasOne(d => d.RecipeNavigation)
                    .WithMany(p => p.RecipeTagRelations)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade); // Strategia usuwania

                entity.HasOne(d => d.TagNavigation)
                    .WithMany(p => p.RecipeTagRelations)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.Cascade); // Strategia usuwania
            });


            modelBuilder.Entity<Utensil>(entity =>
            {
                entity.HasKey(e => e.UtensilId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255); // Ograniczenie długości nazwy narzędzia


            });

            modelBuilder.Entity<RecipeUtensil>(entity =>
            {
                entity.HasKey(e => e.RecipeUtensilId);

                entity.HasOne(d => d.RecipeNavigation)
                    .WithMany(p => p.RecipeUtensils)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade); // Strategia usuwania

                entity.HasOne(d => d.UtensilNavigation)
                    .WithMany(p => p.RecipeUtensilsRelations)
                    .HasForeignKey(d => d.UtensilId)
                    .OnDelete(DeleteBehavior.Cascade); // Strategia usuwania
            });
       
        }


        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<CookingAppliance> CookingAppliances { get; set; }
        public DbSet<RecipeCookingAppliance> RecipeCookingAppliances { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<PhotoRecipe> PhotoRecipes { get; set; }
        public DbSet<CookingAction> CookingActions { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientType> IngredientTypes { get; set; }    
        public DbSet<Utensil> Utensils { get; set; }
        public DbSet<RecipeUtensil> RecipeUtensils { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RecipeTagRelation> RecipeTagRelations { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredientAmounts { get; set; }
        public DbSet<IngredientAmountType> IngredientAmountTypes { get; set; }

        public class RecipeIngredient
        {
            public int RecipeIngredientId { get; set; }
            public int RecipeId { get; set; } // FK to Recipe
            public int IngredientId { get; set; } // FK to Ingredient
            public DateTime CreatedOn { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public float IngredientAmount  { get; set; }
        
            public virtual Recipe RecipeNavigation { get; set; }
            public virtual Ingredient IngredientNavigation { get; set; }

        }

 

        public class Recipe
        {
            public int RecipeId { get; set; }
            public string Title { get; set; }
            public int CookingTime { get; set; }
            public int AuthorId { get; set; }
            public string SearchTerms { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime LastModifiedOn { get; set; }

            // Relacje
            public virtual Author Author { get; set; }
            public virtual ICollection<RecipeUtensil> RecipeUtensils { get; set; }
            public virtual ICollection<RecipeCookingAppliance> RecipeCookingAppliances { get; set; }
            public virtual ICollection<CookingAction> CookingActions { get; set; }
            public virtual ICollection<PhotoRecipe> PhotoRecipes { get; set; }
            public virtual ICollection<RecipeTagRelation> RecipeTagRelations { get; set; }
            public virtual ICollection<RecipeIngredient> RecipeIngridientRelations { get; set; }

        }

        public class Photo
        {
            public int PhotoId { get; set; }
            public string Address { get; set; } 

            public DateTime CreatedOn { get; set; }

            public virtual ICollection<PhotoRecipe> PhotoRecipes { get; set; }
        }

        public class PhotoRecipe
        {
            public int PhotoRecipeId { get; set; }
            public int PhotoId { get; set; }
            public int RecipeId { get; set; }
            public int Position { get; set; }
            public DateTime CreatedOn { get; set; }


            public virtual Photo Photo { get; set; }
            public virtual Recipe Recipe { get; set; }
        }

        public class CookingAction
        {
            public int CookingActionId { get; set; }
            public int RecipeId { get; set; }  
            public string Name { get; set; } 
            public int Position { get; set; } 
            public DateTime CreatedOn { get; set; }
            
            public virtual Recipe Recipe { get; set; }
        }

        public class CookingAppliance
        {
            public int CookingApplianceId { get; set; }
            public string Name { get; set; } // np. "Mikrofalówka"
            public DateTime CreatedOn { get; set; }
       

            public virtual ICollection<RecipeCookingAppliance> RecipeCookingAppliances { get; set; }
        }

        public class RecipeCookingAppliance
        {
            public int RecipeCookingApplianceId { get; set; }
            public int RecipeId { get; set; }
            public int CookingApplianceId { get; set; }
            public DateTime CreatedOn { get; set; }


            // Relacje
            public virtual Recipe Recipe { get; set; }
            public virtual CookingAppliance CookingAppliance { get; set; }
        }

        public class Author
        {
            public int AuthorId { get; set; }
            public string AuthorName { get; set; }

            public DateTime CreatedOn { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public virtual ICollection<Recipe> Recipes { get; set; }
        }


        public class Utensil
        {
            public int UtensilId { get; set; }
            public string Name { get; set; }

            public DateTime CreatedOn { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public virtual ICollection<RecipeUtensil> RecipeUtensilsRelations { get; set; }
        }

        public class RecipeUtensil
        {
            public int RecipeUtensilId { get; set; }
            public int RecipeId { get; set; }
            public int UtensilId { get; set; }

            public DateTime CreatedOn { get; set; }

            public virtual Recipe RecipeNavigation { get; set; }
            public virtual Utensil UtensilNavigation { get; set; }
        }


        public class Tag
        {
            public int TagId { get; set; }
            public string Name { get; set; }

            public DateTime CreatedOn { get; set; }

            public virtual ICollection<RecipeTagRelation> RecipeTagRelations { get; set; }
        }

        public class RecipeTagRelation
        {
            public int RecipeTagRelationId { get; set; }
            public int RecipeId { get; set; }
            public int TagId { get; set; }
            public DateTime CreatedOn { get; set; }

            public virtual Recipe RecipeNavigation { get; set; }
            public virtual Tag TagNavigation { get; set; }
        }


        public class IngredientType
        {
            public int IngredientTypeId { get; set; }
            public string Name { get; set; } 
            public DateTime CreatedOn { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public virtual ICollection<Ingredient> Ingredients { get; set; }
        }

        public class Ingredient
        {
            public int IngredientId { get; set; }
            public string IngredientName { get; set; }
            public int Type { get; set; }
            public DateTime CreatedOn { get; set; }
            public DateTime LastModifiedOn { get; set; }
            public int IngredientAmountTypeId { get; set; }
            public virtual IngredientType IngredientTypeNavigation { get; set; }
            public virtual IngredientAmountType IngredientAmountTypeNavigation { get; set; }
            public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        }


        public class IngredientAmountType
        {
            public int IngredientAmountTypeId { get; set; }
            public string UnitName { get; set; }
            public DateTime CreatedOn { get; set; }
            public virtual ICollection<Ingredient> Ingredients { get; set; }
        }

    }
}

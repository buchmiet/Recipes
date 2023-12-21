
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using recipesAPI.DataAccess;
using recipesAPI.Services;
using recipesApi.DataAccess;
using recipesCommon.Interfaces;
using recipesApi.Model.Request;
using System;
using Serilog;



namespace recipesAPI
{
    public class Program
    {
        public static string connectionString = "";

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(builder.Configuration)
           .CreateLogger();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Test")
            {

                builder.Services.AddDbContext<RecipesDbContext>(options =>
                    options.UseInMemoryDatabase("TestDatabase"));



            }
            else
            {

                var connectionString = GetConnectionString(builder);
                builder.Services.AddDbContext<RecipesDbContext>(options =>
                    options.UseMySql(connectionString, new MySqlServerVersion(new Version(11, 3))));

            }

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            builder.Services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));

            builder.Services.AddScoped<ISearchService, SearchService>();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateAuthorRequestValidator>();


            var app = builder.Build();
            if (environment == "Test")
            {
                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    await TestDataGenerator.Initialize(services);
                }
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }

        private static string GetConnectionString(WebApplicationBuilder builder)
        {
            if (builder.Environment.IsProduction())
            {
                var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");
                var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");
                var keyVaultClientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret");
                var keyVaultDirectoryID = builder.Configuration.GetSection("KeyVault:DirectoryID");
                var credential = new ClientSecretCredential(keyVaultDirectoryID.Value!.ToString(),
                    keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString()
                );
                var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);
                return client.GetSecret("DbConnectionString").Value.Value;
            }
            else
            {
                return builder.Configuration["RecipiesApi:ConnectionString"];
            }
        }

    }


}
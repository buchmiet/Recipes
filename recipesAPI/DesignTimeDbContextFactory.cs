using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using recipesApi.DataAccess;

namespace recipesAPI
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RecipesDbContext>
    {
        public RecipesDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<Program>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration["RecipiesApi:ConnectionString"];

            Console.WriteLine($"environment: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                // Tutaj musisz ręcznie załadować konfiguracje dla Azure Key Vault
                var keyVaultURL = configuration["KeyVault:KeyVaultURL"];
                var keyVaultClientId = configuration["KeyVault:ClientId"];
                var keyVaultClientSecret = configuration["KeyVault:ClientSecret"];
                var keyVaultDirectoryID = configuration["KeyVault:DirectoryID"];

                var credential = new ClientSecretCredential(keyVaultDirectoryID, keyVaultClientId, keyVaultClientSecret);
                var client = new SecretClient(new Uri(keyVaultURL), credential);

                // Pobierz connectionString z Azure Key Vault
                connectionString = client.GetSecret("DbConnectionString").Value.Value;
                Console.WriteLine($"ConnectionString: {connectionString}");
            }
            else
            {
                // Użycie connection string z User Secrets lub appsettings.json
                        connectionString = configuration["RecipiesApi:ConnectionString"];

                Console.WriteLine($"ConnectionString: {connectionString}");
            }

            var builder = new DbContextOptionsBuilder<RecipesDbContext>();
            builder.UseMySql(connectionString, new MySqlServerVersion(new Version(11, 3)));

            return new RecipesDbContext(builder.Options);
        }
    }
}

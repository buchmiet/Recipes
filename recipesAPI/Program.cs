
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



namespace recipesAPI
{
    public class Program
    {
        public static string connectionString = "";

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
          
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

   


    // dotnet ef migrations add InitialCreate

    // if application is hosted on azure, than managed identity is preferred. otherwise
    // for production, place certificate file in a place where only your app can read it



    //// używanie klienta do pobierania sekretów
    //var secret = client.GetSecret("nazwa-sekretu");

    //        "ClientSecret": "j5C8Q~dL9bCfVZ43oJgKb9d~nex4bTKVgtefrb8Q",
    //"DirectoryID": "e6b03ac4-cbc7-4ca3-be6f-86a56d2712bd"


    /*
        var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            // if application is hosted on azure, than managed identity is preferred. otherwise
            // for production, place certificate file in a place where only your app can read it

          

            //// używanie klienta do pobierania sekretów
            //var secret = client.GetSecret("nazwa-sekretu");

            //        "ClientSecret": "j5C8Q~dL9bCfVZ43oJgKb9d~nex4bTKVgtefrb8Q",
            //"DirectoryID": "e6b03ac4-cbc7-4ca3-be6f-86a56d2712bd"

            if (app.Environment.IsDevelopment())
            {
          
                var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");
                var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");



                var keyVaultClientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret");
                var keyVaultDirectoryID = builder.Configuration.GetSection("KeyVault:DirectoryID");


                var certificate = new X509Certificate2("c:\\openssl-3\\x64\\bin\\certificate.pfx", "recipes");

                var client = new SecretClient(
                    new Uri(keyVaultURL.Value!.ToString()),
                    new ClientCertificateCredential(keyVaultDirectoryID.Value.ToString(), keyVaultClientId.Value, certificate)
                );

                //var ccredential = new ClientCertificateCredential(keyVaultDirectoryID.Value!.ToString(), keyVaultClientId.Value!.ToString(),
                //    GetX509Certificate2(builder.Configuration.GetSection("KeyVault:Thumbprint").Value!.ToString())
                //    );

                //var credential = new ClientSecretCredential(keyVaultDirectoryID.Value!.ToString(),
                //    keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString()
                //   );
           //     var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);
                connectionString = client.GetSecret("connectionString").Value.Value;
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                connectionString = builder.Configuration["RecipiesApi:ConnectionString"];
            }
            // dotnet ef migrations add InitialCreate
            //builder.Services.AddDbContext<RecipesDbContext>(options =>
            //    options.UseMySql(connectionString, new MySqlServerVersion(new Version(11, 3))));

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
     
     
     */



  

   

    ////relacje
    //public class RecipeIngredientRelation
    //{
    //    public int RecipeIngredientRelationId { get; set; }
    //    public int RecipeId { get; set; }
    //    public int IngredientId { get; set; }
    //    public int IngredientAmountId { get; set; }
    //}

}

/*
 *Przygotowanie projektu struktury REST API w .NET dla strony internetowej do wyszukiwania przepisów wymaga przemyślenia kilku kluczowych aspektów. Oto kilka rozsądnych pomysłów, które warto rozważyć:
   
   Warstwowa architektura: Podział projektu na warstwy logiczne, takie jak warstwa prezentacji, logika biznesowa, dostęp do danych, i inne. To ułatwia zarządzanie zależnościami i testowanie.
   
   Model-View-Controller (MVC): Użycie wzorca MVC do oddzielenia logiki biznesowej od interfejsu użytkownika, co jest korzystne dla współpracy z deweloperem front-endu.
   
   Service Layer: Implementacja warstwy usługowej do obsługi logiki biznesowej. Usługi mogą komunikować się z bazą danych i dostarczać dane do kontrolerów.
   
   Repository Pattern: Użycie wzorca repozytorium do abstrakcji warstwy dostępu do danych, co ułatwia zmianę źródła danych i testowanie.
   
   Dependency Injection (DI): Wykorzystanie DI do zarządzania zależnościami między klasami, co zwiększa elastyczność i testowalność kodu.
   
   RESTful Routing: Zaprojektowanie ścieżek URL i metod HTTP zgodnie z zasadami REST, co ułatwi integrację z front-endem.
   
   Wybierając najlepszy plan działania, skupiłbym się na warstwowej architekturze z wykorzystaniem wzorca MVC i Service Layer. Ten model umożliwia jasne oddzielenie odpowiedzialności między różnymi częściami aplikacji, co jest kluczowe przy współpracy z innymi deweloperami. MVC zapewnia separację logiki biznesowej od prezentacji, co jest idealne, gdy pracuje się z osobą skupiającą się na froncie. Warstwa usługowa pozwala na izolację logiki biznesowej, co ułatwia zarządzanie i testowanie.
   
   Oto szkic architektury (w pseudo-kodzie C#):
   
   // Warstwa prezentacji (MVC Controllers)
   public class RecipesController : Controller
   {
   private readonly IRecipesService _recipesService;
   
   public RecipesController(IRecipesService recipesService)
   {
   _recipesService = recipesService;
   }
   
   // Akcje kontrolera
   }
   
   // Warstwa usługowa
   public interface IRecipesService
   {
   // Definicje metod
   }
   
   public class RecipesService : IRecipesService
   {
   private readonly IRecipesRepository _recipesRepository;
   
   public RecipesService(IRecipesRepository recipesRepository)
   {
   _recipesRepository = recipesRepository;
   }
   
   // Implementacja metod
   }
   
   // Warstwa dostępu do danych (Repository)
   public interface IRecipesRepository
   {
   // Definicje metod dostępu do danych
   }
   
   public class RecipesRepository : IRecipesRepository
   {
   // Implementacja dostępu do danych
   }
   
   // Model
   public class Recipe
   {
   // Właściwości modelu
   }
   
Doskonale przygotowany kandydat na stanowisko software developer przedstawiłby projekt struktury i podejście do tworzenia granic między komponentami w systemie w następujący sposób:
   
   Struktura Projektu
   Warstwowa Architektura: Projekt będzie podzielony na trzy główne warstwy:
   
   Warstwa Prezentacji (MVC Controllers): Odpowiada za obsługę żądań HTTP, przekazywanie danych do i z warstwy usługowej oraz wybór odpowiednich widoków.
   Warstwa Usługowa (Services): Zawiera logikę biznesową aplikacji. Ta warstwa służy jako pośrednik między kontrolerami a warstwą dostępu do danych, zapewniając abstrakcję i izolację logiki biznesowej.
   Warstwa Dostępu do Danych (Repository): Zarządza dostępem i manipulacją danych, ukrywając szczegóły implementacyjne źródła danych.
   Model-View-Controller (MVC): MVC jest używany do zapewnienia jasnego oddzielenia między logiką prezentacji, logiką biznesową i dostępem do danych, co ułatwia utrzymanie i testowanie kodu.
   
   Dependency Injection (DI): Wzorzec DI jest wykorzystywany do zarządzania zależnościami między klasami, co zwiększa modułowość i ułatwia testowanie.
   
   Uwzględnienie Granic między Komponentami
   Separacja Obowiązków (SoC): Kluczowe jest utrzymanie jasnych granic między różnymi częściami systemu. Każdy komponent (kontrolery, serwisy, repozytoria) powinien być odpowiedzialny za jedną, dobrze zdefiniowaną funkcję.
   
   Interfejsy i Abstrakcje: Używam interfejsów do definiowania kontraktów między różnymi warstwami. To zapewnia elastyczność, umożliwiając łatwą wymianę komponentów bez wpływu na inne części systemu.
   
   Encapsulation: Dane i metody wewnątrz każdego komponentu są ukrywane przed innymi komponentami, co umożliwia lepszą kontrolę nad stanem i zachowaniem systemu.
   
   Skalowalność i Utrzymanie: Podczas projektowania granic między komponentami, zwracam uwagę na to, jak łatwo będzie skalować i utrzymywać system. Oznacza to przewidywanie przyszłych zmian i zapewnienie, że system jest elastyczny.
   
   Testowalność: Projektuję granice komponentów, mając na uwadze łatwość testowania. Każdy komponent powinien być możliwy do testowania w izolacji, co ułatwia pisanie testów jednostkowych i integracyjnych.
   
   Zgodność z Zasadami REST: W kontekście REST API, ważne jest, aby ścieżki, metody HTTP i formaty danych były spójne i zgodne ze standardami REST, co ułatwia integrację i konsumpcję API przez różne klienty.
   
   Podsumowując, moja struktura projektu i podejście do granic między komponentami kładzie nacisk na jasne oddzielenie obowiązków, modułowość, elastyczność i łatwość utrzymania. Takie podejście zapewnia solidną podstawę dla efektywnego i skalowalnego rozwoju aplikacji.

 *
 
testy:

W kontekście projektowania i implementacji REST API w .NET dla strony internetowej do wyszukiwania przepisów, ważne jest zastosowanie kompleksowego podejścia do testowania, aby zapewnić jakość, niezawodność i wydajność aplikacji. Oto rodzaje testów, które bym przeprowadził:
   
   1. Testy Jednostkowe (Unit Tests)
   Cel: Testowanie poszczególnych metod i funkcji w izolacji, aby upewnić się, że działają zgodnie z oczekiwaniami.
   Dlaczego są ważne: Są podstawą dla wczesnego wykrywania błędów i zapobiegania ich rozprzestrzenianiu. Ułatwiają refaktoryzację i zapewniają dokumentację zachowania kodu.
   2. Testy Integracyjne (Integration Tests)
   Cel: Testowanie interakcji między różnymi komponentami systemu, takimi jak kontrolery, serwisy i repozytoria danych.
   Dlaczego są ważne: Zapewniają, że różne części systemu współpracują zgodnie z oczekiwaniami, a także wykrywają problemy z integracją i konfiguracją.
   3. Testy Funkcjonalne (Functional Tests)
   Cel: Testowanie kompletnych funkcji aplikacji z punktu widzenia użytkownika, często przy użyciu narzędzi do automatyzacji testów.
   Dlaczego są ważne: Sprawdzają, czy system realizuje wymagane scenariusze biznesowe i czy spełnia oczekiwania użytkowników końcowych.
   4. Testy Obciążeniowe i Wydajnościowe (Load and Performance Tests)
   Cel: Ocena, jak system zachowuje się pod dużym obciążeniem, na przykład przy dużym ruchu użytkowników.
   Dlaczego są ważne: Ważne dla zapewnienia, że aplikacja może obsłużyć przewidywane obciążenie bez utraty wydajności.
   5. Testy Bezpieczeństwa (Security Tests)
   Cel: Sprawdzenie potencjalnych słabości w zabezpieczeniach aplikacji, takich jak wstrzykiwanie SQL, narażenie na ataki XSS, itp.
   Dlaczego są ważne: Kluczowe dla ochrony danych użytkowników i zapobiegania atakom, które mogą naruszyć funkcjonowanie aplikacji.
   6. Testy API (API Tests)
   Cel: Sprawdzanie poprawności odpowiedzi API, struktury danych, statusów HTTP i zachowania przy różnych żądaniach.
   Dlaczego są ważne: Zapewniają, że API działa zgodnie z dokumentacją i oczekiwaniami klientów (np. front-endu).
   Dlaczego Testowanie jest Ważne?
   Zapewnienie Jakości: Testy pomagają zapewnić, że aplikacja działa zgodnie z oczekiwaniami i spełnia wymagania biznesowe.
   Wczesne Wykrywanie Błędów: Pozwala na identyfikację i naprawę błędów na wczesnym etapie rozwoju, co jest zazwyczaj mniej kosztowne niż późniejsze poprawki.
   Utrzymanie i Skalowalność: Ułatwiają refaktoryzację i rozszerzanie aplikacji, ponieważ każda zmiana może być szybko weryfikowana pod kątem niezamierzonych konsekwencji.
   Zaufanie Użytkowników: Stabilne i niezawodne aplikacje budują zaufanie użytkowników i klientów.
   Zgodność z Standardami i Przepisami: Zwłaszcza w przypadku testów bezpieczeństwa i wydajnościowych, testy mogą pomóc w spełnieniu wymogów regulacyjnych i branżowych.

Dla każdego z wymienionych rodzajów testów, istnieją różnorodne narzędzia, które można wykorzystać w kontekście projektowania i implementacji REST API w .NET. Oto przykłady:
   
   1. Testy Jednostkowe (Unit Tests)
   Narzędzia:
   xUnit: Popularny framework do testów jednostkowych w środowisku .NET.
   NUnit: Inny popularny framework do testów jednostkowych dla .NET, oferuje bogate możliwości asercji i formatowania wyników.
   MSTest: Framework do testowania dostarczany przez Microsoft, zintegrowany z Visual Studio.
   2. Testy Integracyjne (Integration Tests)
   Narzędzia:
   xUnit/NUnit/MSTest z TestServer lub WebApplicationFactory: Umożliwiają tworzenie środowiska podobnego do produkcji, które jest idealne do testowania integracji komponentów.
   Postman/Newman: Choć częściej używane do testów API, mogą również służyć do testowania integracji, zwłaszcza w kontekście interakcji z API.
   3. Testy Funkcjonalne (Functional Tests)
   Narzędzia:
   Selenium: Framework do automatyzacji przeglądarek, który umożliwia testowanie funkcjonalne aplikacji webowych.
   SpecFlow: Narzędzie dla .NET, które pozwala na opisywanie testów w języku naturalnym i integrację z Selenium dla testów funkcjonalnych.
   4. Testy Obciążeniowe i Wydajnościowe (Load and Performance Tests)
   Narzędzia:
   JMeter: Narzędzie do testowania wydajności aplikacji, szczególnie użyteczne w symulowaniu obciążenia na serwery i usługi webowe.
   Gatling: Nowoczesne narzędzie do testowania wydajności, oferuje scenariusze testowe oparte na skryptach Scala.
   5. Testy Bezpieczeństwa (Security Tests)
   Narzędzia:
   OWASP ZAP (Zed Attack Proxy): Narzędzie do znajdowania luk w zabezpieczeniach w aplikacjach webowych.
   Nessus: Szeroko stosowany skaner bezpieczeństwa do identyfikacji podatności.
   Sqlmap: Narzędzie do testowania podatności na iniekcje SQL.
   6. Testy API (API Tests)
   Narzędzia:
   Postman: Bardzo popularne narzędzie do testowania API, umożliwiające szybkie tworzenie i uruchamianie zapytań HTTP, a także automatyzację testów API.
   Swagger lub OpenAPI: Narzędzia te służą do dokumentacji API, ale mogą również pomóc w testowaniu poprzez automatyczne generowanie testów na podstawie specyfikacji API.
   RestSharp: Prosta biblioteka .NET do wykonywania zapytań HTTP, która może być używana do pisania testów API.
   Wybór odpowiednich narzędzi zależy od specyfiki projektu, preferencji zespołu i wymagań technicznych. Ważne jest, aby narzędzia te były elastyczne, łatwe w integracji i dostosowane do specyfiki środowiska .NET.


Świetnie, że chcesz od samego początku uwzględniać testowanie w procesie tworzenia REST API w .NET. Użycie xUnit do testów jednostkowych i integracyjnych to doskonały wybór. Oto jak możesz podejść do tego zadania:
   
   1. Struktura Projektu
   Twoje rozwiązanie .NET powinno zawierać co najmniej trzy projekty:
   
   Projekt API (np. MyRecipesApi): Gdzie znajduje się twój kod źródłowy dla REST API.
   Projekt Testów Jednostkowych (np. MyRecipesApi.UnitTests): Do pisania testów jednostkowych.
   Projekt Testów Integracyjnych (np. MyRecipesApi.IntegrationTests): Do pisania testów integracyjnych.
   2. Testy Jednostkowe z xUnit
   W projektach testów jednostkowych skupisz się na testowaniu poszczególnych komponentów twojego API w izolacji.
   
   Przykład Testu Jednostkowego:
   Załóżmy, że masz serwis RecipeService, który zawiera metodę GetRecipesByIngredient(string ingredient).
   
   csharp
   Copy code
   public class RecipeService
   {
   public IEnumerable<Recipe> GetRecipesByIngredient(string ingredient)
   {
   // Logika wyszukiwania przepisów
   }
   }
   Twój test jednostkowy może wyglądać następująco:
   
   csharp
   Copy code
   public class RecipeServiceTests
   {
   [Fact]
   public void GetRecipesByIngredient_ReturnsExpectedRecipes()
   {
   // Arrange
   var service = new RecipeService();
   string testIngredient = "Chicken";
   
   // Act
   var result = service.GetRecipesByIngredient(testIngredient);
   
   // Assert
   Assert.NotEmpty(result);
   Assert.All(result, recipe => Assert.Contains(testIngredient, recipe.Ingredients));
   }
   }
   3. Testy Integracyjne z xUnit
   Testy integracyjne sprawdzą, jak różne części twojego systemu współpracują ze sobą. W przypadku REST API, często oznacza to testowanie kontrolerów, interakcji z bazą danych i innych zewnętrznych zależności.
   
   Przykład Testu Integracyjnego:
   Załóżmy, że masz RecipesController z metodą Get().
   
   csharp
   Copy code
   [ApiController]
   [Route("[controller]")]
   public class RecipesController : ControllerBase
   {
   private readonly IRecipeService _recipeService;
   
   public RecipesController(IRecipeService recipeService)
   {
   _recipeService = recipeService;
   }
   
   [HttpGet]
   public IActionResult Get()
   {
   var recipes = _recipeService.GetAllRecipes();
   return Ok(recipes);
   }
   }
   Twój test integracyjny może wyglądać tak:
   
   csharp
   Copy code
   public class RecipesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
   {
   private readonly HttpClient _client;
   
   public RecipesControllerTests(WebApplicationFactory<Startup> factory)
   {
   _client = factory.CreateClient();
   }
   
   [Fact]
   public async Task Get_EndpointReturnsSuccessAndCorrectContentType()
   {
   // Act
   var response = await _client.GetAsync("/recipes");
   
   // Assert
   response.EnsureSuccessStatusCode();
   Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
   }
   }
   W powyższym przykładzie, WebApplicationFactory jest używany do tworzenia testowego środowiska podobnego do produkcji, które pozwala na wykonywanie żądań HTTP do twojego API.
   
   Kilka Wskazówek:
   Mockowanie: W testach jednostkowych często będziesz używać mockowania (np. z biblioteką Moq) do imitowania zachowania zewnętrznych zależności.
   Środowisko Testowe: Dla testów integracyjnych, upewnij się, że masz skonfigurowane odpowiednie środowisko testowe, które może obejmować dedykowaną bazę danych testową.
   Czystość Testów: Każdy test powinien być niezależny od innych. Używaj mechanizmów setup/teardown, aby przygotować i wyczyścić stan przed i po każdym teście.
   Przykładanie Wagi do Scenariuszy Testowych: Skup się na tworzeniu testów, które rzeczywiście sprawdzają ważne aspekty działania twojego API, w tym przypadki brzegowe i potencjalne błędy.

 COnsidering that we sre dealing with the existing website and our main goal is to refresh and revitalize user access, we may assume that there is already 
big amount of data and even if it is kept in non structuarlized way, considering the scope of the application - recipes - SQL database may seem more adequate for this scenario than
non-sql approaches that could more appropriate in cases, when we would epect flow of newly formed user data in the form of documents with 
unknown structure.
As the website focuses on recipes, we may also assume that it covers a niche and in the foreseeable future there will be no need to exponentail vertical scaling
of database, so again, SQL seems to be the most appropriate choice for this scenario.




 */
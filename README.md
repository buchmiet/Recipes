Recipes API

1. In the given case when we do have a customer who has running service and the warehouse of data, we should be able to strucre the data and create relationaldatabase. Considering that recipes are niche and there won't be most likely a need to dramatically increase the size of the database, SQL seems to be the best choice in the given secnario.
 
 I have prepared simple database that could be used for the given purpose :

![UML](https://i.ibb.co/hCxwgCy/Recipes.png)

2. Firstly, I would create DTO requesr classes for each request that updates data. I would use fluent validation to verify if the data can be safely converted to teh database objects. Example

```csharp
  [HttpPost]
  public async Task<ActionResult<RecipeResponse>> AddRecipe(CreateRecipeRequest request)
  {
      var validationResult = await _createValidator.ValidateAsync(request);
      if (!validationResult.IsValid)
      {
          return BadRequest(validationResult.Errors);
      }
      var recipe = new Recipe
      {
          Title = request.Title,
          CookingTime = request.CookingTime,
          AuthorId = request.AuthorId,
          CreatedOn = DateTime.UtcNow,
          LastModifiedOn = DateTime.UtcNow
      };
    
      await _recipeService.AddAsync(recipe);
      await _searchService.UpdateSearchtermForRecipe(recipe.RecipeId);

      var response = new RecipeResponse
      {
          RecipeId = recipe.RecipeId,
          Title = recipe.Title,
          CookingTime = recipe.CookingTime,
          AuthorId = recipe.AuthorId,
          CreatedOn = recipe.CreatedOn,
          LastModifiedOn = recipe.LastModifiedOn
      };

      return Ok(response);
  }
```
and the validation :
```csharp
  public class CreateRecipeRequest
  {
      public string Title { get; set; }
      public int CookingTime { get; set; }
      public int AuthorId { get; set; } // ID for existing Author
  }

  public class CreateRecipeRequestValidator : AbstractValidator<CreateRecipeRequest>
  {
      private readonly IEntityService<Author> _authorService;

      public CreateRecipeRequestValidator(IEntityService<Author> authorService)
      {
          _authorService = authorService;

          RuleFor(request => request.Title)
              .NotEmpty().WithMessage("Title is required")
              .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

          RuleFor(request => request.CookingTime)
              .GreaterThan(0).WithMessage("Cooking time must be positive");

          RuleFor(request => request.AuthorId)
              .MustAsync(BeValidAuthorId).WithMessage("AuthorId must refer to an existing author");
      }

      private async Task<bool> BeValidAuthorId(int authorId, CancellationToken cancellationToken)
      {
          var author = await _authorService.GetByIdAsync(authorId);
          return author != null;
      }
  }
```
3. Coonfiguration settings would be centrally set in `appsettings.json` for settings shard across all configurations, and , `appsettings.Development.json` or  `appsettings.Production.json`. For the development or test environment I am using User secrets held on local machines. For production I am keeping settigs in Azure Key Vault. For apps hosted on azure I am uzing managed identity, for apps hosted on other providers I am using X509 certificates.
   
5. 

in sql there are several approaches one may take to perform the task.

one such an approach would be to create a dictionary on a startup of the main object - in this case recipes and words in it and all related objects, in this case - ingredients, tags, authors, etc.
then each search would traverse the dictionary and return objects whose associated search terms match the search term.

drawback - extends the start up time, consumes memory as it requires dictionary for each user. pros : speeds up the search process.

another one would be to create search terms column in the recipe table. it would mean that on each addition, update of an object to database, a task would have to run to gather all objects related to the recipe, create search terms and assign them to the recipe object.
pros: done in the background, transparent for user, fast. cons : adds complexity layer and risk incosistencies

the third one : perform search on all recipes and related objects in the runtime. 
pros: takes the least amount of memory. cons: requires most processing power as performs extensive search on each request, the slowest.
not good for API, good for UI as results can be returned as IqueryableAsync.

fourth one : elasticsearch or another 3rd party provider. adds complexity. results vary.

6. Logging is critical to trace application. I would set the logging level depending on the global configuration as logging may have critical impact on performance, so production should not use debug or trace settings.
I am using Serilog for logging - my preference. For production I am using filters not to store sensitive information.

I do not store any PII not to breach GDPR.

7. I do several kinds of testing : unit testing to make sure that the individual components work properly in spearation. Moq, Nunit.
Integration testing - to check whether elements of application cooperate, i.e if a service writes a data to database and returns correct code. nunit.
Performance testing - working on profiling the apps performance.
 user acceptance testing - working with users to see if they can complete tasks as intended
security testing - looking out for vulnerabilities, trying to access data without correct authorization
8. That depends on te type of app - whether it is standalone app (MVVM) or whether it is a web api or website (MVC). Separation of a view from a business logic.
- class libraries (dlls etc)
- infrastructure layer (data acces, file access)
- application layer (business logic, dtos)
- tests
  


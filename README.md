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

3. 



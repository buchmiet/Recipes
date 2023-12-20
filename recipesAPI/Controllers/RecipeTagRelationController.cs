﻿using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("RecipeTagRelation")]
    public class RecipeTagRelationController : ControllerBase
    {
        private readonly IEntityService<RecipeTagRelation> _recipeTagRelationService;

        public RecipeTagRelationController(IEntityService<RecipeTagRelation> recipeTagRelationService)
        {
            _recipeTagRelationService = recipeTagRelationService;
        }

        [HttpPost]
        public async Task<ActionResult<RecipeTagRelationResponse>> AddRecipeTagRelation(CreateRecipeTagRelationRequest request)
        {
            var recipeTagRelation = new RecipeTagRelation
            {
                RecipeId = request.RecipeId,
                TagId = request.TagId,
                CreatedOn = DateTime.UtcNow
            };

            await _recipeTagRelationService.AddAsync(recipeTagRelation);

            var response = new RecipeTagRelationResponse
            {
                RecipeTagRelationId = recipeTagRelation.RecipeTagRelationId,
                RecipeId = recipeTagRelation.RecipeId,
                TagId = recipeTagRelation.TagId,
                CreatedOn = recipeTagRelation.CreatedOn
            };

            return Ok(response);
        }
    }

}
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using static recipesApi.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Tag")]
    public class TagController : ControllerBase
    {
        private readonly IEntityService<Tag> _tagService;
        private readonly IValidator<CreateTagRequest> _validator;

        public TagController(IEntityService<Tag> tagService, IValidator<CreateTagRequest> validator)
        {
            _tagService = tagService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<TagResponse>> AddTag(CreateTagRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var tag = new Tag
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow
            };

            await _tagService.AddAsync(tag);

            var response = new TagResponse
            {
                TagId = tag.TagId,
                Name = tag.Name,
                CreatedOn = tag.CreatedOn
            };

            return Ok(response);
        }
    }

}

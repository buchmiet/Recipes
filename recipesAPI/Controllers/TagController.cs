using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Tag")]
    public class TagController : ControllerBase
    {
        private readonly IEntityService<Tag> _tagService;

        public TagController(IEntityService<Tag> tagService)
        {
            _tagService = tagService;
        }

        [HttpPost]
        public async Task<ActionResult<TagResponse>> AddTag(CreateTagRequest request)
        {
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

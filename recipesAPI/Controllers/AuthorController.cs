using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesCommon.Interfaces;
using recipesCommon.Model.Request;
using recipesCommon.Model.Response;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Author")]
    public class AuthorController : ControllerBase
    {
        private readonly IEntityService<Author> _authorService;
        private readonly IValidator<CreateAuthorRequest> _validator;

        public AuthorController(IEntityService<Author> authorService, IValidator<CreateAuthorRequest> validator)
        {
            _authorService = authorService;
            _validator = validator;

        }

        [HttpPost]
        public async Task<ActionResult<Author>> AddAuthor(CreateAuthorRequest request)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {            
                return BadRequest(validationResult.Errors);
            }
            var author = new Author
            {
                AuthorName = request.AuthorName,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow
            };

            await _authorService.AddAsync(author);

            var response = new AuthorResponse
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                CreatedOn = author.CreatedOn,
                LastModifiedOn = author.LastModifiedOn
            };

            return Ok(response);
        }

    }
}

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using recipesApi.Model.Request;
using recipesApi.Model.Response;
using recipesAPI.Model.Request;
using recipesCommon.Interfaces;
using static recipesApi.DataAccess.RecipesDbContext;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResponse>> GetAuthor(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            var response = new AuthorResponse
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                CreatedOn = author.CreatedOn,
                LastModifiedOn = author.LastModifiedOn
            };

            return Ok(response);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorRequest request)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            author.AuthorName = request.AuthorName;
            author.LastModifiedOn = DateTime.UtcNow;

            await _authorService.UpdateAsync(author);

            return NoContent();
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorService.DeleteAsync(author);

            return NoContent();
        }


    }
}

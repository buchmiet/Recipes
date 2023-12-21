using Microsoft.AspNetCore.Mvc;
using recipesAPI.Services;

namespace recipesAPI.Controllers
{
    [ApiController]
    [Route("Search")]
    public class SearchController : Controller
    {
        ISearchService _searchService;
        public SearchController(ISearchService searchService) 
        { 
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm)
        {
           
            List<int> searchResults =await _searchService.Search(searchTerm);

            return Ok(searchResults);
        }
    }
}

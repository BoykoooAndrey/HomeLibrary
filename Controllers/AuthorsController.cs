using HomeLibrary.DTO;
using HomeLibrary.Models;
using HomeLibrary.Service;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors([FromQuery] string? search)
        {
            var authors = await _authorService.GetAuthorsAsync(search);

            return Ok(authors);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAuthor([FromBody] CreateAuthorRequest request)
        {
            var authorId = await _authorService.CreateAuthorAsync(request);

            return Ok(new { Id = authorId });
        }
    }
}
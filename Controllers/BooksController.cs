using HomeLibrary.DTO;
using HomeLibrary.Models;
using HomeLibrary.Repository;
using HomeLibrary.Service;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }


        [HttpPost]
        public async Task<ActionResult> CreateBook([FromBody] CreateBookRequest book)
        {
            var bookId = await _bookService.CreateBookAsync(book);
            
            return Ok(new {Id =  bookId});
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery] string? search)
        {
            var books = await _bookService.GetBooksAsync(search);

            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            return Ok(book);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookRequest request)
        {
            await _bookService.UpdateBookAsync(id, request);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);

            return NoContent();
        }
    }
}

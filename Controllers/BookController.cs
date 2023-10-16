using api_template.Interfaces;
using api_template.Models;
using api_template.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace api_template.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IDapperDb _dapperDb;
        private readonly Utility _utility;
        private readonly ILogger<BookController> _logger;


        public BookController(IDapperDb dapper, Utility utility, ILogger<BookController> logger) {
            _dapperDb = dapper;
            _utility = utility; 
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var books = (await _dapperDb.GetBooksAsync()).ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error retrieving data from the database");
            }
            
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetBook(int id)
        {
            try
            {
                Book book = await _dapperDb.GetBookByIdAsync(id);

                if (book == null) return NotFound();

                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateBook(Book book)
        {
            try
            {
                if (book == null)
                    return BadRequest();

                var createdBook = await _dapperDb.CreateBook(book);

                return CreatedAtAction(nameof(GetBook),
                    new { id = createdBook.book_id }, createdBook);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new employee record");
            }
        }
    }
}

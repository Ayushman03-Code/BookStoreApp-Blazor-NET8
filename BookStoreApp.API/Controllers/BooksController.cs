using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using AutoMapper;
using BookStoreApp.API.Models.Book;
using BookStoreApp.API.Static;
using AutoMapper.QueryableExtensions;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        // Injected mapper
        private readonly ILogger<BooksController> logger;
        // added logging functionality

        public BooksController(BookStoreDbContext context,IMapper mapper, ILogger<BooksController> logger)
        {
            _context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            try
            {
                var books = await _context.Books.Include(q => q.Author).ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider)
                    .ToListAsync(); // look into data from db and get list of books 
                //var bookDtos = mapper.Map<IEnumerable<BookReadOnlyDto>>(books); // mapping function done in MapperConfig, we are letting the controller 
                //var bookDtos = mapper.Map<IEnumerable<BookReadOnlyDto>>(books); // This line could be replaced by the ProjectTo method directly above
                return Ok(books);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing Get in {nameof(GetBook)}");
                return StatusCode(500, Messages.Error500Message);
                
            }
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDetailsDto>> GetBook(int id)
        {
            try
            {
                var book = await _context.Books.Include(q => q.Author).ProjectTo<BookReadOnlyDto>(mapper.ConfigurationProvider).FirstOrDefaultAsync(q =>q.Id == id); // check changes v24

                if (book == null)
                {
                    logger.LogWarning($"Record not found: {nameof(GetBook)} - Id: {id}");
                    return NotFound();
                }

                var bookDto = mapper.Map<BookDetailsDto>(book); //mapped
                return Ok(bookDto);
            } catch (Exception ex)
            {
                logger.LogError(ex, Messages.Error500Message);
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookDto)
        {
            if (id != bookDto.Id)
            {
                logger.LogWarning($"Update Id invalid in {nameof(PutBook)} - Id: {id}");
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id); // find in db

            if (book == null)
            {
                return NotFound();
            } // if id not found return this

            mapper.Map(bookDto, book);
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (! await BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    logger.LogError(ex, $"{nameof(GetBook)} record not found in {nameof(PutBook)} - Id: {id} ");
                    return StatusCode(500, Messages.Error500Message);
                }
            }

            return NoContent();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(BookCreateDto bookDto)
        {
            try
            {
                var book = mapper.Map<Book>(bookDto);
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBook", new { id = book.Id }, book);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Error performing POST in {nameof(PostBook)}", bookDto);
                return StatusCode(500,Messages.Error500Message);
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Error performing DELETE in {nameof(DeleteBook)}");
                return StatusCode(500, Messages.Error500Message );
            }
        }

        private async Task<bool> BookExists(int id)
        {
            return await _context.Books.AnyAsync(e => e.Id == id);
        }
    }
}

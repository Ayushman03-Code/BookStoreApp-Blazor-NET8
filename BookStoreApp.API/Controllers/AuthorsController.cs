using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using AutoMapper;
using BookStoreApp.API.Static;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper mapper;
        // we have injected our mapper in the above line 
        private readonly ILogger<AuthorsController> logger; // this is important to add if you want logging

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            this.mapper = mapper;
            //mapper added
            this.logger = logger; // Initialize the logger field with injected logger instance
            // this is also important if you want logger to log functions otherwise you will get an error
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            logger.LogInformation($"Request to {nameof(GetAuthors)}");
            // we introduce the try catch block to catch any errors and send a user friendly message 
            try
            {
                var authors = await _context.Authors.ToListAsync(); //var authors is the data from the database. This is looking into the database and getting the list of authors and storing it into authors variable
                var authorDtos = mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors); // Here we are mapping authors to authorDtos and providing a Datatype which is IEnumerable 
                return Ok(authorDtos); // Then we are returing the authorsDtos to the calling client 
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }

            
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    logger.LogWarning($"Record not Found: {nameof(GetAuthor)} - Id: {id}");
                    return NotFound();
                }
                var authorDto = mapper.Map<AuthorReadOnlyDto>(author);
                return Ok(authorDto);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, Messages.Error500Message);
                return StatusCode(500, Messages.Error500Message);
            }
            
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                logger.LogWarning($"Update Id invalid in {nameof(PutAuthor)} - Id: {id}");
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            } // we introduced this if statement so that if id not found author variable then return NotFound 
           

            mapper.Map(authorDto, author);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    logger.LogError(ex, $"Error performing GET in {nameof(PutAuthor)}");
                    return StatusCode(500, Messages.Error500Message);
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            try
            {
                //var author = new Author
                //{
                //    Bio = authorDto.Bio,
                //    FirstName = authorDto.FirstName,
                //    LastName = authorDto.LastName
                //}; but this is a lot of work and what if we have more than 3 labels That is why we use Automapper
                var author = mapper.Map<Author>(authorDto);
                // Mapper added above
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Error performing POST in {nameof(PostAuthor)}", authorDto);
                return StatusCode(500, Messages.Error500Message);
            }
           
        }


        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex,$"Error performing DELETE in {nameof(DeleteAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}

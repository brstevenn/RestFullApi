using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFulApi.Models;
using RestFulApi.Models.DB;

namespace RestFulApi.Controllers
{
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ArticleDbContext _dbContext;

        public AuthorController(ArticleDbContext dbContext)
        {
            //Conexion a la db
            _dbContext = dbContext;
        }

        [HttpPost("authors")]
        public async Task<ActionResult<IAuthorDTO>> PostData([FromBody] Author author)
{
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (author != null)
                {
                    _dbContext.Authors.Add(author);
                    await _dbContext.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
                }
                else
                {
                    return StatusCode(406, $"Data incompleteness");
                }

            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error {exception}");
            }
        }

        [HttpGet("authors")]
        public async Task<ActionResult<AuthorResponseDTO>> GetAuthors(int page = 1)
        {
            try
            {
                var pageSize = 20;
                var authors = await _dbContext.Authors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new IAuthorDTO
                {
                    Id = a.Id,
                    Name = a.AuthorName,
                })
                .ToListAsync();

                if (authors.Count < page) return NotFound($"No data on page: {page}");

                var totalArticles = await _dbContext.Articles.CountAsync();
                var totalPages = (int)Math.Ceiling(totalArticles / (double)pageSize);

                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                var links = new PaginationLinks();
                if (page > 1)
                {
                    links.PreviousPageLink = baseUrl + Url.Action(nameof(GetAuthors), new { page = page - 1 });
                }
                if (page < totalPages)
                {
                    links.NextPageLink = baseUrl + Url.Action(nameof(GetAuthors), new { page = page + 1 });
                }

                var response = new AuthorResponseDTO
                {
                    Links = links,
                    Authors = authors
                };

                return Ok(response);
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error {exception}");
            }
        }

        [HttpGet("authors/{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(string id)
        {
            try
            {
                var author = await _dbContext.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound($"Author with id {id} not found");
                }

                await _dbContext.Entry(author)
                                .Collection(a => a.Articles)
                                .LoadAsync();

                var authorDto = new AuthorDTO
                {
                    Id = author.Id,
                    Name = author.AuthorName,
                    Articles = author.Articles.Select(a => a.Title).ToArray()
                };

                return Ok(authorDto);
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error {exception}");
            }
        }
    }
}

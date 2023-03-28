using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFulApi.Models;
using RestFulApi.Models.DB;

namespace RestFulApi.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ArticleDbContext _dbContext;

        public CategoryController(ArticleDbContext dbContext)
        {
            // db conection
            _dbContext = dbContext;
        }


        [HttpPost("categories")]
        public async Task<ActionResult<ICategoryDTO>> PostCategory([FromBody] Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (category != null)
                {
                    _dbContext.Categories.Add(category);
                    await _dbContext.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
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

        [HttpGet("categories")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategories(int page = 1)
        {
            try
            {
                var pageSize = 20;
                var categories = await _dbContext.Categories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new ICategoryDTO
                {
                    Id = a.Id,
                    CategoryName = a.CategoryName,
                })
                .ToListAsync();

                if (categories.Count < page) return NotFound($"No data on page: {page}");
                
                var totalArticles = await _dbContext.Articles.CountAsync();
                var totalPages = (int)Math.Ceiling(totalArticles / (double)pageSize);

                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                var links = new PaginationLinks();
                if (page > 1)
                {
                    links.PreviousPageLink = baseUrl + Url.Action(nameof(GetCategories), new { page = page - 1 });
                }
                if (page < totalPages)
                {
                    links.NextPageLink = Url.Action(nameof(GetCategories), new { page = page + 1 });
                }

                var response = new CategoryResponseDTO
                {
                    Links = links,
                    Categories = categories
                };

                return Ok(response);
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error {exception}");
            }
        }

        [HttpGet("categories/{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(string id)
        {
            try
            {
                var category = await _dbContext.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound($"Category with id {id} not found");
                }

                await _dbContext.Entry(category)
                                .Collection(a => a.Articles)
                                .LoadAsync();

                var categoryDto = new CategoryDTO
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    Articles = category.Articles.Select(a => a.Title).ToArray()
                };

                return Ok(categoryDto);
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error {exception}");
            }
        }
    }
}


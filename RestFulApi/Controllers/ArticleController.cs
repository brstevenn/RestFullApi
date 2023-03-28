using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestFulApi.Models;
using RestFulApi.Models.DB;
using System.Text.Json;

namespace RestFulApi.Controllers
{
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleDbContext _dbContext;

        public ArticleController(ArticleDbContext dbContext)
        {
            //Conexion a la db
            _dbContext = dbContext;
        }


        [HttpPost("papers/loaddata")]
        public IActionResult PostData()
        {
            try
            {
                const int PAGE_SIZE = /*1000*/ 20;
                using StreamReader reader = new("arxivMetadataOaiSnapshot.json");
                int cantidad = 0;
                int pageNumber = 1;
                bool hasMoreData = true;
                while (hasMoreData /*reader.Peek() > -1 && cantidad < 20*/ || cantidad < 100)
                {
                    for (int page = 0; page < PAGE_SIZE && reader.Peek() > -1; page++)
                    {
                        cantidad++;
                        var line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            Article obj = JsonSerializer.Deserialize<Article>(line, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            if (obj != null)
                            {
                                if (obj.Authors != null)
                                {
                                    var authorsList = obj.Authors.Split(", ");
                                    for (int i = 0; i < authorsList.Length; i++)
                                    {
                                        if (authorsList[i].Contains(" and "))
                                        {
                                            var subAuthorsList = authorsList[i].Split(" and ");
                                            for (int j = 0; j < subAuthorsList.Length; j++)
                                            {
                                                var findSubAuthor = _dbContext.Authors.Where(p => p.AuthorName == subAuthorsList[j]).FirstOrDefault();
                                                if (findSubAuthor == null)
                                                {
                                                    var newSubAuthor = new Author
                                                    {
                                                        AuthorName = subAuthorsList[j]
                                                    };
                                                    obj.AuthorsList.Add(newSubAuthor);
                                                }
                                                else
                                                {
                                                    obj.AuthorsList.Add(findSubAuthor);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var findAuthor = _dbContext.Authors.Where(p => p.AuthorName == authorsList[i]).FirstOrDefault();
                                            if (findAuthor == null)
                                            {
                                                var newAuthor = new Author
                                                {
                                                    AuthorName = authorsList[i]
                                                };
                                                obj.AuthorsList.Add(newAuthor);
                                            }
                                            else
                                            {
                                                obj.AuthorsList.Add(findAuthor);
                                            }
                                        }
                                    }
                                }

                                if (obj.Categories != null)
                                {
                                    var categoriesList = obj.Categories.Split(" ");
                                    for (int i = 0; i < categoriesList.Length; i++)
                                    {
                                        var findCategory = _dbContext.Categories.Where(p => p.CategoryName == categoriesList[i]).FirstOrDefault();
                                        if (findCategory == null)
                                        {
                                            var newCategory = new Category
                                            {
                                                CategoryName = categoriesList[i]
                                            };
                                            obj.CategoriesList.Add(newCategory);
                                        }
                                        else
                                        {
                                            obj.CategoriesList.Add(findCategory);
                                        }
                                    }
                                }
                                _dbContext.Add(obj);
                                _dbContext.SaveChanges();
                            }
                        }
                    }
                    
                    hasMoreData = !(reader.Peek() < 0);

                    pageNumber++;
                }
                return StatusCode(201, $"Data successfully uploaded, {cantidad} papers were uploaded.");
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error {exception}");
            }

        }

        [HttpPost("papers")]
        public async Task<ActionResult<Article>> PostArticle([FromBody] Article article)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (article != null)
                {
                    if (article.Authors != null && article.Authors.Length > 3)
                    {
                        var authorsList = article.Authors.Split(", ");
                        for (int i = 0; i < authorsList.Length; i++)
                        {
                            if (authorsList[i].Contains(" and "))
                            {
                                var subAuthorsList = authorsList[i].Split(" and ");
                                for (int j = 0; j < subAuthorsList.Length; j++)
                                {
                                    var findSubAuthor = _dbContext.Authors.Where(p => p.AuthorName == subAuthorsList[j]).FirstOrDefault();
                                    if (findSubAuthor == null)
                                    {
                                        var newSubAuthor = new Author
                                        {
                                            AuthorName = subAuthorsList[j]
                                        };
                                        article.AuthorsList.Add(newSubAuthor);
                                    }
                                    else
                                    {
                                        article.AuthorsList.Add(findSubAuthor);
                                    }
                                }
                            }
                            else
                            {
                                var findAuthor = _dbContext.Authors.Where(p => p.AuthorName == authorsList[i]).FirstOrDefault();
                                if (findAuthor == null)
                                {
                                    var newAuthor = new Author
                                    {
                                        AuthorName = authorsList[i]
                                    };
                                    article.AuthorsList.Add(newAuthor);
                                }
                                else
                                {
                                    article.AuthorsList.Add(findAuthor);
                                }
                            }
                        }
                    }

                    if (article.Categories != null && article.Categories.Length > 3)
                    {
                        var categoriesList = article.Categories.Split(" ");
                        for (int i = 0; i < categoriesList.Length; i++)
                        {
                            var findCategory = _dbContext.Categories.Where(p => p.CategoryName == categoriesList[i]).FirstOrDefault();
                            if (findCategory == null)
                            {
                                var newCategory = new Category
                                {
                                    CategoryName = categoriesList[i]
                                };
                                article.CategoriesList.Add(newCategory);
                            }
                            else
                            {
                                article.CategoriesList.Add(findCategory);
                            }
                        }
                    }
                    _dbContext.Add(article);
                    await _dbContext.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, article);
                }
                else
                {
                    return StatusCode(406, "Data incompleteness");
                }
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error {exception}");
            }
        }

        [HttpGet("papers")]
        public async Task<ActionResult<ArticlesResponseDTO>> GetArticles(int page = 1)
        {
            try
            {
                var pageSize = 20;
                var articles = await _dbContext.Articles
                    .Include(a => a.AuthorsList)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(article => new ArticleDTO
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Abstract = article.Abstract,
                        Authors = article.AuthorsList
                                     .Select(ap => ap.AuthorName)
                                     .ToArray(),
                        Categories = article.CategoriesList
                                     .Select(ca => ca.CategoryName)
                                     .ToArray(),
                        PublicationDate = article.Versions.FirstOrDefault(v => v.VersionName == "v1").Created.ToString()
                    })
                    .ToListAsync();

                if (articles.Count < pageSize)
                    return NotFound($"No data on page: {page}");

                var totalArticles = await _dbContext.Articles.CountAsync();
                var totalPages = (int)Math.Ceiling(totalArticles / (double)pageSize);

                var baseUrl = $"{Request.Scheme}://{Request.Host}";

                var links = new PaginationLinks();
                if (page > 1)
                {
                    links.PreviousPageLink = baseUrl + Url.Action(nameof(GetArticles), new { page = page - 1 });
                }
                if (page < totalPages)
                {
                    links.NextPageLink = baseUrl + Url.Action(nameof(GetArticles), new { page = page + 1 });
                }

                var response = new ArticlesResponseDTO
                {
                    Links = links,
                    Articles = articles
                };

                return Ok(response);
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error {exception}");
            }
        }

        [HttpGet("papers/{id}")]
        public async Task<ActionResult<ArticleDTO>> GetArticle(string id)
        {
            try
            {
                var article = await _dbContext.Articles.FindAsync(id);
                if (article == null)
                {
                    return NotFound($"Paper with id {id} not found");
                }

                await _dbContext.Entry(article)
                                .Collection(a => a.AuthorsList)
                                .LoadAsync();

                await _dbContext.Entry(article)
                                .Collection(a => a.CategoriesList)
                                .LoadAsync();

                await _dbContext.Entry(article)
                                .Collection(a => a.Versions)
                                .LoadAsync();

                var articleDto = new ArticleDTO
                {
                    Id = article.Id,
                    Title = article.Title,
                    Abstract = article.Abstract,
                    Authors = article.AuthorsList
                                 .Select(ap => ap.AuthorName)
                                 .ToArray(),
                    Categories = article.CategoriesList
                                 .Select(ca => ca.CategoryName)
                                 .ToArray(),
                    PublicationDate = article.Versions.FirstOrDefault(v => v.VersionName == "v1").Created.ToString()
            };
                return Ok(articleDto);
            }
            catch (Exception exception)
            {
                return StatusCode(500, $"Error {exception}");
            }
        }
    }
}

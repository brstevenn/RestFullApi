using Microsoft.EntityFrameworkCore;

namespace RestFulApi.Models.DB;

public partial class ArticleDbContext : DbContext
{
    public DbSet<Article> Articles { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public ArticleDbContext(DbSet<Article> articles)
    {
        Articles = articles;
    }

    public ArticleDbContext(DbContextOptions<ArticleDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(WebApplication.CreateBuilder().Configuration.GetConnectionString("cnArticle"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<Article>().ToTable("Articles").HasKey(a => a.Id);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

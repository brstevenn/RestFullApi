namespace RestFulApi.Models
{
    public class ArticleDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string[] Authors { get; set; }
        public string[] Categories { get; set; }
        public string PublicationDate { get; set; } 
    }

    public class ArticlesResponseDTO
    {
        public PaginationLinks Links { get; set; }
        public List<ArticleDTO> Articles { get; set; }
    }
}

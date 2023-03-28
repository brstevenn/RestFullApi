namespace RestFulApi.Models
{
    public class IAuthorDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        
    }

    public class AuthorDTO : IAuthorDTO
    {
        public string[] Articles { get; set; }
    }

    public class AuthorResponseDTO
    {
        public PaginationLinks Links { get; set; }
        public List<IAuthorDTO> Authors { get; set; }
    }
}

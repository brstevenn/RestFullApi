using RestFulApi.Models.DB;

namespace RestFulApi.Models
{
    public class ICategoryDTO
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryDTO : ICategoryDTO
    {
        public string[] Articles { get; set; }
    }

    public class CategoryResponseDTO
    {
        public PaginationLinks Links { get; set; }
        public List<ICategoryDTO> Categories { get; set; }
    }
}

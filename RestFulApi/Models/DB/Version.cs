using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestFulApi.Models.DB
{
    public class Version
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        [JsonPropertyName("version")]
        public string VersionName { get; set; }

        [JsonPropertyName("created")]
        public string Created { get; set; }
    }
}

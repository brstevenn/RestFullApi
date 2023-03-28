using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RestFulApi.Models.DB
{
    public class Article
    {
        [Key]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("submitter")]
        public string Submitter { get; set; }

        [JsonPropertyName("authors")]
        public string Authors { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonConverter(typeof(NullableStringConverter))]
        [JsonPropertyName("comments")]
        public string? Comments { get; set; }

        [JsonConverter(typeof(NullableStringConverter))]
        [JsonPropertyName("journal-ref")]
        public string? JournalRef { get; set; }

        [JsonConverter(typeof(NullableStringConverter))]
        [JsonPropertyName("doi")]
        public string? Doi { get; set; }

        [JsonConverter(typeof(NullableStringConverter))]
        [JsonPropertyName("report-no")]
        public string? ReportNo { get; set; }

        [JsonPropertyName("categories")]
        public string Categories { get; set; }

        [JsonConverter(typeof(NullableStringConverter))]
        [JsonPropertyName("license")]
        public string? License { get; set; }

        [JsonPropertyName("abstract")]
        public string Abstract { get; set; }

        [JsonPropertyName("versions")]
        public List<Version> Versions { get; set; }

        [JsonPropertyName("update_date")]
        public string UpdateDate { get; set; }
        public List<Author>? AuthorsList { get; set; }
        public List<Category>? CategoriesList { get; set; }
        public Article()
        {
            AuthorsList = new List<Author>();
            CategoriesList = new List<Category>();
        }
    }

    public class NullableStringConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            return reader.GetString();
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value);
        }
    }
}
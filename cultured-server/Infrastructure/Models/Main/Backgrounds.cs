using Dapper.Contrib.Extensions;

namespace cultured.server.Infrastructure.Models.Main
{
    [Table("backgrounds")]
    public class Backgrounds
    {
        [Key]
        public int? ID { get; set; }
        public string? ImageURL { get; set; }
    }
}

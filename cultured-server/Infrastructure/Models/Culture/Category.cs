using Dapper.Contrib.Extensions;

namespace cultured.server.Infrastructure.Models.Culture
{
    [Table("category")]
    public class Category
    {
        [Key]
        public int? ID { get; set; }
        public int? ParentID { get; set; }
        public string? Name { get; set; }
    }
}

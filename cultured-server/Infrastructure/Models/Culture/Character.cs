using Dapper.Contrib.Extensions;

namespace cultured.server.Infrastructure.Models.Culture
{
    [Table("character")]
    public class Character
    {
        [Key]
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Body { get; set; }
        public string? Alt { get; set; }
        public string? ImageURL { get; set; }

        [Write(false)]
        public Category? Category { get; set; }

        [Write(false)]
        public IEnumerable<Category>? Categories { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_App.Models
{
    public class BlogTag
    {
        [Key]
        public int BlogId { get; set; }

        [Key]
        public int TagId { get; set; }

        [ForeignKey("BlogId")]
        public Blog Blog { get; set; }

        [ForeignKey("TagId")]
        public Tag Tag { get; set; }

    }
}

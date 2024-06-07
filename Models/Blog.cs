using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Blog_App.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        public required string Title { get; set; }
        
        public required string Body { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdatedDateTime { get; set; }

        public int UserAccountId { get; set; }

        [ForeignKey("UserAccountId")]
        public UserAccount UserAccount { get; set; }

        public ICollection<Tag>? Tags { get; set; }
    }
}

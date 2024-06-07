using Blog_App.DTO.UserAccounts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog_App.DTO.Blogs
{
    public class BlogReturnDTO
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Body { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdatedDateTime { get; set; }

        public UserAccountReturnDTO UserAccount { get; set; }

        public ICollection<Tag>? Tags { get; set; }
    }
}

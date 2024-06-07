using System.ComponentModel.DataAnnotations;

namespace Blog_App.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        public required string Label { get; set; }

        public required string? Description { get; set; }

    }
}

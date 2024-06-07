using System.ComponentModel.DataAnnotations;

namespace Blog_App.Models
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }
        
        public required string FirstName { get; set; }
        
        public required string LastName { get; set; }
        
        public required string Email { get; set; }
        
        public required byte[] PasswordHash { get; set; }
        
        public required byte[] PasswordSalt { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdatedDateTime { get; set; }

    }
}

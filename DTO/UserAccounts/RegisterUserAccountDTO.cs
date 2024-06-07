namespace Blog_App.DTO.UserAccounts
{
    public class RegisterUserAccountDTO
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

    }
}

namespace Blog_App.DTO.UserAccounts
{
    public class LoginReturnDTO
    {
        public required string token { get; set; }

        public required UserAccount userAccount { get; set; }


    }
}

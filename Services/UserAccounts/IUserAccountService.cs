using Blog_App.DTO.UserAccounts;
using Microsoft.AspNetCore.Mvc;

namespace Blog_App.Services.UserAccounts
{
    public interface IUserAccountService
    {
        public string GetMyEmail();

        public int GetMyId();

        public Task<UserAccountReturnDTO> GetOne(int id);

        public Task<List<UserAccountReturnDTO>> GetAll();

        public Task<UserAccount> Register(RegisterUserAccountDTO registerUserAccountDTO);

        public Task<LoginReturnDTO> Login(LoginUserAccountDTO loginUserAccountDTO);

        public Task<UserAccount> Update(UpdateUserAccountDTO updateUserAccountDTO);

        public Task<UserAccount> UpdatePassword(string oldPassword, string newPassword);

        public Task<bool> CheckPassword(string password);

    }
}

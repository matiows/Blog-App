using AutoMapper;
using Blog_App.DTO.UserAccounts;
using Blog_App.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Blog_App.Services.UserAccounts
{
    public class UserAccountService : IUserAccountService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccountService(DataContext context, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            

        }

        public string GetMyEmail()
        {
            var result = string.Empty;

            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }

            if (result == null) throw new KeyNotFoundException("Email not found."); 

            return result;
        }

        public int GetMyId()
        {
            var result = 0;

            if (_httpContextAccessor.HttpContext != null)
            {
                var UserAccountId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);

                result = Convert.ToInt32(UserAccountId);

            }

            return result;
        }

        public async Task<List<UserAccountReturnDTO>> GetAll()
        {
            var userAccounts = await _context.UserAccounts.ToListAsync();

            if (userAccounts.Count == 0) throw new KeyNotFoundException("No user found.");

            var userAccountReturnDTOs = _mapper.Map<List<UserAccountReturnDTO>>(userAccounts);

            return userAccountReturnDTOs;
        }

        public async Task<UserAccountReturnDTO> GetOne(int id)
        {
            var userAccount = await _context.UserAccounts
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (userAccount == null) throw new KeyNotFoundException("User not found.");

            var userAccountReturnDTO = _mapper.Map<UserAccountReturnDTO>(userAccount);

            return userAccountReturnDTO;
        }

        public async Task<UserAccount> Register(RegisterUserAccountDTO registerUserAccountDTO)
        {
            var checkUserAccount = await _context.UserAccounts
                .Where(u => u.Email == registerUserAccountDTO.Email)
                .FirstOrDefaultAsync();

            if (checkUserAccount != null) throw new InvalidDataException("Email is already used.");

            CreatePasswordHash(registerUserAccountDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            UserAccount userAccount = new()
            {
                FirstName = registerUserAccountDTO.FirstName,
                LastName = registerUserAccountDTO.LastName,
                Email = registerUserAccountDTO.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt

            };

            _context.UserAccounts.Add(userAccount);

            await _context.SaveChangesAsync();

            return userAccount;
            
        }

        public async Task<LoginReturnDTO> Login(LoginUserAccountDTO loginUserAccountDTO)
        {
            var userAccount = await _context.UserAccounts
                .Where(u => u.Email == loginUserAccountDTO.Email)
                .FirstOrDefaultAsync();

            if (userAccount == null) throw new KeyNotFoundException("User not found.");

            if (!VerifyPasswordHash(loginUserAccountDTO.Password, userAccount.PasswordHash, userAccount.PasswordSalt))
            {
                throw new InvalidCredentialException ("Wrong password.");
            }

            string token = CreateToken(userAccount);

            var loginReturnDTO = new LoginReturnDTO
            {
                token = token,
                userAccount = userAccount
            };

            return loginReturnDTO;
        }

        public async Task<UserAccount> Update(UpdateUserAccountDTO updateUserAccountDTO)
        {
            int userAccountId = GetMyId();
            var userAccount = await _context.UserAccounts
                .Where(u => u.Id == userAccountId)
                .FirstOrDefaultAsync();

            if (userAccount == null) throw new KeyNotFoundException("User not found.");

            var checkUserAccount = await _context.UserAccounts
                .Where(u => u.Email == updateUserAccountDTO.Email)
                .FirstOrDefaultAsync();

            if (checkUserAccount != null) throw new InvalidDataException("Email is already used.");

            userAccount.FirstName = updateUserAccountDTO.FirstName;
            userAccount.LastName = updateUserAccountDTO.LastName;
            userAccount.Email = updateUserAccountDTO.Email;
            userAccount.LastUpdatedDateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return userAccount;
        }

        public async Task<UserAccount> UpdatePassword(string oldPassword, string newPassword)
        {
            int userAccountId = GetMyId();
            var userAccount = await _context.UserAccounts
                .Where(u => u.Id == userAccountId)
                .FirstOrDefaultAsync();

            if (userAccount == null) throw new KeyNotFoundException("User not found.");

            if (!VerifyPasswordHash(oldPassword, userAccount.PasswordHash, userAccount.PasswordSalt))
            {
                throw new InvalidCredentialException("Wrong password.");
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            userAccount.PasswordHash = passwordHash;
            userAccount.PasswordSalt = passwordSalt;
            userAccount.LastUpdatedDateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return userAccount;
        }

        public async Task<bool> CheckPassword(string password)
        {
            int userAccountId = GetMyId();
            var userAccount = await _context.UserAccounts
                .Where(u => u.Id == userAccountId)
                .FirstOrDefaultAsync();

            if (userAccount == null) throw new KeyNotFoundException("User not found.");

            if (!VerifyPasswordHash(password, userAccount.PasswordHash, userAccount.PasswordSalt))
            {
                throw new InvalidCredentialException("Wrong password.");
            }

            return true;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);

        }

        private string CreateToken(UserAccount userAccount)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, (userAccount.FirstName + " " + userAccount.LastName).ToString()),
                new Claim(ClaimTypes.Email, userAccount.Email.ToString()),
                new Claim(ClaimTypes.Sid, userAccount.Id.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(2),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Blog_App.DTO.UserAccounts;
using Microsoft.AspNetCore.Mvc;
using Blog_App.Services.UserAccounts;
using System.Security.Authentication;

namespace Blog_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IUserAccountService _userAccountService;

        public UserAccountController (DataContext context, IUserAccountService userAccountService)
        {
            this.context = context;

            _userAccountService = userAccountService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserAccountReturnDTO>>> GetAll()
        {
            try
            {
                var userAccountReturnDTOs = await _userAccountService.GetAll();
                return Ok(userAccountReturnDTOs);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserAccountReturnDTO>> GetOne(int id)
        {
            try
            {
                var userAccountReturnDTO = await _userAccountService.GetOne(id);
                return Ok(userAccountReturnDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserAccount>> Register(RegisterUserAccountDTO registerUserAccountDTO)
        {
            try
            {
                var userAccount = await _userAccountService.Register(registerUserAccountDTO);
                return Ok(userAccount);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginReturnDTO>> Login(LoginUserAccountDTO loginUserAccountDTO)
        {
            try
            {
                var loginReturnDTO = await _userAccountService.Login(loginUserAccountDTO);
                return Ok(loginReturnDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidCredentialException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult<UserAccount>> Update(UpdateUserAccountDTO updateUserAccountDTO)
        {
            try
            {
                var userAccount = await _userAccountService.Update(updateUserAccountDTO);
                return Ok(userAccount);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("update-password")]
        [Authorize]
        public async Task<ActionResult<UserAccount>> UpdatePassword(string oldPassword, string newPassword)
        {
            try
            {
                var userAccount = await _userAccountService.UpdatePassword(oldPassword, newPassword);
                return Ok(userAccount);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidCredentialException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("check-password")]
        [Authorize]
        public async Task<ActionResult<bool>> CheckPassword(string password)
        {
            try
            {
                var checkPassword = await _userAccountService.CheckPassword(password);
                return Ok(checkPassword);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidCredentialException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
    }
}

using Blog_App.DTO.Blogs;
using Blog_App.Services.Blogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BlogReturnDTO>>> GetAll()
        {
            try
            {
                var blogs = await _blogService.GetAll();
                return Ok(blogs);
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
        public async Task<ActionResult<BlogReturnDTO>> GetOne(int id)
        {
            try
            {
                var blog = await _blogService.GetOne(id);
                return Ok(blog);
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

        
        [HttpGet("user/{userAccountId}")]
        public async Task<ActionResult<List<BlogReturnDTO>>> GetByUser(int userAccountId)
        {
            try
            {
                var blogs = await _blogService.GetByUser(userAccountId);
                return Ok(blogs);
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

        [HttpGet("tag/{tagId}")]
        public async Task<ActionResult<List<BlogReturnDTO>>> GetByTag(int tagId)
        {
            try
            {
                var blogs = await _blogService.GetByTag(tagId);
                return Ok(blogs);
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

        [HttpPost("create")]
        public async Task<ActionResult<Blog>> Create(CreateBlogDTO createBlogDTO)
        {
            try
            {
                var blog = await _blogService.Create(createBlogDTO);
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<Blog>> Update(UpdateBlogDTO updateBlogDTO)
        {
            try
            {
                var blog = await _blogService.Update(updateBlogDTO);
                return Ok(blog);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("update-tags")]
        public async Task<ActionResult<Blog>> UpdateTags(UpdateBlogTagsDTO updateBlogTagsDTO)
        {
            try
            {
                var blog = await _blogService.UpdateTags(updateBlogTagsDTO);
                return Ok(blog);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _blogService.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
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

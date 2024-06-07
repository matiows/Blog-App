using Blog_App.DTO.Blogs;

namespace Blog_App.Services.Blogs
{
    public interface IBlogService
    {
        public Task<List<BlogReturnDTO>> GetAll();

        public Task<BlogReturnDTO> GetOne(int id);

        public Task<List<BlogReturnDTO>> GetByUser(int userAccountId);
        
        public Task<List<BlogReturnDTO>> GetByTag(int tagId);

        public Task<Blog> Create(CreateBlogDTO createBlogDTO);

        public Task<Blog> Update(UpdateBlogDTO updateBlogDTO);

        public Task<Blog> UpdateTags(UpdateBlogTagsDTO updateBlogTagsDTO);

        public Task Delete(int id);
    }
}

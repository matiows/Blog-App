using AutoMapper;
using Blog_App.DTO.Blogs;
using Blog_App.DTO.UserAccounts;
using Blog_App.Models;
using Blog_App.Services.UserAccounts;

namespace Blog_App.Services.Blogs
{
    public class BlogService : IBlogService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccountService _userAccountService;

        public BlogService(DataContext context, IMapper mapper, IUserAccountService userAccountService)
        {
            _context = context;
            _mapper = mapper;
            _userAccountService = userAccountService;
        }

        public async Task<List<BlogReturnDTO>> GetAll()
        {
            var blogs = await _context.Blogs
                .Include(b => b.Tags)
                .Include(b =>b.UserAccount)
                .ToListAsync();

            if (blogs == null) throw new KeyNotFoundException("No blog found.");

            var blogReturnDTOs = _mapper.Map<List<BlogReturnDTO>>(blogs);

            return blogReturnDTOs;
        }

        public async Task<BlogReturnDTO> GetOne(int id)
        {
            var blog = await _context.Blogs
               .Where(b => b.Id == id)
               .Include(b => b.Tags)
               .Include(b => b.UserAccount)
               .FirstOrDefaultAsync();

            if (blog == null) throw new KeyNotFoundException("Blog not found.");

            var blogReturnDTO = _mapper.Map<BlogReturnDTO>(blog);

            return blogReturnDTO;
        }

        public async Task<List<BlogReturnDTO>> GetByUser(int userAccountId)
        {
            var blogs = await _context.Blogs
                .Where(b => b.UserAccountId == userAccountId)
                .Include(b => b.Tags)
                .Include(b => b.UserAccount)
                .ToListAsync();

            if (blogs.Count == 0) throw new KeyNotFoundException("No blog found.");

            var blogReturnDTOs = _mapper.Map<List<BlogReturnDTO>>(blogs);

            return blogReturnDTOs;
        }

        public async Task<List<BlogReturnDTO>> GetByTag(int tagId)
        {
            var blogTags = await _context.BlogTags
                .Where(bt => bt.TagId == tagId)
                .Include(bt => bt.Blog)
                .ThenInclude(b => b.Tags)
                .Include(bt => bt.Blog)
                .ThenInclude(b => b.UserAccount)
                .ToListAsync();

            if (blogTags.Count == 0) throw new KeyNotFoundException("No blog found.");

            List<Blog> blogs= new();

            foreach (var blogTag in blogTags)
            {
                blogs.Add(blogTag.Blog);
            }

            var blogReturnDTOs = _mapper.Map<List<BlogReturnDTO>>(blogs);

            return blogReturnDTOs;
        }

        public async Task<Blog> Create(CreateBlogDTO creatBlogDTO)
        {
            List<Tag> tags = new();

            int userAccountId = _userAccountService.GetMyId();
            Blog blog = new()
            {
                Title = creatBlogDTO.Title,
                Body = creatBlogDTO.Body,
                UserAccountId = userAccountId,
                Tags = tags
            };

            _context.Blogs.Add(blog);

            await _context.SaveChangesAsync();

            tags = SerializeTags(creatBlogDTO.Tags).Result;
            
            foreach (Tag tag in tags)
            {
                BlogTag blogTag = new()
                {
                    TagId = tag.Id,
                    BlogId = blog.Id
                };
                
                blog.Tags.Add(tag);
                _context.BlogTags.Add(blogTag);
            }

            await _context.SaveChangesAsync();

            return blog;
        }

        public async Task<Blog> Update(UpdateBlogDTO updateBlogDTO)
        {
            var blog = await _context.Blogs.FindAsync(updateBlogDTO.Id);

            if (blog == null) throw new KeyNotFoundException("Blog not found.");

            if (blog.UserAccountId != _userAccountService.GetMyId()) throw new UnauthorizedAccessException("User is not author of blog.");

            blog.Title = updateBlogDTO.Title;
            blog.Body = updateBlogDTO.Body;
            blog.LastUpdatedDateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            UpdateBlogTagsDTO updateBlogTagsDTO = new()
            {
                BlogId = blog.Id,
                Tags = updateBlogDTO.Tags
            };

            blog = UpdateTags(updateBlogTagsDTO).Result;

            return blog;
        }

        public async Task<Blog> UpdateTags(UpdateBlogTagsDTO updateBlogTagsDTO)
        {
            var blog = await _context.Blogs
                .Where(b => b.Id == updateBlogTagsDTO.BlogId)
                .Include(b => b.Tags)
                .FirstOrDefaultAsync();
                
            if (blog == null) throw new KeyNotFoundException("Blog not found.");

            if (blog.UserAccountId != _userAccountService.GetMyId()) throw new UnauthorizedAccessException("User is not author of blog.");

            if (updateBlogTagsDTO.Tags == null) throw new KeyNotFoundException("No tags to add.");

            
            List<Tag> tags = new();

            tags = SerializeTags(updateBlogTagsDTO.Tags).Result;
            
            var tagIds = tags.Select(t => t.Id).ToList();

            var existingTagIds = blog.Tags.Select(t => t.Id).ToList();

            var tagsToRemove = existingTagIds.Except(tagIds).ToList();

            foreach (int tagId in tagsToRemove) 
            {
                var blogTag = await _context.BlogTags.FindAsync(updateBlogTagsDTO.BlogId, tagId);

                if (blogTag == null) continue;

                _context.BlogTags.Remove(blogTag);

            }

            var tagsToAdd = tagIds.Except(existingTagIds).ToList();

            foreach (int tagId in tagsToAdd)
            {
                BlogTag blogTag = new()
                {
                    TagId = tagId,
                    BlogId = blog.Id
                };
                _context.BlogTags.Add(blogTag);
            }

            blog.Tags = tags;

            blog.LastUpdatedDateTime = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return blog;
        }

        public async Task Delete(int id)
        {
            var blog = await _context.Blogs
                .Where(b => b.Id == id)
                .Include(b => b.Tags)
                .FirstOrDefaultAsync();

            if (blog == null) throw new KeyNotFoundException("Blog not found.");

            if (blog.UserAccountId != _userAccountService.GetMyId()) throw new UnauthorizedAccessException("User is not author of blog.");

            foreach (var tag in blog.Tags)
            {
                var blogTag = await _context.BlogTags.FindAsync(blog.Id, tag.Id);

                if (blogTag == null) continue;

                _context.BlogTags.Remove(blogTag);
            }

            await _context.SaveChangesAsync();

            _context.Blogs.Remove(blog);

            await _context.SaveChangesAsync();
        }

        private async Task<List<Tag>> SerializeTags(ICollection<CreateBlogTag> createBlogTag)
        {
            var tags = new List<Tag>();

            foreach (var blogTag in createBlogTag)
            {
                var tag = await _context.Tags.FindAsync(blogTag.TagId);

                if (tag == null || tags.Contains(tag)) continue;

                tags.Add(tag);
            }

            return tags;
        }
    }
}

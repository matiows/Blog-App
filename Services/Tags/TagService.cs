using Blog_App.DTO.Tags;
using Blog_App.Services.Blogs;
using Microsoft.AspNetCore.Mvc;

namespace Blog_App.Services.Tags
{
    public class TagService : ITagService
    {
        private readonly DataContext _context;

        public TagService(DataContext context)
        {
            _context = context; 
        }

        public async Task<List<Tag>> GetAll()
        {
            var tags = await _context.Tags.ToListAsync();

            if (tags.Count() == 0) throw new Exception("No tag found.");

            return tags;
        }
        
        public async Task<Tag> GetOne(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null) throw new KeyNotFoundException("Tag not found.");

            return tag;
        }

        public async Task<Tag> Create(CreateTagDTO createTagDTO)
        {
            var checkTag =  await _context.Tags
                .Where(t => t.Label == createTagDTO.Label)
                .FirstOrDefaultAsync();

            if (checkTag != null) throw new InvalidDataException("Tag already exists.");

            Tag tag = new()
            {
                Label = createTagDTO.Label,
                Description = createTagDTO.Description
            };

            _context.Tags.Add(tag);

            await _context.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag> Update(UpdateTagDTO updateTagDTO)
        {
            var tag = await _context.Tags.FindAsync(updateTagDTO.Id);

            if (tag == null) throw new KeyNotFoundException("Tag not found.");

            tag.Label = updateTagDTO.Label;
            tag.Description = updateTagDTO.Description;

            await _context.SaveChangesAsync();

            return tag;
        }

        public async Task Delete(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null) throw new KeyNotFoundException("Tag not found.");

            var checkBlogTag = await _context.BlogTags
                .Where(bt => bt.TagId == id)
                .FirstOrDefaultAsync();

            if (checkBlogTag != null) throw new InvalidDataException("Tag is associated with a blog.");

            _context.Tags.Remove(tag);

            await _context.SaveChangesAsync();
        }
    }
}

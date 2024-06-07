using Blog_App.DTO.Tags;

namespace Blog_App.Services.Tags
{
    public interface ITagService
    {
        public Task<List<Tag>> GetAll();

        public Task<Tag> GetOne(int id);

        public Task<Tag> Create(CreateTagDTO createTagDTO);

        public Task<Tag> Update(UpdateTagDTO updateTagDTO);

        public Task Delete(int id);
    }
}

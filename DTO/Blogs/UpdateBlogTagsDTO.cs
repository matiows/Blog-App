namespace Blog_App.DTO.Blogs
{
    public class UpdateBlogTagsDTO
    {
        public int BlogId { get; set; }

        public ICollection<CreateBlogTag>? Tags { get; set; }
    }
}

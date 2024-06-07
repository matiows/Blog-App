namespace Blog_App.DTO.Blogs
{
    public class CreateBlogDTO
    {
        public required string Title { get; set; }

        public required string Body { get; set; }

        public ICollection<CreateBlogTag>? Tags { get; set; }
    }

    public class CreateBlogTag
    {
        public int TagId { get; set; }
    }
}

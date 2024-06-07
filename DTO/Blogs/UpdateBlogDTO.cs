namespace Blog_App.DTO.Blogs
{
    public class UpdateBlogDTO
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Body { get; set; }

        public ICollection<CreateBlogTag>? Tags { get; set; }
    }

    public class UpdateBlogTag
    {
        public int TagId { get; set; }
    }
}

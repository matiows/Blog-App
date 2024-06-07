namespace Blog_App.DTO.Tags
{
    public class CreateTagDTO
    {
        public required string Label { get; set; }

        public required string? Description { get; set; }
    }
}

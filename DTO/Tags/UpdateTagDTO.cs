namespace Blog_App.DTO.Tags
{
    public class UpdateTagDTO
    {
        public int Id { get; set; }

        public required string Label { get; set; }

        public required string? Description { get; set; }
    }
}

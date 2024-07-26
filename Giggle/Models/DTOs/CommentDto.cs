namespace Giggle.Models.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

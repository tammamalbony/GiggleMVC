namespace Giggle.Models.DTOs
{
    public class BlogPostDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool Visibility { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public string CoverImage { get; set; }
    }
}

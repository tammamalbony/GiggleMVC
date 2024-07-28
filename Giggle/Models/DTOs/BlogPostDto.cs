using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Giggle.Models.DTOs
{
    [Table("blog_posts")]
    public class BlogPostDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [ForeignKey("users")]
        [Column("author_id", TypeName = "int")]
        public int AuthorId { get; set; }

        [Column("title", TypeName = "nvarchar(256)")]
        public string Title { get; set; }

        [Column("description", TypeName = "nvarchar(512)")]
        public string Description { get; set; }

        [Column("content", TypeName = "nvarchar(max)")]
        public string Content { get; set; }

        [Column("visibility", TypeName = "bit")]
        public bool Visibility { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at", TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("categories")]
        [Column("category_id", TypeName = "int")]
        public int CategoryId { get; set; }

        [Column("image", TypeName = "nvarchar(256)")]
        public string Image { get; set; }

        [Column("cover_image", TypeName = "nvarchar(256)")]
        public string CoverImage { get; set; }
    }
}

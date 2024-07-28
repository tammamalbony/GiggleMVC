using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Giggle.Models.DTOs
{
    [Table("comments")]
    public class CommentDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [ForeignKey("posts")]
        [Column("post_id", TypeName = "int")]
        public int PostId { get; set; }

        [ForeignKey("users")]
        [Column("author_id", TypeName = "int")]
        public int AuthorId { get; set; }

        [Column("content", TypeName = "nvarchar(max)")]
        public string? Content { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
    }
}

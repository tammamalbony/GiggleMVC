using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Giggle.Models.DTOs
{
    [Table("likes")]
    public class LikeDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [ForeignKey("posts")]
        [Column("post_id", TypeName = "int")]
        public int PostId { get; set; }

        [ForeignKey("users")]
        [Column("user_id", TypeName = "int")]
        public int UserId { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }
    }
}

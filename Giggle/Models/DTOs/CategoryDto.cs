using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Giggle.Models.DTOs
{
    [Table("categories")]
    public class CategoryDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Column("name", TypeName = "nvarchar(256)")]
        public string Name { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at", TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [Column("icon", TypeName = "nvarchar(256)")]
        public string Icon { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Giggle.Models.DTOs
{
    [Table("roles")]
    public class RoleDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Column("name", TypeName = "nvarchar(256)")]
        public string Name { get; set; }
    }
}

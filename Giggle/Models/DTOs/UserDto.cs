using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Giggle.Models.DTOs
{
    [Table("users")]
    public class UserDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int Id { get; set; }

        [Column("email_address", TypeName = "nvarchar(256)")]
        public string Email { get; set; }

        [Column("password_hash", TypeName = "nvarchar(512)")]
        public string Password { get; set; }

        [Column("verification_token", TypeName = "nvarchar(128)")]
        public string VerificationToken { get; set; }

        [Column("is_verified", TypeName = "bit")]
        public bool IsVerified { get; set; }

        [Column("first_name", TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [Column("last_name", TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [Column("user_name", TypeName = "nvarchar(100)")]
        public string Username { get; set; }

        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at", TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [Column("accepted_terms", TypeName = "bit")]
        public bool Terms { get; set; }

        [Column("user_role", TypeName = "nvarchar(100)")]
        public string Role { get; set; }
    }
}

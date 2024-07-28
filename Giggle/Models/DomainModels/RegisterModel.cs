using System.ComponentModel.DataAnnotations;
using Giggle.Configurations;
using Giggle.Models.DomainModels.Attributes;

namespace Giggle.Models.DomainModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "First name must be at most 50 characters long")]
        [Display(Name = "First Name")]
        [InputType(InputTypeEnum.FirstName)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long")]
        [MaxLength(50, ErrorMessage = "Last name must be at most 50 characters long")]
        [Display(Name = "Last Name")]
        [InputType(InputTypeEnum.LastName)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessage = "Username can only contain letters, numbers, and underscores")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long")]
        [MaxLength(20, ErrorMessage = "Username must be at most 20 characters long")]
        [Display(Name = "Username")]
        [InputType(InputTypeEnum.UserName)]
        [RealTimeValidation]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not a valid email address")]
        [MaxLength(100, ErrorMessage = "Email must be at most 100 characters long")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email is not in a valid format")]
        [Display(Name = "Email Address")]
        [InputType(InputTypeEnum.Email)]
        [RealTimeValidation]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Password must be at least 8 characters and include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(100, ErrorMessage = "Password must be at most 100 characters long")]
        [Display(Name = "Password")]
        [InputType(InputTypeEnum.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "PasswordRepeat is required")]
        [Compare("Password", ErrorMessage = "PasswordRepeat do not match the Password")]
        [MinLength(8, ErrorMessage = "PasswordRepeat must be at least 8 characters long")]
        [MaxLength(100, ErrorMessage = "PasswordRepeat must be at most 100 characters long")]
        [Display(Name = "Confirm Password")]
        [InputType(InputTypeEnum.RepeatPassword)]
        public string PasswordRepeat { get; set; }

        [Required(ErrorMessage = "You must accept the terms and conditions")]
        [Display(Name = "Accept Terms")]
        [InputType(InputTypeEnum.Confirm)]
        public bool Terms { get; set; }
    }
}

using Giggle.Models.DomainModels;
using Giggle.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Timers;
using Giggle.Services;
using Microsoft.AspNetCore.Components.Web;

namespace Giggle.Helpers
{
    public class RegisterHelper
    {
        private readonly UserRepository _userRepository;
        private readonly IJSRuntime _jsRuntime;
        private readonly NavigationManager _navigation;
        private readonly IUserService _userService;
        private System.Timers.Timer _debounceTimer;

        public RegisterModel RegisterModel { get; private set; } = new RegisterModel();

        public string FirstNameValidationClass => FirstNameErrorMessage == null ? "" : !string.IsNullOrEmpty(FirstNameErrorMessage) ? "is-invalid" : "is-valid";
        public string LastNameValidationClass => LastNameErrorMessage == null ? "" : !string.IsNullOrEmpty(LastNameErrorMessage) ? "is-invalid" : "is-valid";
        public string UsernameValidationClass => UsernameErrorMessage == null ? "" : !string.IsNullOrEmpty(UsernameErrorMessage) ? "is-invalid" : "is-valid";
        public string EmailValidationClass => EmailErrorMessage == null ? "" : !string.IsNullOrEmpty(EmailErrorMessage) ? "is-invalid" : "is-valid";
        public string PasswordValidationClass => PasswordErrorMessage == null ? "" : !string.IsNullOrEmpty(PasswordErrorMessage) ? "is-invalid" : "is-valid";
        public string PasswordRepeatValidationClass => PasswordRepeatErrorMessage == null ? "" : !string.IsNullOrEmpty(PasswordRepeatErrorMessage) ? "is-invalid" : "is-valid";
        public string TermsValidationClass => TermsErrorMessage == null ? "" : !string.IsNullOrEmpty(TermsErrorMessage) ? "is-invalid" : "is-valid";

        public string FirstNameErrorMessage { get; private set; }
        public string LastNameErrorMessage { get; private set; }
        public string UsernameErrorMessage { get; private set; }
        public string EmailErrorMessage { get; private set; }
        public string PasswordErrorMessage { get; private set; }
        public string PasswordRepeatErrorMessage { get; private set; }
        public string TermsErrorMessage { get; private set; }

        public bool IsFormValid => string.IsNullOrEmpty(FirstNameErrorMessage) && string.IsNullOrEmpty(LastNameErrorMessage) &&
                                   string.IsNullOrEmpty(UsernameErrorMessage) && string.IsNullOrEmpty(EmailErrorMessage) &&
                                   string.IsNullOrEmpty(PasswordErrorMessage) && string.IsNullOrEmpty(PasswordRepeatErrorMessage) &&
                                   string.IsNullOrEmpty(TermsErrorMessage);

        public RegisterHelper(UserRepository userRepository, IJSRuntime jsRuntime, NavigationManager navigation, IUserService userService)
        {
            _userRepository = userRepository;
            _jsRuntime = jsRuntime;
            _navigation = navigation;
            _userService = userService;
        }

        private void SetDebounceTimer(Func<Task> validationFunc)
        {
            _debounceTimer?.Stop();
            _debounceTimer = new System.Timers.Timer(2000) { AutoReset = false };
            _debounceTimer.Elapsed += async (sender, e) => await validationFunc();
            _debounceTimer.Start();
        }

        public void ValidateFirstName(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (string.IsNullOrEmpty(value) || value.Length < 2)
            {
                FirstNameErrorMessage = "First name must be at least 2 characters long";
            }
            else
            {
                FirstNameErrorMessage = null;
            }
        }

        public void ValidateLastName(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (string.IsNullOrEmpty(value) || value.Length < 2)
            {
                LastNameErrorMessage = "Last name must be at least 2 characters long";
            }
            else
            {
                LastNameErrorMessage = null;
            }
        }

        public void ValidatePassword(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (string.IsNullOrEmpty(value) || !new RegularExpressionAttribute(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$").IsValid(value))
            {
                PasswordErrorMessage = "Password must be at least 8 characters and include at least one uppercase letter, one lowercase letter, one number, and one special character.";
            }
            else
            {
                PasswordErrorMessage = null;
            }
        }

        public void ValidatePasswordRepeat(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (value != RegisterModel.Password)
            {
                PasswordRepeatErrorMessage = "Passwords do not match";
            }
            else
            {
                PasswordRepeatErrorMessage = null;
            }
        }

        public void ValidateTerms(ChangeEventArgs e)
        {
            var value = (bool)e.Value;
            TermsErrorMessage = !value ? "You must accept the terms and conditions" : null;
        }

        public void ValidateUsername(ChangeEventArgs e)
        {
            SetDebounceTimer(async () =>
            {
                var value = e.Value?.ToString();
                if (string.IsNullOrEmpty(value) || value.Length < 3)
                {
                    UsernameErrorMessage = "Username must be at least 3 characters long";
                }
                else if (!await _userRepository.IsUsernameUniqueAsync(value))
                {
                    UsernameErrorMessage = "Username is already taken";
                }
                else
                {
                    UsernameErrorMessage = null;
                }
            });
        }

        public void ValidateEmail(ChangeEventArgs e)
        {
            SetDebounceTimer(async () =>
            {
                var value = e.Value?.ToString();
                if (string.IsNullOrEmpty(value) || !new EmailAddressAttribute().IsValid(value))
                {
                    EmailErrorMessage = "Email is not a valid email address";
                }
                else if (!await _userRepository.IsEmailUniqueAsync(value))
                {
                    EmailErrorMessage = "Email is already taken";
                }
                else
                {
                    EmailErrorMessage = null;
                }
            });
        }

        public async Task HandleValidSubmit(string successUrl)
        {
            var result = await _userService.RegisterUserAsync(RegisterModel);
            if (result.Success)
            {
                _navigation.NavigateTo(successUrl);
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("alert", $"Registration failed: {result.ErrorMessage}");
            }
        }
    }
}

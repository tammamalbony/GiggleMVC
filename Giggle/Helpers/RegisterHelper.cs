using Giggle.Configurations;
using Giggle.Exceptions;
using Giggle.Models.DomainModels;
using Giggle.Providers;
using Giggle.Services;
using Giggle.Validators;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Giggle.Helpers
{
    public class RegisterHelper : BaseHelper<RegisterModel>
    {
        protected readonly IUserService _userService;

        private bool isIntialized { get; set; }
        public FieldValidator<RegisterModel>? FirstName { get; private set; }
        public FieldValidator<RegisterModel>? LastName { get; private set; }
        public FieldValidator<RegisterModel>? Username { get; private set; }
        public FieldValidator<RegisterModel>? Email { get; private set; }
        public FieldValidator<RegisterModel>? Password { get; private set; }
        public FieldValidator<RegisterModel>? PasswordRepeat { get; private set; }
        public FieldValidator<RegisterModel>? Terms { get; private set; }


        public RegisterHelper(IJSRuntime jsRuntime, NavigationManager navigation, IUserService userService, ILogger<RegisterModel> logger, Func<Task> updateCallBack)
            : base(jsRuntime, navigation, logger, updateCallBack)
        {
            _userService = userService;
        }

        public override void Initialize()
        {
            if (!isIntialized)
            {
                FirstName = CreateValidator(nameof(RegisterModel.FirstName), PropertyType.String, _userService);
                LastName = CreateValidator(nameof(RegisterModel.LastName), PropertyType.String, _userService);
                Username = CreateValidator(nameof(RegisterModel.Username), PropertyType.String, _userService);
                Email = CreateValidator(nameof(RegisterModel.Email), PropertyType.String, _userService);
                Password = CreateValidator(nameof(RegisterModel.Password), PropertyType.String, _userService);
                PasswordRepeat = CreateValidator(nameof(RegisterModel.PasswordRepeat), PropertyType.String, _userService);
                Terms = CreateValidator(nameof(RegisterModel.Terms), PropertyType.Bool, _userService);
                isIntialized = true;
            }
        }

        public override async Task SubmitLogic()
        {
            try
            {
                var result = await _userService.RegisterUserAsync(Model);
                SweetAlertType alertType = result.Success ? SweetAlertType.Success : SweetAlertType.Error;
                string title = result.Success ? "Success!" : "Error!";
                string message = result.Success ? "Registration successful." : $"Registration failed: {result.Message}";
                await _jsRuntime.InvokeVoidAsync("showCustomAlert", alertType.ToString().ToLower(), title, message, "/Auth/login");
            }
            catch (Exception ex)
            {
                SweetAlertType alertType =  SweetAlertType.Error;
                string title = "Error!";
                string message = $"Registration failed: {ex.Message}";
                await _jsRuntime.InvokeVoidAsync("showCustomAlert", alertType.ToString().ToLower(), title, message);
            }
           
        }

        public override bool IsFormValid =>
           (FirstName?.IsValid ?? false) &&
           (LastName?.IsValid ?? false) &&
           (Username?.IsValid ?? false) &&
           (Email?.IsValid ?? false) &&
           (Password?.IsValid ?? false) &&
           (PasswordRepeat?.IsValid ?? false) &&
           (Terms?.IsValid ?? false);
    }
}

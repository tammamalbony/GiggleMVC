using Giggle.Configurations;
using Giggle.Exceptions;
using Giggle.Models.DomainModels;
using Giggle.Models.DomainModels.Attributes;
using Giggle.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using Timer = System.Timers.Timer;

namespace Giggle.Validators
{
    public class FieldValidator<TModel> : IFieldValidator<TModel> where TModel : class, new()
    {

        public Func<TModel> GetModel { get; set; }
        private Func<string?, TModel, TModel> SetValue { get; set; }
        protected ILogger<TModel> _logger;
        protected Func<Task> _updateCallBack;
        public string DisplayName { get; private set; }

        public bool IsValid { get; private set; }
        public bool IsRequired { get; private set; }


        private object? GetValue(TModel model)
        {
            var property = typeof(TModel).GetProperty(PropertyName);
            return property?.GetValue(model);
        }

        public FieldValidator(string propertyName, IUserService userService, ILogger<TModel> logger, Func<TModel> getModel, Func<string?,TModel, TModel> setValue, Func<Task> updateCallBack)
        {
            _logger = logger;
            _updateCallBack = updateCallBack;
            SetValue = setValue;
            PropertyName = propertyName;
            GetModel = getModel;
            var property = typeof(TModel).GetProperty(PropertyName);
            if (property == null)
            {
                throw new InvalidOperationException("Property not found.");
            }
            var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
            DisplayName = displayAttribute?.Name ?? propertyName;

            var inputTypeAttribute = property.GetCustomAttributes(typeof(InputTypeAttribute), false).FirstOrDefault() as InputTypeAttribute;
            if (inputTypeAttribute == null)
            {
                throw new InvalidOperationException("InputType attribute not found.");
            }
            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            IsRequired = requiredAttribute != null;
            switch (inputTypeAttribute.InputType)
            {
                case InputTypeEnum.FirstName:
                    Validators = new() {
                        new RequiredField<TModel>(property, PropertyName),
                        new LengthConstraints<TModel>(property, PropertyName),
                    };
                    break;
                case InputTypeEnum.LastName:
                    Validators = new() {
                        new RequiredField<TModel>(property, PropertyName),
                        new LengthConstraints<TModel>(property, PropertyName),
                    };
                    break;
                case InputTypeEnum.UserName:
                    Validators = new() {
                        new RequiredField<TModel>(property, PropertyName),
                        new RegularExpression<TModel>(property, PropertyName),
                        new LengthConstraints<TModel>(property, PropertyName)
                        {
                            CheckCallBack = async (model) =>
                            {
                                var username  = GetValue(model)?.ToString();
                                if (string.IsNullOrEmpty(username))
                                {
                                  throw new Exceptions.ValidationException("Username is required.");

                                }
                                return await userService.IsUsernameUniqueAsync(username);
                            }
                        },
                    };
                    break;
                case InputTypeEnum.Email:
                    Validators = new() {
                        new RequiredField<TModel>(property, PropertyName),
                        new RegularExpression<TModel>(property, PropertyName),
                        new LengthConstraints<TModel>(property, PropertyName)
                        {
                            CheckCallBack = async (model) =>
                            {
                                var email  = GetValue(model)?.ToString();
                                if (string.IsNullOrEmpty(email))
                                {
                                   throw new Exceptions.ValidationException("Email is required.");
                                }
                                return await userService.IsEmailUniqueAsync(email);
                            }
                        },
                    };
                    break;
                case InputTypeEnum.Password:
                    Validators = new() {
                        new RequiredField<TModel>(property, PropertyName),
                        new RegularExpression<TModel>(property, PropertyName),
                        new LengthConstraints<TModel>(property, PropertyName),
                    };
                    break;
                case InputTypeEnum.RepeatPassword:
                    Validators = new() {
                        new RequiredField<TModel>(property, PropertyName),

                        new RegularExpression<TModel>(property, PropertyName),
                        new LengthConstraints<TModel>(property, PropertyName),
                        new MatchProperty<TModel>(property, PropertyName),
                    };
                    break;
                case InputTypeEnum.Confirm:
                    Validators = new() {
                        new RequiredField<TModel>(property, PropertyName),
                        new MustBeTrue<TModel>(property, PropertyName),
                    };
                    break;
                default:
                    throw new NotImplementedException("Type of Input");
            }
        }


        public bool Touched { get; private set; } = false;
        public string PropertyName { get; private set; }
        public string ErrorMessage { get; private set; } = "";

        public List<IValidator<TModel>?> Validators { get; private set; }
        // Method to mark the field as touched
        public void Touch() => Touched = true;

        // Method to update the error message and touch the field
        public void SetError(string message)
        {
            ErrorMessage = message;
            Touch();
        }

      
        public async Task Validate( Action<TModel>? defaultValidation = null )
        {
            var model = GetModel();
            Touch(); // Mark the field as touched when validation is attempted
            try
            {

                if (!IsRequired && GetValue == null)
                {

                }
                else { 
                    foreach (var validator in Validators)
                    {
                        if (validator != null)
                        {
                            await validator.Validate(model);
                        }
                    }
                }
                defaultValidation?.Invoke(model);
                SetError("");
                IsValid = true;
                await _updateCallBack.Invoke();

            }
            catch (Exceptions.ValidationException error)
            {
                SetError(error?.Message ?? "");
                IsValid = false;
            }
            catch (Exception ex)
            {
                // Handle the exception by setting an error message
                SetError("Validation failed due to an exception: " + ex.Message);
                IsValid = false;
            }
        }

        // Compute the CSS class based on the field state
        public string ValidationClass =>
            !Touched ? "" : string.IsNullOrEmpty(ErrorMessage) ? "is-valid" : "is-invalid";


        public async Task Validate(ChangeEventArgs e)
        {
            string? value = e.Value?.ToString();
            var _ = SetValue(value,GetModel());
            try
            {
                await Validate();
            }
            catch (Exception ex)
            {
                SetError($"An error occurred during {PropertyName} validation.");
                _logger.LogError($"Error in Validate {PropertyName}: ", ex);
            }
            return;
        }
    }
}

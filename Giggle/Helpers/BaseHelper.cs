using Giggle.Configurations;
using Giggle.Models.DomainModels;
using Giggle.Services;
using Giggle.Validators;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

namespace Giggle.Helpers
{
    public abstract class BaseHelper<TModel> where TModel : class, new()
    {
        protected readonly IJSRuntime _jsRuntime;
        protected readonly NavigationManager _navigation;
        protected Func<Task> _updateCallBack;
        protected ILogger<TModel> _logger;
        public List<Func<Task>> Valdiations { get; private set; }

        public TModel Model { get; private set; } = new TModel();
        public bool IsSubmitting { get; protected set; }
        public virtual bool IsFormValid  { get; protected set; }
        public bool CanSubmit => IsFormValid && !IsSubmitting;
        public BaseHelper(IJSRuntime jsRuntime, NavigationManager navigation,  ILogger<TModel> logger, Func<Task> updateCallBack)
        {
            _logger = logger;
            _updateCallBack = updateCallBack;
            _jsRuntime = jsRuntime;
            _navigation = navigation;
            Valdiations = new();
        }

       
        public abstract Task SubmitLogic();
        protected virtual async Task HandleValidSubmit()
        {
            if (!CanSubmit)
            {
                _logger.LogWarning("Attempted to submit while form is invalid or submitting.");
                return;
            }

            try
            {
                foreach (var item in Valdiations)
                {
                    await item.Invoke();
                }
                IsSubmitting = true;
                await SubmitLogic();

                _logger.LogInformation("Form submitted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during form submission.");
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        public async Task ProcessSubmit()
        {
            await ValidateModel();
            if (IsFormValid)
            {
                await HandleValidSubmit();
            }
            else
            {
                _logger.LogWarning("Form submission attempted but the model is invalid.");
            }
        }

        private Task ValidateModel()
        {
            IsFormValid = true; 

            var validationContext = new ValidationContext(Model, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            IsFormValid = Validator.TryValidateObject(Model, validationContext, validationResults, true);

            if (!IsFormValid)
            {
                foreach (var validationResult in validationResults)
                {
                    _logger.LogError($"Validation error: {validationResult.ErrorMessage}");
                }
            }
            return Task.CompletedTask;
        }



        protected FieldValidator<TModel> CreateValidator(string propertyName, PropertyType propertyType, IUserService userService)
        {
            var validator =  new FieldValidator<TModel>(
                propertyName,
                userService,
                _logger,
                () => Model,
                (value, model) => SetModelProperty(model, propertyName, value, propertyType),
                _updateCallBack
            );
            Valdiations.Add(async ()=> { await validator.Validate(); });
            return validator;
        }

        private TModel SetModelProperty(TModel model, string propertyName, string? value, PropertyType propertyType)
        {
            var propertyInfo = typeof(TModel).GetProperty(propertyName);
            if (propertyInfo == null) return model;

            switch (propertyType)
            {
                case PropertyType.Bool:
                    if (bool.TryParse(value, out bool boolValue))
                    {
                        propertyInfo.SetValue(model, boolValue);
                    }
                    break;
                case PropertyType.Int:
                    if (int.TryParse(value, out int intValue))
                    {
                        propertyInfo.SetValue(model, intValue);
                    }
                    break;
                case PropertyType.String:
                    propertyInfo.SetValue(model, value);
                    break;
            }
            return model;
        }
        public abstract void Initialize();
    }
}

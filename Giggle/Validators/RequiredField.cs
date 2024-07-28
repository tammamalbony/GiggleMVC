using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Giggle.Validators
{
    public class RequiredField<TModel> : BaseValidator<TModel>, IValidator<TModel> where TModel : class, new()
    {
        public bool IsRequired { get; private set; }

        public RequiredField(PropertyInfo property, string propertyName) :base(property,propertyName) 
        {

            var requiredAttribute = property.GetCustomAttribute<RequiredAttribute>();
            IsRequired = requiredAttribute != null;

            if (requiredAttribute == null)
            {
                throw new InvalidOperationException($"Required attribute not found on property {propertyName}.");
            }

            ErrorMessage = requiredAttribute.ErrorMessage ?? $"{DisplayName} is required.";
        }

        public override Task Validate(TModel model)
        {
            var value = GetValue(model);

            if (IsRequired && value == null)
            {
                throw new Exceptions.ValidationException(ErrorMessage);
            }

            return Task.CompletedTask;
        }

    }
}

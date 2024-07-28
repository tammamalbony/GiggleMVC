using Giggle.Exceptions;
using System.Reflection;

namespace Giggle.Validators
{
    public class MustBeTrue<TModel> : BaseValidator<TModel>, IValidator<TModel> where TModel : class, new()
    {

        public MustBeTrue(PropertyInfo property, string propertyName) : base(property, propertyName)
        {
            if (property.PropertyType != typeof(bool))
            {
                throw new InvalidOperationException($"The {propertyName} property is not a boolean type.");
            }

            ErrorMessage = $"The {DisplayName} must be checked.";
        }

        public override async Task Validate(TModel model)
        {
            var value = GetValue(model);
            if (value == null || !((bool)value))
            {
                throw new Exceptions.ValidationException(ErrorMessage);
            }

            await base.Validate(model);
        }
    }
}

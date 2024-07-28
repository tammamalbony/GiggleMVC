using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Giggle.Validators
{
    public class RegularExpression<TModel> : BaseValidator<TModel> ,  IValidator<TModel> where TModel : class, new()
    {
        public string Pattern { get; }

        public RegularExpression(PropertyInfo property, string propertyName) : base(property, propertyName)
        {
            try
            {
                var regexAttr = property?.GetCustomAttribute<RegularExpressionAttribute>();
                Pattern =  regexAttr?.Pattern ?? "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{8,}$";
                ErrorMessage =  regexAttr?.ErrorMessage ?? propertyName;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new InvalidOperationException($"Error retrieving regex pattern for {propertyName}: {ex.Message}", ex);
            }
        }

        public override async Task Validate(TModel model)
        {
            var value = GetValue(model)?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                throw new Exceptions.ValidationException($"The {PropertyName} can not be empty.");
            }

            if (Pattern != null)
            {
                var regex = new Regex(Pattern);
                if (!regex.IsMatch(value))
                {
                    throw new Exceptions.ValidationException(ErrorMessage);
                }
            }

            await base.Validate(model);
        }
    }
}

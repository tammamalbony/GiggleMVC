using Giggle.Exceptions;
using Google.Protobuf.WellKnownTypes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Giggle.Validators
{
    public class LengthConstraints<TModel>  : BaseValidator<TModel> ,  IValidator<TModel> where TModel : class, new()
    {
        public int MinLength { get; private set; }
        public int MaxLength { get; private set; }
        public override string ErrorMessage => $"{DisplayName} must be between {MinLength} and {MaxLength} characters long.";

        public LengthConstraints(PropertyInfo property, string propertyName) : base(property, propertyName)
        {
            try
            {
                var minLengthAttr = property?.GetCustomAttribute<MinLengthAttribute>();
                var maxLengthAttr = property?.GetCustomAttribute<MaxLengthAttribute>();

                MinLength = minLengthAttr?.Length ?? 0;
                MaxLength = maxLengthAttr?.Length ?? int.MaxValue;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new InvalidOperationException($"Error retrieving length constraints for {propertyName}: {ex.Message}", ex);
            }
        }

        public override async Task Validate(TModel model)
        {
            var value = GetValue(model)?.ToString(); 
            if (value == null)
            {
                throw new Exceptions.ValidationException($"The {DisplayName} cannot be empty.");
            }

            int length = value.Length;
            if (length < MinLength || length > MaxLength)
            {
                throw new Exceptions.ValidationException(ErrorMessage);
            }

            await base.Validate(model); 
        }

    }
}

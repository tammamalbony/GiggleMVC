using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Giggle.Validators
{

    public class MatchProperty<TModel> : BaseValidator<TModel>, IValidator<TModel> where TModel : class, new()
    {
        private readonly string _comparisonProperty;

        public MatchProperty(PropertyInfo property, string propertyName) : base(property, propertyName)
        {
            var compareAttribute = property.GetCustomAttribute<CompareAttribute>();
            if (compareAttribute == null)
            {
                throw new InvalidOperationException($"Compare attribute not found on property {propertyName}.");
            }

            _comparisonProperty = compareAttribute.OtherProperty;
            if (_comparisonProperty == null)
            {
                throw new InvalidOperationException($"Comparison property {_comparisonProperty} not found on model type {typeof(TModel).Name}.");
            }
            ErrorMessage = compareAttribute.ErrorMessage ?? $"The {propertyName} does not match the {_comparisonProperty}.";
        }

        public object? propertyToCompareGetValue(TModel model)
        {
            var property = typeof(TModel).GetProperty(_comparisonProperty);
            return property?.GetValue(model);
        }


        public override async Task Validate(TModel model)
        {
            object? originalPropertyValue = GetValue(model);
            var comparisonPropertyValue = propertyToCompareGetValue(model);
            if (!Equals(originalPropertyValue, comparisonPropertyValue))
            {
                throw new Exceptions.ValidationException(ErrorMessage);
            }

            await base.Validate(model);
        }
    }
}

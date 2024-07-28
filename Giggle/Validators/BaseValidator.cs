using Giggle.Models.DomainModels.Attributes;
using Giggle.Models.Results;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Linq;

namespace Giggle.Validators
{
    public class BaseValidator<TModel> where TModel : class, new()
    {
        public string DisplayName { get; private set; }
        public  virtual string? ErrorMessage { get; protected set; }
        public bool IsRealTime { get; protected set; }

        public BaseValidator(PropertyInfo property, string propertyName)
        {
            PropertyName = propertyName;

            var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
            DisplayName = displayAttribute?.Name ?? propertyName;

            var realTimeValidation = property.GetCustomAttribute<RealTimeValidationAttribute>() != null;
            if (realTimeValidation)
            {
                IsRealTime = true;
            }
        }

        public string PropertyName { get; private set; }


        public object? GetValue(TModel model)
        {
            var property = typeof(TModel).GetProperty(PropertyName);
            return property?.GetValue(model);
        }
        public Func<TModel, Task<bool>>? CheckCallBack { get; set; }
        public virtual async Task Validate(TModel model)
        {

            if (IsRealTime != false && CheckCallBack != null)
            {
                 await CheckCallBack(model);
            }
            await Task.CompletedTask;
        }
    }
}

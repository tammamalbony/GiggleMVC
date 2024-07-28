
using Microsoft.AspNetCore.Components;

namespace Giggle.Validators
{
    public interface IFieldValidator<TModel> where TModel : class, new()
    {
        string ErrorMessage { get; }
        string DisplayName { get; }
        string PropertyName { get; }
        bool Touched { get; }
        bool IsValid { get; }
        bool IsRequired { get; }
        string ValidationClass { get; }
        void SetError(string message);
        void Touch();
        Task Validate(ChangeEventArgs e);
    }
}
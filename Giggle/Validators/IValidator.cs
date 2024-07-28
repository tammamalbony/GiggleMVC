using Giggle.Models.Results;

namespace Giggle.Validators
{
    public interface IValidator<TModel> where TModel : class, new()
    {
        string PropertyName { get; }
        string DisplayName { get; }
        string? ErrorMessage { get; }
        public bool IsRealTime { get; }
        public Func<TModel, Task<bool>>? CheckCallBack { get; set; }
        Task Validate(TModel model);
       
    }
}

using Giggle.Configurations;

namespace Giggle.Models.DomainModels.Attributes
{
    public class InputTypeAttribute : Attribute
    {
        public InputTypeEnum InputType { get; }

        public InputTypeAttribute(InputTypeEnum inputType)
        {
            InputType = inputType;
        }
    }
}

namespace Giggle.Models.DomainModels.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RealTimeValidationAttribute : Attribute
    {
        public string ValidationUrl { get; }

        // Optionally, pass a URL to which real-time validation requests should be sent
        public RealTimeValidationAttribute(string validationUrl = "")
        {
            ValidationUrl = validationUrl;
        }
    }
}

using Giggle.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Giggle.Configurations
{
    public class CustomConfigurationManager

    {
        private readonly IConfiguration _configuration;

        public CustomConfigurationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string JwtIssuer => Environment.GetEnvironmentVariable("Jwt_Issuer");
        public RSA JwtPrivateKey => RsaKeyConverter.ImportPrivateKey(Environment.GetEnvironmentVariable("Jwt_PrivateKey"));
		public RSA JwtPublicKey => RsaKeyConverter.ImportPublicKey(Environment.GetEnvironmentVariable("Jwt_PublicKey"));
		public string DbHost => Environment.GetEnvironmentVariable("DB_HOST");
        public string DbName => Environment.GetEnvironmentVariable("DB_NAME");
        public string DbUsername => Environment.GetEnvironmentVariable("DB_USERNAME");
        public string DbPassword => Environment.GetEnvironmentVariable("DB_PASSWORD");
        public string DbCharset => Environment.GetEnvironmentVariable("DB_CHARSET");
    }
}

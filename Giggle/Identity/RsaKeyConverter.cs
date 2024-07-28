using System.Security.Cryptography;
using System.Text.Json;

namespace Giggle.Identity
{
    public class RsaKeyConverter
    {
        public static string ExportPrivateKey(RSA rsa)
        {
            RSAParameters parameters = rsa.ExportParameters(true);
            var key = new
            {
                Modulus = parameters.Modulus,
                Exponent = parameters.Exponent,
                P = parameters.P,
                Q = parameters.Q,
                DP = parameters.DP,
                DQ = parameters.DQ,
                InverseQ = parameters.InverseQ,
                D = parameters.D
            };
            string json = JsonSerializer.Serialize(key);
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        }

        public static string ExportPublicKey(RSA rsa)
        {
            RSAParameters parameters = rsa.ExportParameters(false);
            var key = new
            {
                Modulus = parameters.Modulus,
                Exponent = parameters.Exponent
            };
            string json = JsonSerializer.Serialize(key);
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));
        }

        public static RSA ImportPublicKey(string publicKeyBase64)
        {
            string publicKeyJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(publicKeyBase64));
            var keyParams = JsonSerializer.Deserialize<RsaPublicKey>(publicKeyJson);

            var rsa = RSA.Create();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = keyParams.Modulus,
                Exponent = keyParams.Exponent
            });

            return rsa;
        }

        public static RSA ImportPrivateKey(string privateKeyBase64)
        {
            string privateKeyJson = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(privateKeyBase64));
            var keyParams = JsonSerializer.Deserialize<RsaPrivateKey>(privateKeyJson);

            var rsa = RSA.Create();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = keyParams.Modulus,
                Exponent = keyParams.Exponent,
                P = keyParams.P,
                Q = keyParams.Q,
                DP = keyParams.DP,
                DQ = keyParams.DQ,
                InverseQ = keyParams.InverseQ,
                D = keyParams.D
            });

            return rsa;
        }


        public class RsaPublicKey
        {
            public byte[] Modulus { get; set; }
            public byte[] Exponent { get; set; }
        }

        public class RsaPrivateKey
        {
            public byte[] Modulus { get; set; }
            public byte[] Exponent { get; set; }
            public byte[] P { get; set; }
            public byte[] Q { get; set; }
            public byte[] DP { get; set; }
            public byte[] DQ { get; set; }
            public byte[] InverseQ { get; set; }
            public byte[] D { get; set; }
        }
    }
}

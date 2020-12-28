using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BitMax.Net.CoreObjects
{
    public class BitMaxAuthenticationProvider : AuthenticationProvider
    {
        private readonly HMACSHA256 encryptor;
        private readonly ArrayParametersSerialization arraySerialization;

        public BitMaxAuthenticationProvider(ApiCredentials credentials, ArrayParametersSerialization arraySerialization) : base(credentials)
        {
            if (credentials == null || credentials.Secret == null)
                throw new ArgumentException("No valid API credentials provided. Key/Secret needed.");

            encryptor = new HMACSHA256(Encoding.ASCII.GetBytes(credentials.Secret.GetString()));
            this.arraySerialization = arraySerialization;
        }

        /*
        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
        {
            if (!signed)
                return new Dictionary<string, string>();

            if (Credentials == null || Credentials.Key == null)
                throw new ArgumentException("No valid API credentials provided. Key/Secret/PassPhrase needed.");

            var key = Credentials.Key.GetString();
            var time = DateTime.UtcNow.ToUnixTimeMilliseconds().ToString();
            var signtext = $"{time}+{ExtractApiUrlPath(uri)}";
            var signature = BitMaxHelpers.Base64Encode(encryptor.ComputeHash(Encoding.UTF8.GetBytes(signtext)));

            return new Dictionary<string, string> {
                { "x-auth-key", key},
                { "x-auth-timestamp", time },
                { "x-auth-signature", signature },
            };
        }

        public static string ExtractApiUrlPath(string url)
        {
            //url = new Uri(url).LocalPath;
            //url = new Regex("/[0-9]/").Replace(url, "");
            //url = url.Replace("api/pro", "");
            //url = new Regex("/v[0-9]/").Replace(url, "");
            //url = url.Trim('/');
            //url = url.Split('/').LastOrDefault();
            //return url;

            return new Uri(url).LocalPath.Trim().Split('/').LastOrDefault();
        }
        */
    }
}
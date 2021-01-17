using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Attributes;
using Newtonsoft.Json;

namespace BitMax.Net.CoreObjects
{
    public class BitMaxSocketAuthResponse
    {
        [JsonProperty("m")]
        public string Method { get; set; }

        [JsonProperty("id")]
        public string RequestId { get; set; }
        
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("err"), JsonOptionalProperty]
        public string Error { get; set; }
    }

    public class BitMaxSocketChannelResponse<T>
    {
        [JsonProperty("m")]
        public string Method { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class BitMaxSocketBarChannelResponse<T>
    {
        [JsonProperty("m")]
        public string Method { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class BitMaxSocketAccountResponse<T>
    {
        [JsonProperty("m")]
        public string Method { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("ac"), JsonConverter(typeof(AccountTypeConverter))]
        public BitMaxAccountType AccountType { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}

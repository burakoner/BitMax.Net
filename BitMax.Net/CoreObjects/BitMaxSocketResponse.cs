using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Attributes;
using Newtonsoft.Json;

namespace BitMax.Net.CoreObjects
{
    public class BitMaxSocketCashAuthResponse
    {
        [JsonProperty("m")]
        internal string Method { get; set; }

        [JsonProperty("id")]
        internal string RequestId { get; set; }
        
        [JsonProperty("code")]
        internal int Code { get; set; }

        [JsonProperty("err"), JsonOptionalProperty]
        internal string Error { get; set; }
    }

    public class BitMaxSocketCashChannelResponse<T>
    {
        [JsonProperty("m")]
        internal string Method { get; set; }

        [JsonProperty("symbol")]
        internal string Symbol { get; set; }

        [JsonProperty("data")]
        internal T Data { get; set; }
    }

    public class BitMaxSocketCashBarChannelResponse<T>
    {
        [JsonProperty("m")]
        internal string Method { get; set; }

        [JsonProperty("s")]
        internal string Symbol { get; set; }

        [JsonProperty("data")]
        internal T Data { get; set; }
    }

    public class BitMaxSocketAccountResponse<T>
    {
        [JsonProperty("m")]
        internal string Method { get; set; }

        [JsonProperty("accountId")]
        internal string AccountId { get; set; }

        [JsonProperty("ac"), JsonConverter(typeof(CashAccountTypeConverter))]
        public BitMaxCashAccountType AccountType { get; set; }

        [JsonProperty("data")]
        internal T Data { get; set; }
    }
}

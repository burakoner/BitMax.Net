using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace BitMax.Net.CoreObjects
{
    public class BitMaxResponse
    {
        [JsonProperty("code")]
        public int ErrorCode { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore), JsonOptionalProperty]
        public string ErrorMessage { get; set; }

        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore), JsonOptionalProperty]
        public string ErrorReason { get; set; }

        [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore), JsonOptionalProperty]
        public object ErrorInformation { get; set; }
    }

    public class BitMaxApiResponse<T>: BitMaxResponse
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class BitMaxError : Error
    {
        public string Reason { get; set; }
        public string Information { get; set; }

        public BitMaxError(string message, object data = null) : base(null, message, data) { 
        }
        public BitMaxError(int code, string message, object data = null) : base(code, message, data)
        {
        }
        public BitMaxError(int code, string message,string reason,string info) : base(code, message, null)
        {
            Reason = reason;
            Information = info;
        }
        public BitMaxError(BitMaxResponse response) : base(response.ErrorCode, response.ErrorMessage, null)
        {
            Reason = response.ErrorReason;
            Information = response.ErrorInformation?.ToString();
        }

        public override string ToString()
        {
            return $"{Code}: {Message}";
        }
    }
}
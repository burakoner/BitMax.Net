using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxFuturesFundingRate
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }
        
        [JsonProperty("fundingRate")]
        public decimal FundingRate { get; set; }
    }
}

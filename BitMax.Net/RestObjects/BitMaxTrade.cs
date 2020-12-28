using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxTrade
    {
        [JsonProperty("ts"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("seqnum")]
        public long SequenceNumber { get; set; }

        [JsonProperty("p")]
        public decimal Price { get; set; }
        
        [JsonProperty("q")]
        public decimal Amount { get; set; }

        [JsonProperty("bm")]
        public bool IsBuyerMarketMaker { get; set; }

    }
}

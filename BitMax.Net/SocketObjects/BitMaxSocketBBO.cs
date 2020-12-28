using BitMax.Net.RestObjects;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.SocketObjects
{
    public class BitMaxSocketBBO
    {
        [JsonProperty("ts"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("ask")]
        public BitMaxOrderBookEntry BestAsk { get; set; }

        [JsonProperty("bid")]
        public BitMaxOrderBookEntry BestBid { get; set; }
    }
}

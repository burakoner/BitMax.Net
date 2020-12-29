using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxFuturesCancelOrder
    {
        [JsonProperty("time"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
    }
}
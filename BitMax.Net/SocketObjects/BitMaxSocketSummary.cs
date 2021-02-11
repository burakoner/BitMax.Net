using BitMax.Net.Converters;
using BitMax.Net.Enums;
using BitMax.Net.RestObjects;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.SocketObjects
{
    public class BitMaxSocketSummary
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("i"), JsonConverter(typeof(PeriodConverter))]
        public BitMaxPeriod Period { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("o")]
        public decimal Open { get; set; }

        [JsonProperty("h")]
        public decimal High { get; set; }

        [JsonProperty("l")]
        public decimal Low { get; set; }

        [JsonProperty("c")]
        public decimal Close { get; set; }

        [JsonProperty("v")]
        public decimal Volume { get; set; }
    }
}

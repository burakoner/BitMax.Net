using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxCandleSerie
    {
        [JsonProperty("m")]
        public string Message { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("data")]
        public BitMaxCandle Data { get; set; }
    }

    public class BitMaxCandle
    {
        [JsonProperty("i"), JsonConverter(typeof(PeriodConverter))]
        public BitMaxPeriod Period { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(TimestampConverter))]
        public DateTime OpenTime { get; set; }

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

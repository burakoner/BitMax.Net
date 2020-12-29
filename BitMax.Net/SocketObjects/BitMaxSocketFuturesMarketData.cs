using BitMax.Net.RestObjects;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.SocketObjects
{
    public class BitMaxSocketFuturesMarketData
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("oi")]
        public decimal OpenInterest { get; set; }

        [JsonProperty("fr")]
        public decimal FundingRate { get; set; }

        [JsonProperty("fpf")]
        public bool FundingPaymentFlag { get; set; }

        [JsonProperty("ip")]
        public decimal IndexPrice { get; set; }

        [JsonProperty("mp")]
        public decimal MarkPrice { get; set; }

        [JsonProperty("srct"), JsonConverter(typeof(TimestampConverter))]
        public DateTime SourceTime { get; set; }

        [JsonProperty("fpt"), JsonConverter(typeof(TimestampConverter))]
        public DateTime NextFundingPaymentTime { get; set; }

    }
}

using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxFuturesMarketData
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("sourceTime"), JsonConverter(typeof(TimestampConverter))]
        public DateTime SourceTime { get; set; }
        
        [JsonProperty("nextFundingPaymentTime"), JsonConverter(typeof(TimestampConverter))]
        public DateTime NextFundingPaymentTime { get; set; }

        [JsonProperty("fundingPaymentFlag")]
        public bool FundingPaymentFlag { get; set; }

        [JsonProperty("openInterest")]
        public decimal OpenInterest { get; set; }

        [JsonProperty("fundingRate")]
        public decimal FundingRate { get; set; }
        
        [JsonProperty("indexPrice")]
        public decimal IndexPrice { get; set; }
        
        [JsonProperty("markPrice")]
        public decimal MarkPrice { get; set; }
    }
}

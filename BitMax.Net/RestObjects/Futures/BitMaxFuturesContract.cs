using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxFuturesContract
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("tradingStartTime"), JsonConverter(typeof(TimestampConverter))]
        public DateTime tradingStartTime { get; set; }

        [JsonProperty("collapseDecimals")]
        public string CollapseDecimals { get; set; }

        [JsonProperty("minQty")]
        public decimal MinimumQuantity { get; set; }

        [JsonProperty("maxQty")]
        public decimal MaximumQuantity { get; set; }
        
        [JsonProperty("minNotional")]
        public decimal MinimumNotional { get; set; }
        
        [JsonProperty("maxNotional")]
        public decimal MaximumNotional { get; set; }
        
        [JsonProperty("tickSize")]
        public decimal TickSize { get; set; }
        
        [JsonProperty("lotSize")]
        public decimal LotSize { get; set; }

        [JsonProperty("statusCode"), JsonConverter(typeof(ProductStatusConverter))]
        public BitMaxProductStatus Status { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }
    }
}

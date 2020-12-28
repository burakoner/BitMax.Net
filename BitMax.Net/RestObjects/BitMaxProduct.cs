using BitMax.Net.Converters;
using BitMax.Net.Enums;
using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{
    public class BitMaxProduct
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("baseAsset")]
        public string BaseAsset { get; set; }
        
        [JsonProperty("quoteAsset")]
        public string QuoteAsset { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(ProductStatusConverter))]
        public BitMaxProductStatus Status { get; set; }
        
        [JsonProperty("minNotional")]
        public decimal MinimumNotional { get; set; }
        
        [JsonProperty("maxNotional")]
        public decimal MaximumNotional { get; set; }
        
        [JsonProperty("marginTradable")]
        public bool MarginTradable { get; set; }
        
        [JsonProperty("commissionType"), JsonConverter(typeof(ProductCommissionTypeConverter))]
        public BitMaxProductCommissionType CommissionType { get; set; }

        [JsonProperty("commissionReserveRate")]
        public decimal CommissionReserveRate { get; set; }

        [JsonProperty("tickSize")]
        public decimal TickSize { get; set; }

        [JsonProperty("lotSize")]
        public decimal LotSize { get; set; }
    }
}

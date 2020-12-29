using BitMax.Net.Converters;
using BitMax.Net.Enums;
using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{
    public class BitMaxFuturesAsset
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }
        
        [JsonProperty("assetName")]
        public string AssetName { get; set; }
        
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        
        [JsonProperty("discountFactor")]
        public decimal DiscountFactor { get; set; }
        
        [JsonProperty("conversionFactor")]
        public decimal ConversionFactor { get; set; }
        
        [JsonProperty("statusCode"), JsonConverter(typeof(AssetStatusConverter))]
        public BitMaxAssetStatus Status { get; set; }
    }
}

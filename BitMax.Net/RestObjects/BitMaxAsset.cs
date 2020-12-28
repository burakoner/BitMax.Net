using BitMax.Net.Converters;
using BitMax.Net.Enums;
using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{
    public class BitMaxAsset
    {
        [JsonProperty("assetCode")]
        public string AssetCode { get; set; }
        
        [JsonProperty("assetName")]
        public string AssetName { get; set; }
        
        [JsonProperty("precisionScale")]
        public int PrecisionScale { get; set; }
        
        [JsonProperty("nativeScale")]
        public int NativeScale { get; set; }
        
        [JsonProperty("withdrawalFee")]
        public decimal WithdrawalFee { get; set; }
        
        [JsonProperty("minWithdrawalAmt")]
        public decimal MinimumWithdrawalAmount { get; set; }
        
        [JsonProperty("status"), JsonConverter(typeof(AssetStatusConverter))]
        public BitMaxAssetStatus Status { get; set; }
    }
}

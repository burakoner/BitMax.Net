using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{

    public class BitMaxFuturesBalance
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("totalBalance")]
        public decimal TotalBalance { get; set; }
        
        [JsonProperty("availableBalance")]
        public decimal AvailableBalance { get; set; }
        
        [JsonProperty("maxTransferrable")]
        public decimal MaxTransferrable { get; set; }
        
        [JsonProperty("priceInUSDT")]
        public decimal PriceInUSDT { get; set; }
        
    }
}

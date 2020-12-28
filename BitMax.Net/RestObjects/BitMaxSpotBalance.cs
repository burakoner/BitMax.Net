using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{

    public class BitMaxSpotBalance
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("totalBalance")]
        public decimal TotalBalance { get; set; }
        
        [JsonProperty("availableBalance")]
        public decimal AvailableBalance { get; set; }
        
    }
}

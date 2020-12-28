using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{

    public class BitMaxMarginBalance
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("totalBalance")]
        public decimal TotalBalance { get; set; }
        
        [JsonProperty("availableBalance")]
        public decimal AvailableBalance { get; set; }
        
        [JsonProperty("borrowed")]
        public decimal Borrowed { get; set; }
        
        [JsonProperty("interest")]
        public decimal Interest { get; set; }
        
    }
}

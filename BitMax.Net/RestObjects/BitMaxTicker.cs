using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{
    public class BitMaxTicker
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("open")]
        public decimal open { get; set; }
        
        [JsonProperty("close")]
        public decimal close { get; set; }
        
        [JsonProperty("high")]
        public decimal high { get; set; }
        
        [JsonProperty("low")]
        public decimal low { get; set; }
        
        [JsonProperty("volume")]
        public decimal volume { get; set; }
        
        [JsonProperty("ask")]
        public BitMaxOrderBookEntry Ask { get; set; }
        
        [JsonProperty("bid")]
        public BitMaxOrderBookEntry Bid { get; set; }
        
    }
}

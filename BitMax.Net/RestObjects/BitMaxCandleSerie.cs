using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{
    public class BitMaxCandleSerie
    {
        [JsonProperty("m")]
        public string Message { get; set; }
        
        [JsonProperty("s")]
        public string Symbol { get; set; }
        
        [JsonProperty("data")]
        public BitMaxCandle Data { get; set; }
    }
}

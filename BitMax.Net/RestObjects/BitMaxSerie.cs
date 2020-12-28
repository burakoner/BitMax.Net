using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{

    public class BitMaxSerie<T>
    {
        [JsonProperty("m")]
        public string Message { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}

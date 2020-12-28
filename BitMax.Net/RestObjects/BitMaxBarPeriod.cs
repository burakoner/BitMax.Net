using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{
    public class BitMaxBarPeriod
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("intervalInMillis")]
        public long IntervalInMillis { get; set; }
        
        public long IntervalInSeconds { get { return IntervalInMillis / 1000; } }
    }
}

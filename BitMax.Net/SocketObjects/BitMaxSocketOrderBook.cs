using BitMax.Net.RestObjects;
using Newtonsoft.Json;

namespace BitMax.Net.SocketObjects
{
    public class BitMaxSocketOrderBook: BitMaxOrderBook
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}

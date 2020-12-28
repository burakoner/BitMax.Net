using BitMax.Net.RestObjects;
using Newtonsoft.Json;

namespace BitMax.Net.SocketObjects
{
    public class BitMaxSocketTrade: BitMaxTrade
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}

using BitMax.Net.RestObjects;
using Newtonsoft.Json;

namespace BitMax.Net.SocketObjects
{
    public class BitMaxSocketTrade: BitMaxCashTrade
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}

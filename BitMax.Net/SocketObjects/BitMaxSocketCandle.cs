using BitMax.Net.RestObjects;
using Newtonsoft.Json;

namespace BitMax.Net.SocketObjects
{
    public class BitMaxSocketCandle : BitMaxCandle
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}

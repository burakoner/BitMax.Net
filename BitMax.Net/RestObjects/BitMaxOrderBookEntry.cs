using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{
    [JsonConverter(typeof(ArrayConverter))]
    public class BitMaxOrderBookEntry
    {
        [ArrayProperty(0)]
        public decimal Price { get; set; }

        [ArrayProperty(1)]
        public decimal Quantity { get; set; }

    }
}

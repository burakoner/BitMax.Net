using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BitMax.Net.RestObjects
{
    public class BitMaxOrderBook
    {
        [JsonProperty("ts"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("seqnum")]
        public long SequenceNumber { get; set; }

        [JsonProperty("asks")]
        public IEnumerable<BitMaxOrderBookEntry> Asks { get; set; }

        [JsonProperty("bids")]
        public IEnumerable<BitMaxOrderBookEntry> Bids { get; set; }

    }
}

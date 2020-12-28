using Newtonsoft.Json;
using System.Collections.Generic;

namespace BitMax.Net.RestObjects
{
    public class BitMaxDepositAddress
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }
        
        [JsonProperty("assetName")]
        public string AssetName { get; set; }
        
        [JsonProperty("address")]
        public IEnumerable<BitMaxBlockchainAddress> Addresses { get; set; }
    }

    public class BitMaxBlockchainAddress
    {
        [JsonProperty("blockchain")]
        public string Blockchain { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("destTag")]
        public string Tag { get; set; }
    }
}

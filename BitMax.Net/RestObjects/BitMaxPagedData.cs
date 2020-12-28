using Newtonsoft.Json;
using System.Collections.Generic;

namespace BitMax.Net.RestObjects
{

    public class BitMaxPagedData<T>
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        
        [JsonProperty("hasNext")]
        public bool HasNext { get; set; }
        
        [JsonProperty("data")]
        public IEnumerable< T> Data { get; set; }
    }
}

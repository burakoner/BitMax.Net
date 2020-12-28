using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BitMax.Net.RestObjects
{
    public class BitMaxAccountInfo
    {
        [JsonProperty("accountGroup")]
        public int AccountGroup { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("expireTime"), JsonConverter(typeof(TimestampConverter))]
        public DateTime ExpireTime { get; set; }
        
        [JsonProperty("allowedIps")]
        public IEnumerable<string> AllowedIps { get; set; }
        
        [JsonProperty("cashAccount")]
        public IEnumerable<string> CashAccounts { get; set; }
        
        [JsonProperty("marginAccount")]
        public IEnumerable<string> MarginAccounts { get; set; }
        
        [JsonProperty("futuresAccount")]
        public IEnumerable<string> FuturesAccounts { get; set; }
        
        [JsonProperty("userUID")]
        public string UserUID { get; set; }
        
        [JsonProperty("tradePermission")]
        public bool TradePermission { get; set; }
        
        [JsonProperty("transferPermission")]
        public bool TransferPermission { get; set; }
        
        [JsonProperty("viewPermission")]
        public bool ViewPermission { get; set; }
        
        [JsonProperty("limitQuota")]
        public int LimitQuota { get; set; }
    }
}

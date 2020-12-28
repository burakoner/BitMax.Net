using BitMax.Net.Converters;
using BitMax.Net.Enums;
using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{
    public class BitMaxSocketSpotBalance
    {
        [JsonProperty("a")]
        public string Asset { get; set; }

        [JsonProperty("sn")]
        public long SequenceNumber { get; set; }

        [JsonProperty("tb")]
        public decimal TotalBalance { get; set; }

        [JsonProperty("ab")]
        public decimal AvailableBalance { get; set; }
    }

    public class BitMaxSocketSpotBalanceExt : BitMaxSocketSpotBalance
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("ac"), JsonConverter(typeof(CashAccountTypeConverter))]
        public BitMaxCashAccountType AccountType { get; set; }
    }

    public class BitMaxSocketMarginBalance
    {
        [JsonProperty("a")]
        public string Asset { get; set; }

        [JsonProperty("sn")]
        public long SequenceNumber { get; set; }

        [JsonProperty("tb")]
        public decimal TotalBalance { get; set; }

        [JsonProperty("ab")]
        public decimal AvailableBalance { get; set; }

        [JsonProperty("brw")]
        public decimal BorrowedAmount { get; set; }

        [JsonProperty("int")]
        public decimal InterestAmount { get; set; }
    }

    public class BitMaxSocketMarginBalanceExt : BitMaxSocketMarginBalance
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("ac"), JsonConverter(typeof(CashAccountTypeConverter))]
        public BitMaxCashAccountType AccountType { get; set; }
    }
}
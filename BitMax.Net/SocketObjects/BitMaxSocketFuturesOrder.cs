using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxSocketFuturesOrderExt : BitMaxSocketFuturesOrder
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("ac"), JsonConverter(typeof(AccountTypeConverter))]
        public BitMaxAccountType AccountType { get; set; }
    }

    public class BitMaxSocketFuturesOrder
    {
        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("sn")]
        public long SequenceNumber { get; set; }

        [JsonProperty("ap")]
        public decimal? AveragePrice { get; set; }

        [JsonProperty("t"), JsonConverter(typeof(TimestampConverter))]
        public DateTime LastExecutionTime { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("ot"), JsonConverter(typeof(FuturesOrderTypeConverter))]
        public BitMaxFuturesOrderType OrderType { get; set; }

        [JsonProperty("p")]
        public decimal? Price { get; set; }

        [JsonProperty("q")]
        public decimal? Quantity { get; set; }

        [JsonProperty("sd"), JsonConverter(typeof(OrderSideConverter))]
        public BitMaxOrderSide Side { get; set; }

        [JsonProperty("st"), JsonConverter(typeof(FuturesOrderStatusConverter))]
        public BitMaxFuturesOrderStatus Status { get; set; }

        [JsonProperty("cf")]
        public decimal CumulativeCommission { get; set; }

        [JsonProperty("cfq")]
        public decimal CumulativeFilledQuantity { get; set; }

        [JsonProperty("sp")]
        public decimal? StopPrice { get; set; }

        [JsonProperty("err")]
        public string Error { get; set; }

        [JsonProperty("fa")]
        public string FeeAsset { get; set; }

        [JsonProperty("ei")]
        public string ExecutionInstruction { get; set; }

        [JsonProperty("lq")]
        public decimal? LastFilledQuantity { get; set; }

        [JsonProperty("lp")]
        public decimal? LastFilledPrice { get; set; }
        
        [JsonProperty("lf")]
        public decimal? LastFilledFee { get; set; }
        
        [JsonProperty("pos")]
        public decimal? ContractPosition  { get; set; }
        
        [JsonProperty("rc")]
        public decimal? ReferenceCost  { get; set; }
    }
}
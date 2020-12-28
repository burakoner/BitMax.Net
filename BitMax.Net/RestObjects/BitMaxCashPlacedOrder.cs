using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxCashPlacedOrder<T>
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("ac"), JsonConverter(typeof(CashAccountTypeConverter))]
        public BitMaxCashAccountType AccountType { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(CashOrderResponseInstructionConverter))]
        public BitMaxCashOrderResponseInstruction Status { get; set; }
        
        [JsonProperty("info")]
        public T Info { get; set; }
    }

    public class BitMaxCashPlacedOrderInfoAccept
    {
        [JsonProperty("id")]
        public string ClientOrderId { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("orderType"), JsonConverter(typeof(CashOrderTypeConverter))]
        public BitMaxCashOrderType OrderType { get; set; }

        [JsonProperty("avgPx")]
        public decimal AveragePrice { get; set; }

        [JsonProperty("cumFee")]
        public decimal CumulativeFee { get; set; }

        [JsonProperty("cumFilledQty")]
        public decimal CumulativeFilledQuantity { get; set; }

        [JsonProperty("feeAsset")]
        public string FeeAsset { get; set; }

        [JsonProperty("lastExecTime"), JsonConverter(typeof(TimestampConverter))]
        public DateTime LastExecutionTime { get; set; }

        [JsonProperty("orderQty")]
        public decimal OrderQuantity { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("seqNum")]
        public long SequenceNumber { get; set; }

        [JsonProperty("side"), JsonConverter(typeof(CashOrderSideConverter))]
        public BitMaxCashOrderSide Side { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(CashOrderStatusConverter))]
        public BitMaxCashOrderStatus Status { get; set; }

        [JsonProperty("stopPrice")]
        public decimal? StopPrice { get; set; }

        [JsonProperty("execInst")]
        public string ExecutionInstruction { get; set; }
    }

    public class BitMaxCashPlacedOrderInfoAck
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("orderType"), JsonConverter(typeof(CashOrderTypeConverter))]
        public BitMaxCashOrderType? OrderType { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Timestamp { get; set; }
    }
}
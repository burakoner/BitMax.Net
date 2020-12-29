using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BitMax.Net.RestObjects
{
    public class BitMaxFuturesPlacedOrder<T>
    {
        [JsonProperty("code"), JsonOptionalProperty]
        public int? ErrorCode { get; set; }

        [JsonProperty("message"), JsonOptionalProperty]
        public string ErrorMessage { get; set; }

        [JsonProperty("reason"), JsonOptionalProperty]
        public string ErrorReason { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("ac"), JsonConverter(typeof(AccountTypeConverter))]
        public BitMaxAccountType AccountType { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(OrderResponseInstructionConverter))]
        public BitMaxOrderResponseInstruction Status { get; set; }

        [JsonProperty("info")]
        public T Info { get; set; }
    }

    public class BitMaxFuturesPlacedOrderInfoAccept
    {
        [JsonProperty("code"), JsonOptionalProperty]
        public int? ErrorCode { get; set; }

        [JsonProperty("message"), JsonOptionalProperty]
        public string ErrorMessage { get; set; }

        [JsonProperty("reason"), JsonOptionalProperty]
        public string ErrorReason { get; set; }

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

        [JsonProperty("side"), JsonConverter(typeof(OrderSideConverter))]
        public BitMaxOrderSide Side { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(FuturesOrderStatusConverter))]
        public BitMaxFuturesOrderStatus Status { get; set; }

        [JsonProperty("stopPrice")]
        public decimal? StopPrice { get; set; }

        [JsonProperty("execInst")]
        public string ExecutionInstruction { get; set; }
    }

    public class BitMaxFuturesPlacedOrderInfoAck
    {
        [JsonProperty("code"), JsonOptionalProperty]
        public int? ErrorCode { get; set; }

        [JsonProperty("message"), JsonOptionalProperty]
        public string ErrorMessage { get; set; }

        [JsonProperty("reason"), JsonOptionalProperty]
        public string ErrorReason { get; set; }

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
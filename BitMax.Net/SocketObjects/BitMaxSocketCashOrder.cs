using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxSocketCashOrderExt : BitMaxSocketCashOrder
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("ac"), JsonConverter(typeof(AccountTypeConverter))]
        public BitMaxAccountType AccountType { get; set; }
    }

    public class BitMaxSocketCashOrder
    {
        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("sn")]
        public long SequenceNumber { get; set; }

        [JsonProperty("ap")]
        public decimal? AveragePrice { get; set; }

        [JsonProperty("bab")]
        public decimal? BaseAssetAvailableBalance { get; set; }

        [JsonProperty("btb")]
        public decimal? BaseAssetTotalBalance { get; set; }

        [JsonProperty("qab")]
        public decimal? QuoteAssetAvailableBalance { get; set; }

        [JsonProperty("qtb")]
        public decimal? QuoteAssetTotalBalance { get; set; }

        [JsonProperty("cf")]
        public decimal CumulativeCommission { get; set; }

        [JsonProperty("cfq")]
        public decimal CumulativeFilledQuantity { get; set; }

        [JsonProperty("err")]
        public string Error { get; set; }

        [JsonProperty("fa")]
        public string FeeAsset { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("ot"), JsonConverter(typeof(CashOrderTypeConverter))]
        public BitMaxCashOrderType OrderType { get; set; }

        [JsonProperty("p")]
        public decimal? Price { get; set; }

        [JsonProperty("q")]
        public decimal? Quantity { get; set; }

        [JsonProperty("sd"), JsonConverter(typeof(OrderSideConverter))]
        public BitMaxOrderSide Side { get; set; }

        [JsonProperty("sp")]
        public decimal? StopPrice { get; set; }

        [JsonProperty("st"), JsonConverter(typeof(CashOrderStatusConverter))]
        public BitMaxCashOrderStatus Status { get; set; }

        [JsonProperty("t"), JsonConverter(typeof(TimestampConverter))]
        public DateTime LastExecutionTime { get; set; }

        [JsonProperty("ei")]
        public string ExecutionInstruction { get; set; }
    }
}
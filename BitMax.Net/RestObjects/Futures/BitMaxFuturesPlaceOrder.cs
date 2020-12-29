using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxFuturesPlaceOrder
    {
        [JsonProperty("time"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientOrderId { get; set; }

        [JsonProperty("orderType"), JsonConverter(typeof(FuturesOrderTypeConverter))]
        public BitMaxFuturesOrderType OrderType { get; set; }
        
        [JsonProperty("side"), JsonConverter(typeof(OrderSideConverter))]
        public BitMaxOrderSide OrderSide { get; set; }

        [JsonProperty("orderQty"), JsonConverter(typeof(DecimalFormatConverter))]
        public decimal Size { get; set; }

        //[JsonProperty("respInst"), JsonConverter(typeof(OrderResponseInstructionConverter))]
        //public BitMaxOrderResponseInstruction ResponseInstruction { get; set; }

        [JsonProperty("orderPrice", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(DecimalFormatConverter))]
        public decimal? OrderPrice { get; set; }
        
        [JsonProperty("stopPrice", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(DecimalFormatConverter))]
        public decimal? StopPrice { get; set; }
        
        [JsonProperty("postOnly", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PostOnly { get; set; }

        [JsonProperty("timeInForce", NullValueHandling = NullValueHandling.Ignore), JsonConverter(typeof(FuturesOrderTimeInForceConverter))]
        public BitMaxFuturesOrderTimeInForce? TimeInForce { get; set; }
    }
}
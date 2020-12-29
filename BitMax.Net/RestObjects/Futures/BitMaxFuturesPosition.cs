using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxFuturesPosition
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("position")]
        public decimal Position { get; set; }

        [JsonProperty("referenceCost")]
        public decimal ReferenceCost { get; set; }

        [JsonProperty("positionNotional")]
        public decimal PositionNotional { get; set; }

        [JsonProperty("breakevenPrice")]
        public decimal BreakevenPrice { get; set; }

        [JsonProperty("estLiquidationPrice")]
        public decimal EstimatedLiquidationPrice { get; set; }

        [JsonProperty("positionPnl")]
        public decimal PositionPnl { get; set; }

        [JsonProperty("collateralInUse")]
        public decimal CollateralInUse { get; set; }

        /// <summary>
        /// maximum quantity allowed to buy (deprecated, use maxBuyNotional)
        /// </summary>
        [JsonProperty("maxBuyOrderSize")]
        public decimal MaximumBuyOrderSize { get; set; }

        /// <summary>
        /// maximum quantity allowed to sell (deprecated, use maxSellNotional)
        /// </summary>
        [JsonProperty("maxSellOrderSize")]
        public decimal MaximumSellOrderSize { get; set; }

        [JsonProperty("maxBuyNotional")]
        public decimal MaximumBuyNotional { get; set; }

        [JsonProperty("maxSellNotional")]
        public decimal MaximumSellNotional { get; set; }

        [JsonProperty("indexPrice")]
        public decimal IndexPrice { get; set; }

        [JsonProperty("markPrice")]
        public decimal MarkPrice { get; set; }

        [JsonProperty("avgOpenPrice")]
        public decimal AverageOpenPrice { get; set; }

        [JsonProperty("effInitMarginRate")]
        public decimal EffectiveInitMarginRate { get; set; }

        [JsonProperty("overallPnl")]
        public decimal OverallPnl { get; set; }
    }
}

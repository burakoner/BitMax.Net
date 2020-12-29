using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxFuturesRisk
    {
        [JsonProperty("collateral")]
        public decimal Collateral { get; set; }

        [JsonProperty("effectiveCollateral")]
        public decimal EffectiveCollateral { get; set; }

        [JsonProperty("collateralInUse")]
        public decimal CollateralInUse { get; set; }

        [JsonProperty("freeCollateral")]
        public decimal FreeCollateral { get; set; }

        [JsonProperty("walletBalance")]
        public decimal WalletBalance { get; set; }

        [JsonProperty("accountValue")]
        public decimal AccountValue { get; set; }

        [JsonProperty("positionNotional")]
        public decimal PositionNotional { get; set; }

        [JsonProperty("effectivePositionNotional")]
        public decimal EffectivePositionNotional { get; set; }

        [JsonProperty("currentLeverage")]
        public decimal? CurrentLeverage { get; set; }

        [JsonProperty("accountMarginRate")]
        public decimal? AccountMarginRate { get; set; }

        [JsonProperty("takeoverMarginRate")]
        public decimal TakeoverMarginRate { get; set; }

        [JsonProperty("effectiveMaintenanceMarginRate")]
        public decimal EffectiveMaintenanceMarginRate { get; set; }

        [JsonProperty("effectiveInitialMarginRate")]
        public decimal EffectiveInitialMarginRate { get; set; }

        [JsonProperty("accountMaxLeverage")]
        public decimal AccountMaxLeverage { get; set; }

        [JsonProperty("unrealizedPnl")]
        public decimal UnrealizedPnl { get; set; }

        [JsonProperty("realizedPnl")]
        public decimal RealizedPnl { get; set; }

        [JsonProperty("positionPnl")]
        public decimal PositionPnl { get; set; }
    }
}

using Newtonsoft.Json;

namespace BitMax.Net.RestObjects
{
    public class BitMaxMarginRisk
    {
        [JsonProperty("accountMaxLeverage")]
        public decimal AccountMaxLeverage { get; set; }

        [JsonProperty("availableBalanceInUSDT")]
        public decimal AvailableBalanceInUSDT { get; set; }
        
        [JsonProperty("totalBalanceInUSDT")]
        public decimal TotalBalanceInUSDT { get; set; }
        
        [JsonProperty("totalBorrowedInUSDT")]
        public decimal TotalBorrowedInUSDT { get; set; }
        
        [JsonProperty("totalInterestInUSDT")]
        public decimal TotalInterestInUSDT { get; set; }
        
        [JsonProperty("netBalanceInUSDT")]
        public decimal NetBalanceInUSDT { get; set; }
        
        [JsonProperty("pointsBalance")]
        public decimal PointsBalance { get; set; }
        
        [JsonProperty("currentLeverage")]
        public decimal CurrentLeverage { get; set; }
        
        [JsonProperty("cushion")]
        public decimal Cushion { get; set; }
    }
}

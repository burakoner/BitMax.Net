using BitMax.Net.Converters;
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;

namespace BitMax.Net.RestObjects
{
    public class BitMaxTransaction
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("commission")]
        public decimal Commission { get; set; }

        [JsonProperty("destAddress")]
        public BitMaxDestinationAddress DestinationAddress { get; set; }

        [JsonProperty("networkTransactionId")]
        public string NetworkTransactionId { get; set; }

        [JsonProperty("numConfirmations")]
        public int NumberOfConfirmations { get; set; }

        [JsonProperty("numConfirmed")]
        public int NumberOfConfirmed { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("status"), JsonConverter(typeof(TransactionStatusConverter))]
        public BitMaxTransactionStatus Status { get; set; }

        [JsonProperty("time"), JsonConverter(typeof(TimestampConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("transactionType"), JsonConverter(typeof(TransactionTypeConverter))]
        public BitMaxTransactionType TransactionType { get; set; }
    }

    public class BitMaxDestinationAddress
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("destTag")]
        public string Tag { get; set; }
    }
}

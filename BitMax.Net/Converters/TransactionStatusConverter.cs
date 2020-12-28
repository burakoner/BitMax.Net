using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class TransactionStatusConverter : BaseConverter<BitMaxTransactionStatus>
    {
        public TransactionStatusConverter() : this(true) { }
        public TransactionStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxTransactionStatus, string>> Mapping => new List<KeyValuePair<BitMaxTransactionStatus, string>>
        {
            new KeyValuePair<BitMaxTransactionStatus, string>(BitMaxTransactionStatus.Pending, "pending"),
            new KeyValuePair<BitMaxTransactionStatus, string>(BitMaxTransactionStatus.Reviewing, "reviewing"),
            new KeyValuePair<BitMaxTransactionStatus, string>(BitMaxTransactionStatus.Confirmed, "confirmed"),
            new KeyValuePair<BitMaxTransactionStatus, string>(BitMaxTransactionStatus.Rejected, "rejected"),
            new KeyValuePair<BitMaxTransactionStatus, string>(BitMaxTransactionStatus.Canceled, "canceled"),
            new KeyValuePair<BitMaxTransactionStatus, string>(BitMaxTransactionStatus.Failed, "failed"),
        };
    }
}
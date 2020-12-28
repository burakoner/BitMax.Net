using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class TransactionTypeConverter : BaseConverter<BitMaxTransactionType>
    {
        public TransactionTypeConverter() : this(true) { }
        public TransactionTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxTransactionType, string>> Mapping => new List<KeyValuePair<BitMaxTransactionType, string>>
        {
            new KeyValuePair<BitMaxTransactionType, string>(BitMaxTransactionType.Deposit, "deposit"),
            new KeyValuePair<BitMaxTransactionType, string>(BitMaxTransactionType.Withdrawal, "withdrawal"),
        };
    }
}
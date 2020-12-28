using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class CashAccountTypeConverter : BaseConverter<BitMaxCashAccountType>
    {
        public CashAccountTypeConverter() : this(true) { }
        public CashAccountTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxCashAccountType, string>> Mapping => new List<KeyValuePair<BitMaxCashAccountType, string>>
        {
            new KeyValuePair<BitMaxCashAccountType, string>(BitMaxCashAccountType.Spot, "CASH"),
            new KeyValuePair<BitMaxCashAccountType, string>(BitMaxCashAccountType.Margin, "MARGIN"),
        };
    }
}
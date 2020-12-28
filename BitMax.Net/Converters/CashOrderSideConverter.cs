using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class CashOrderSideConverter : BaseConverter<BitMaxCashOrderSide>
    {
        public CashOrderSideConverter() : this(true) { }
        public CashOrderSideConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxCashOrderSide, string>> Mapping => new List<KeyValuePair<BitMaxCashOrderSide, string>>
        {
            new KeyValuePair<BitMaxCashOrderSide, string>(BitMaxCashOrderSide.Buy, "buy"),
            new KeyValuePair<BitMaxCashOrderSide, string>(BitMaxCashOrderSide.Sell, "sell"),
        };
    }
}
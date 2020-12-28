using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class CashOrderTypeConverter : BaseConverter<BitMaxCashOrderType>
    {
        public CashOrderTypeConverter() : this(true) { }
        public CashOrderTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxCashOrderType, string>> Mapping => new List<KeyValuePair<BitMaxCashOrderType, string>>
        {
            new KeyValuePair<BitMaxCashOrderType, string>(BitMaxCashOrderType.Limit, "limit"),
            new KeyValuePair<BitMaxCashOrderType, string>(BitMaxCashOrderType.Market, "market"),
            new KeyValuePair<BitMaxCashOrderType, string>(BitMaxCashOrderType.StopLimit, "stop_limit"),
            new KeyValuePair<BitMaxCashOrderType, string>(BitMaxCashOrderType.StopMarket, "stop_market"),
        };
    }
}
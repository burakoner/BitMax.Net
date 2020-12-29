using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class FuturesOrderTypeConverter : BaseConverter<BitMaxFuturesOrderType>
    {
        public FuturesOrderTypeConverter() : this(true) { }
        public FuturesOrderTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxFuturesOrderType, string>> Mapping => new List<KeyValuePair<BitMaxFuturesOrderType, string>>
        {
            new KeyValuePair<BitMaxFuturesOrderType, string>(BitMaxFuturesOrderType.Limit, "limit"),
            new KeyValuePair<BitMaxFuturesOrderType, string>(BitMaxFuturesOrderType.Market, "market"),
        };
    }
}
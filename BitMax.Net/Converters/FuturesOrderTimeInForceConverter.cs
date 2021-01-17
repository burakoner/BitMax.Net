using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    public class FuturesOrderTimeInForceConverter : BaseConverter<BitMaxFuturesOrderTimeInForce>
    {
        public FuturesOrderTimeInForceConverter() : this(true) { }
        public FuturesOrderTimeInForceConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxFuturesOrderTimeInForce, string>> Mapping => new List<KeyValuePair<BitMaxFuturesOrderTimeInForce, string>>
        {
            new KeyValuePair<BitMaxFuturesOrderTimeInForce, string>(BitMaxFuturesOrderTimeInForce.GoodTillCanceled, "GTC"),
            new KeyValuePair<BitMaxFuturesOrderTimeInForce, string>(BitMaxFuturesOrderTimeInForce.ImmediateOrCancel, "IOC"),
        };
    }
}
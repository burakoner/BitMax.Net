using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    public class CashOrderTimeInForceConverter : BaseConverter<BitMaxCashOrderTimeInForce>
    {
        public CashOrderTimeInForceConverter() : this(true) { }
        public CashOrderTimeInForceConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxCashOrderTimeInForce, string>> Mapping => new List<KeyValuePair<BitMaxCashOrderTimeInForce, string>>
        {
            new KeyValuePair<BitMaxCashOrderTimeInForce, string>(BitMaxCashOrderTimeInForce.GoodTillCanceled, "GTC"),
            new KeyValuePair<BitMaxCashOrderTimeInForce, string>(BitMaxCashOrderTimeInForce.ImmediateOrCancel, "IOC"),
            new KeyValuePair<BitMaxCashOrderTimeInForce, string>(BitMaxCashOrderTimeInForce.FillOrKill, "FOK"),
        };
    }
}
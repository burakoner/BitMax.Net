using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class OrderSideConverter : BaseConverter<BitMaxOrderSide>
    {
        public OrderSideConverter() : this(true) { }
        public OrderSideConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxOrderSide, string>> Mapping => new List<KeyValuePair<BitMaxOrderSide, string>>
        {
            new KeyValuePair<BitMaxOrderSide, string>(BitMaxOrderSide.Buy, "buy"),
            new KeyValuePair<BitMaxOrderSide, string>(BitMaxOrderSide.Sell, "sell"),
        };
    }
}
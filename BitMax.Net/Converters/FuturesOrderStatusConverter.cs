using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class FuturesOrderStatusConverter : BaseConverter<BitMaxFuturesOrderStatus>
    {
        public FuturesOrderStatusConverter() : this(true) { }
        public FuturesOrderStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxFuturesOrderStatus, string>> Mapping => new List<KeyValuePair<BitMaxFuturesOrderStatus, string>>
        {
            new KeyValuePair<BitMaxFuturesOrderStatus, string>(BitMaxFuturesOrderStatus.New, "New"),
            new KeyValuePair<BitMaxFuturesOrderStatus, string>(BitMaxFuturesOrderStatus.PendingNew, "PendingNew"),
            new KeyValuePair<BitMaxFuturesOrderStatus, string>(BitMaxFuturesOrderStatus.Filled, "Filled"),
            new KeyValuePair<BitMaxFuturesOrderStatus, string>(BitMaxFuturesOrderStatus.PartiallyFilled, "PartiallyFilled"),
            new KeyValuePair<BitMaxFuturesOrderStatus, string>(BitMaxFuturesOrderStatus.Cancelled, "Cancelled"),
            new KeyValuePair<BitMaxFuturesOrderStatus, string>(BitMaxFuturesOrderStatus.Reject, "Reject"),
        };
    }
}
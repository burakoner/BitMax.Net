using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    public class CashOrderStatusConverter : BaseConverter<BitMaxCashOrderStatus>
    {
        public CashOrderStatusConverter() : this(true) { }
        public CashOrderStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxCashOrderStatus, string>> Mapping => new List<KeyValuePair<BitMaxCashOrderStatus, string>>
        {
            new KeyValuePair<BitMaxCashOrderStatus, string>(BitMaxCashOrderStatus.New, "New"),
            new KeyValuePair<BitMaxCashOrderStatus, string>(BitMaxCashOrderStatus.PendingNew, "PendingNew"),
            new KeyValuePair<BitMaxCashOrderStatus, string>(BitMaxCashOrderStatus.Filled, "Filled"),
            new KeyValuePair<BitMaxCashOrderStatus, string>(BitMaxCashOrderStatus.PartiallyFilled, "PartiallyFilled"),
            new KeyValuePair<BitMaxCashOrderStatus, string>(BitMaxCashOrderStatus.Cancelled, "Cancelled"),
            new KeyValuePair<BitMaxCashOrderStatus, string>(BitMaxCashOrderStatus.Cancelled, "Canceled"),
            new KeyValuePair<BitMaxCashOrderStatus, string>(BitMaxCashOrderStatus.Reject, "Reject"),
        };
    }
}
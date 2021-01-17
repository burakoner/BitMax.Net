using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    public class AccountTypeConverter : BaseConverter<BitMaxAccountType>
    {
        public AccountTypeConverter() : this(true) { }
        public AccountTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxAccountType, string>> Mapping => new List<KeyValuePair<BitMaxAccountType, string>>
        {
            new KeyValuePair<BitMaxAccountType, string>(BitMaxAccountType.Spot, "CASH"),
            new KeyValuePair<BitMaxAccountType, string>(BitMaxAccountType.Margin, "MARGIN"),
            new KeyValuePair<BitMaxAccountType, string>(BitMaxAccountType.Futures, "FUTURES"),
        };
    }
}
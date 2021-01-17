using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    public class ProductStatusConverter : BaseConverter<BitMaxProductStatus>
    {
        public ProductStatusConverter() : this(true) { }
        public ProductStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxProductStatus, string>> Mapping => new List<KeyValuePair<BitMaxProductStatus, string>>
        {
            new KeyValuePair<BitMaxProductStatus, string>(BitMaxProductStatus.Normal, "Normal"),
            new KeyValuePair<BitMaxProductStatus, string>(BitMaxProductStatus.NoTrading, "NoTrading"),
        };
    }
}
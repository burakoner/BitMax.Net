using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    public class ProductCommissionTypeConverter : BaseConverter<BitMaxProductCommissionType>
    {
        public ProductCommissionTypeConverter() : this(true) { }
        public ProductCommissionTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxProductCommissionType, string>> Mapping => new List<KeyValuePair<BitMaxProductCommissionType, string>>
        {
            new KeyValuePair<BitMaxProductCommissionType, string>(BitMaxProductCommissionType.Base, "Base"),
            new KeyValuePair<BitMaxProductCommissionType, string>(BitMaxProductCommissionType.Quote, "Quote"),
            new KeyValuePair<BitMaxProductCommissionType, string>(BitMaxProductCommissionType.Received, "Received"),
        };
    }
}
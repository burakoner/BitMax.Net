using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class AssetStatusConverter : BaseConverter<BitMaxAssetStatus>
    {
        public AssetStatusConverter() : this(true) { }
        public AssetStatusConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxAssetStatus, string>> Mapping => new List<KeyValuePair<BitMaxAssetStatus, string>>
        {
            new KeyValuePair<BitMaxAssetStatus, string>(BitMaxAssetStatus.Normal, "Normal"),
            new KeyValuePair<BitMaxAssetStatus, string>(BitMaxAssetStatus.NoDeposit, "NoDeposit"),
            new KeyValuePair<BitMaxAssetStatus, string>(BitMaxAssetStatus.NoWithdraw, "NoWithdraw"),
            new KeyValuePair<BitMaxAssetStatus, string>(BitMaxAssetStatus.NoTransaction, "NoTransaction"),
        };
    }
}
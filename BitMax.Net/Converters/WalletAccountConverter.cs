using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    public class WalletAccountConverter : BaseConverter<BitMaxWalletAccount>
    {
        public WalletAccountConverter() : this(true) { }
        public WalletAccountConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxWalletAccount, string>> Mapping => new List<KeyValuePair<BitMaxWalletAccount, string>>
        {
            new KeyValuePair<BitMaxWalletAccount, string>(BitMaxWalletAccount.Cash, "CASH"),
            new KeyValuePair<BitMaxWalletAccount, string>(BitMaxWalletAccount.Margin, "MARGIN"),
            new KeyValuePair<BitMaxWalletAccount, string>(BitMaxWalletAccount.Futures, "FUTURES"),
        };
    }
}
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;

namespace BitMax.Net.CoreObjects
{
    /// <summary>
    /// Socket client options
    /// </summary>
    public class BitMaxSocketClientOptions : SocketClientOptions
    {
        public int AccountGroup = 1;

        public BitMaxSocketClientOptions(): base("wss://bitmax.io/1/api/pro/v1/stream")
        {
            AccountGroup = 1;
            SocketSubscriptionsCombineTarget = 100;
        }

        public BitMaxSocketClientOptions(int accountGroup): base($"wss://bitmax.io/{accountGroup}/api/pro/v1/stream")
        {
            AccountGroup = accountGroup;
            SocketSubscriptionsCombineTarget = 100;
        }
        
        public BitMaxSocketClientOptions(ApiCredentials credentials) : base($"wss://bitmax.io/1/api/pro/v1/stream")
        {
            AccountGroup = 1;
            SocketSubscriptionsCombineTarget = 100;
        }
        
        public BitMaxSocketClientOptions(int accountGroup, ApiCredentials credentials) : base($"wss://bitmax.io/{accountGroup}/api/pro/v1/stream")
        {
            AccountGroup = accountGroup;
            ApiCredentials = credentials;
            SocketSubscriptionsCombineTarget = 100;
        }

        public BitMaxSocketClientOptions Copy()
        {
            var copy = Copy<BitMaxSocketClientOptions>();
            copy.BaseAddress = copy.BaseAddress.TrimEnd('/');
            return copy;
        }
    }
}

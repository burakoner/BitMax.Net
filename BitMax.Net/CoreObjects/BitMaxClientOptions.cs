using CryptoExchange.Net.Objects;

namespace BitMax.Net.CoreObjects
{
    public class BitMaxClientOptions: RestClientOptions
    {
        public BitMaxClientOptions():base("https://bitmax.io") // without slash on end
        {
        }

        public BitMaxClientOptions Copy()
        {
            return Copy<BitMaxClientOptions>();
        }
    }
}

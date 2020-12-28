using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class CashOrderResponseInstructionConverter : BaseConverter<BitMaxCashOrderResponseInstruction>
    {
        public CashOrderResponseInstructionConverter() : this(true) { }
        public CashOrderResponseInstructionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxCashOrderResponseInstruction, string>> Mapping => new List<KeyValuePair<BitMaxCashOrderResponseInstruction, string>>
        {
            new KeyValuePair<BitMaxCashOrderResponseInstruction, string>(BitMaxCashOrderResponseInstruction.ACK, "ACK"),
            new KeyValuePair<BitMaxCashOrderResponseInstruction, string>(BitMaxCashOrderResponseInstruction.ACCEPT, "ACCEPT"),
            new KeyValuePair<BitMaxCashOrderResponseInstruction, string>(BitMaxCashOrderResponseInstruction.DONE, "DONE"),
        };
    }
}
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    public class OrderResponseInstructionConverter : BaseConverter<BitMaxOrderResponseInstruction>
    {
        public OrderResponseInstructionConverter() : this(true) { }
        public OrderResponseInstructionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxOrderResponseInstruction, string>> Mapping => new List<KeyValuePair<BitMaxOrderResponseInstruction, string>>
        {
            new KeyValuePair<BitMaxOrderResponseInstruction, string>(BitMaxOrderResponseInstruction.ACK, "ACK"),
            new KeyValuePair<BitMaxOrderResponseInstruction, string>(BitMaxOrderResponseInstruction.ACCEPT, "ACCEPT"),
            new KeyValuePair<BitMaxOrderResponseInstruction, string>(BitMaxOrderResponseInstruction.DONE, "DONE"),
            new KeyValuePair<BitMaxOrderResponseInstruction, string>(BitMaxOrderResponseInstruction.ERROR, "Err"),
        };
    }
}
using BitMax.Net.Enums;
using CryptoExchange.Net.Converters;
using System.Collections.Generic;

namespace BitMax.Net.Converters
{
    internal class PeriodConverter : BaseConverter<BitMaxPeriod>
    {
        public PeriodConverter() : this(true) { }
        public PeriodConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<BitMaxPeriod, string>> Mapping => new List<KeyValuePair<BitMaxPeriod, string>>
        {
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.OneMinute, "1"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.FiveMinutes, "5"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.FifteenMinutes, "15"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.ThirtyMinutes, "30"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.OneHour, "60"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.TwoHours, "120"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.FourHours, "240"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.SixHours, "360"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.TwelveHours, "720"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.OneDay, "1d"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.OneWeek, "1w"),
            new KeyValuePair<BitMaxPeriod, string>(BitMaxPeriod.OneMonth, "1m"),
        };
    }
}
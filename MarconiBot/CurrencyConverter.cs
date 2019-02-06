using System;
using System.Collections.Generic;

namespace MarconiBot
{
    class CurrencyConverter : ICurrencyConverter
    {
        private readonly List<ConversionRate> rates;

        public CurrencyConverter()
        {
            this.rates = new List<ConversionRate>()
            {
                new ConversionRate(Currency.EUR, Currency.USD, 1.15)
            };
        }

        public double Convert(Currency source, Currency destination, double value)
        {
            foreach (var rate in this.rates)
            {
                if (rate.Source == source && rate.Destination == rate.Destination)
                {
                    return value * rate.Value;
                }
                else if (rate.Source == destination && rate.Destination == source)
                {
                    return value / rate.Value;
                }
            }

            throw new NotSupportedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarconiBot
{
    enum Currency
    {
        EUR,
        USD
    }

    class CurrencyConverter
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

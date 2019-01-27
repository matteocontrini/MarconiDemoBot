namespace MarconiBot
{
    struct ConversionRate
    {
        public Currency Source { get; }
        public Currency Destination { get; }
        public double Value { get; }

        public ConversionRate(Currency source, Currency destination, double rate) : this()
        {
            Source = source;
            Destination = destination;
            Value = rate;
        }
    }
}

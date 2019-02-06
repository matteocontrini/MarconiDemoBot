namespace MarconiBot
{
    interface ICurrencyConverter
    {
        double Convert(Currency source, Currency destination, double value);
    }
}

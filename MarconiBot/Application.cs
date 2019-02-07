using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace MarconiBot
{
    class Application : IApplication
    {
        private ITelegramBotClient bot;

        public void Start()
        {
            this.bot = new TelegramBotClient("");

            this.bot.OnMessage += Bot_OnMessage;
            this.bot.StartReceiving();
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            long chatId = e.Message.Chat.Id;
            string inputText = e.Message.Text;

            if (inputText == "/start")
            {
                await this.bot.SendTextMessageAsync(chatId, "Ciao! Per iniziare, inviami il valore che vuoi convertire");
            }
            else
            {
                inputText = inputText.Replace('.', ',');

                bool ok = double.TryParse(
                    s: inputText,
                    style: NumberStyles.Float,
                    provider: CultureInfo.GetCultureInfoByIetfLanguageTag("it"),
                    result: out double input
                );

                if (ok && input >= 0)
                {
                    CurrencyConverter converter = new CurrencyConverter();

                    Currency source = Currency.EUR;
                    Currency destination = Currency.USD;

                    double converted = converter.Convert(source, destination, input);

                    string msg = $"{source}: {input}\n{destination}: {converted}";

                    await this.bot.SendTextMessageAsync(chatId, msg);
                }
                else
                {
                    await this.bot.SendTextMessageAsync(chatId, "Non ho capito cosa vuoi dire 🤔");
                }
            }
        }
    }
}

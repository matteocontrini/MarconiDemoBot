using System;
using System.Collections.Generic;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace MarconiBot
{
    class Application : IApplication
    {
        private ITelegramBotClient bot;
        private ReplyKeyboardMarkup defaultKeyboard;

        public void Start()
        {
            this.bot = new TelegramBotClient("");

            this.defaultKeyboard = new ReplyKeyboardMarkup(new List<List<KeyboardButton>>
            {
                new List<KeyboardButton>
                {
                    new KeyboardButton("5"),
                    new KeyboardButton("10"),
                }
            }, true);

            this.bot.OnMessage += Bot_OnMessage;
            this.bot.OnCallbackQuery += Bot_OnCallbackQuery;
            this.bot.StartReceiving();
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            long chatId = e.Message.Chat.Id;
            string inputText = e.Message.Text;

            if (inputText == "/start")
            {
                await this.bot.SendTextMessageAsync(
                    chatId: chatId,
                    text: "Ciao! Per iniziare, inviami il valore che vuoi convertire",
                    replyMarkup: defaultKeyboard
                );
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

                    await this.bot.SendTextMessageAsync(
                        chatId: chatId,
                        text: msg,
                        replyMarkup: GenerateKeyboard(input)
                    );
                }
                else
                {
                    await this.bot.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Non ho capito cosa vuoi dire 🤔",
                        replyMarkup: defaultKeyboard
                    );
                }
            }
        }

        private InlineKeyboardMarkup GenerateKeyboard(double value)
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData("🇪🇺 => 🇺🇸", $"EUR;USD;{value}"),
                        InlineKeyboardButton.WithCallbackData("🇺🇸 => 🇪🇺", $"USD;EUR;{value}")
                    }
                }
            );
        }

        private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            string[] input = e.CallbackQuery.Data.Split(';');

            Enum.TryParse(input[0], out Currency source);
            Enum.TryParse(input[1], out Currency destination);
            double value = double.Parse(input[2]);

            CurrencyConverter converter = new CurrencyConverter();
            double converted = converter.Convert(source, destination, value);

            string msg = $"{source}: {value}\n{destination}: {converted}";

            await this.bot.EditMessageTextAsync(
                chatId: e.CallbackQuery.Message.Chat.Id,
                messageId: e.CallbackQuery.Message.MessageId,
                text: msg,
                replyMarkup: GenerateKeyboard(value)
            );
        }
    }
}

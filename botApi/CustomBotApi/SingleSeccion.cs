using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.CustomBotApi
{
    public class SingleSeccion
    {
        public TelegramBotClient bot_client;
        private List<command> commands;
        public long chat_id;
        private ReplyKeyboardMarkup keyboard;
        private bool firstMessage = true;
        private botServer server;

        public SingleSeccion(string token, long _chat_id, List<command> _commands, ReplyKeyboardMarkup _keyboard, botServer serv)
        {
            server = serv;
            keyboard = _keyboard;
            commands = _commands;
            chat_id = _chat_id;
            bot_client = new TelegramBotClient(token);
            bot_client.OnMessage += BotOnMessageReceived;
            bot_client.OnUpdate += BotOnUpdate;
            bot_client.StartReceiving(new UpdateType[] { UpdateType.MessageUpdate });
        }

        private async void BotOnUpdate(object sender, UpdateEventArgs upadateEventArgs)
        {
            var cnt = upadateEventArgs.Update.Message.Contact;
            if (cnt != null)
                server.Contacts.Add(cnt);
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Message msg = messageEventArgs.Message;
            bool find = false;
            if (msg == null || msg.Type != MessageType.TextMessage) return;
            if (chat_id == msg.Chat.Id)
            {
                foreach (var task in commands)
                {
                    SingleSeccion ss = this;
                    if (task.name == msg.Text)
                    {
                        task.task(ss); find = true;
                        return;
                    }
                }
                if (!find) await bot_client.SendTextMessageAsync(chat_id, "/help - помощь");
            }
        }

        public long GetChatId()
        {
            return chat_id;
        }

        public async void SendText(string text)
        {
            await bot_client.SendTextMessageAsync(chat_id, text);
        }

        public async void SendKeyBoard(string one, string message)
        {
            var keyBoard = new ReplyKeyboardMarkup();
            keyBoard.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                 {
                    new KeyboardButton(one)
                 }
            };
            await bot_client.SendTextMessageAsync(chat_id, message, ParseMode.Default, false, false, 0, keyBoard);
        }

        public async void SendKeyBoard(string one, string two, string message)
        {
            var keyBoard = new ReplyKeyboardMarkup();
            keyBoard.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                 {
                    new KeyboardButton(one),
                    new KeyboardButton(two)
                 }
            };
            await bot_client.SendTextMessageAsync(chat_id, message, ParseMode.Default, false, false, 0, keyBoard);
        }

        public async void SendKeyBoard(string one, string two, string three, string message)
        {
            var keyBoard = new ReplyKeyboardMarkup();
            keyBoard.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                 {
                    new KeyboardButton(one),
                    new KeyboardButton(two)
                 },
                new KeyboardButton[]
                {
                    new KeyboardButton(three)
                }
            };
            await bot_client.SendTextMessageAsync(chat_id, message, ParseMode.Default, false, false, 0, keyBoard);
        }

        public async void SendKeyBoard(string one, string two, string three, string four, string message)
        {
            var keyBoard = new ReplyKeyboardMarkup();
            keyBoard.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                 {
                    new KeyboardButton(one),
                    new KeyboardButton(two)
                 },
                new KeyboardButton[]
                {
                    new KeyboardButton(three),
                    new KeyboardButton(four)
                }
            };
            await bot_client.SendTextMessageAsync(chat_id, message, ParseMode.Default, false, false, 0, keyBoard);
        }

        public async void SendKeyBoard(string one, string two, string three, string four, string five, string message)
        {
            var keyBoard = new ReplyKeyboardMarkup();
            keyBoard.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                 {
                    new KeyboardButton(one),
                    new KeyboardButton(two)
                 },
                new KeyboardButton[]
                {
                    new KeyboardButton(three),
                    new KeyboardButton(four),
                    new KeyboardButton(five)
                }
            };
            await bot_client.SendTextMessageAsync(chat_id, message, ParseMode.Default, false, false, 0, keyBoard);
        }

        public async void SendKeyBoard(string one, string two, string three, string four, string five, string six, bool deleteMess, string message = ".")
        {
            var keyBoard = new ReplyKeyboardMarkup();
            keyBoard.Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[]
                 {
                    new KeyboardButton(one),
                    new KeyboardButton(two)
                 },
                new KeyboardButton[]
                {
                    new KeyboardButton(three)
                },
                new KeyboardButton[]
                {
                    new KeyboardButton(four),
                    new KeyboardButton(five),
                    new KeyboardButton(six)
                }
            };
            Message msg = bot_client.SendTextMessageAsync(chat_id, message, ParseMode.Default, false, false, 0, keyBoard).Result;
            if (deleteMess) await bot_client.DeleteMessageAsync(chat_id, msg.MessageId);
        }

        public async void DeleteKeyBoard()
        {
            Message sentMsg = bot_client.SendTextMessageAsync(chat_id, ".", ParseMode.Default, false, false, replyMarkup: new ReplyKeyboardRemove()).Result;
            await bot_client.DeleteMessageAsync(chat_id, sentMsg.MessageId);
        }
    }
}
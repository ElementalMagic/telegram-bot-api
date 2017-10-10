using botApi.CustomBotApi;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.CustomBotApi;

namespace TelegramBot
{
    public class botServer
    {
        public List<command> commands { get; set; }
        private List<long> chat_id = new List<long>();
        private List<SingleSeccion> connections = new List<SingleSeccion>();
        private string bot_token;
        private TelegramBotClient bot_client;
        private ReplyKeyboardMarkup Start_keyboard;
        private string sayHello;
        public List<Contact> Contacts { get; }
        public List<Connection> user_info_connected;

        public botServer(string token, List<command> _commands, string _sayHello, ReplyKeyboardMarkup kb = null)
        {
            Contacts = new List<Contact>();
            user_info_connected = new List<Connection>();
            Start_keyboard = kb;
            sayHello = _sayHello;
            commands = _commands;
            bot_token = token;
        }

        private async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Message msg = messageEventArgs.Message;
            bool find = false;
            foreach (var client in connections)
            {
                if (msg.Chat.Id == client.chat_id)
                {
                    find = true;
                }
            }
            if (!find)
            {
                user_info_connected.Add(new Connection(msg.From.FirstName, msg.From.LastName, msg.Chat.Id, msg.From.Id));

                connections.Add(new SingleSeccion(bot_token, msg.Chat.Id, commands, Start_keyboard, this));
                await bot_client.SendTextMessageAsync(msg.Chat.Id, sayHello, replyMarkup: Start_keyboard);
            }
        }

        public async void SendMessageToAll(string message)
        {
            if (user_info_connected != null)
            {
                foreach (var user in user_info_connected)
                {
                    await bot_client.SendTextMessageAsync(user.chat_id, message);
                }
            }
        }

        public void setConnectionList(List<Connection> connections)
        {
            user_info_connected = connections;
        }

        public void saveConnectionsToFile(string fileName, string SavePath = "")
        {
            if (user_info_connected != null)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                if (SavePath != string.Empty && !SavePath.EndsWith(@"\"))
                {
                    SavePath += @"\";
                }
                FileStream outputStream = System.IO.File.Create(SavePath + fileName);
                formatter.Serialize(outputStream, user_info_connected);
                outputStream.Close();
            }
        }

        public void BotStart()
        {
            bot_client = new TelegramBotClient(bot_token);
            bot_client.OnMessage += BotOnMessageReceived;
            if (user_info_connected != null)
            {
                foreach (var user in user_info_connected)
                {
                    connections.Add(new SingleSeccion(bot_token, user.chat_id, commands, Start_keyboard, this));
                }
            }
            bot_client.StartReceiving(new UpdateType[] { UpdateType.MessageUpdate });
        }

        public void readConnectionsFromFile(string FullFilePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream inputStream = System.IO.File.OpenRead(FullFilePath);
            user_info_connected = (List<Connection>)formatter.Deserialize(inputStream);
            inputStream.Close();
        }
    }
}
using System;
using TelegramBot.CustomBotApi;

namespace TelegramBot
{
    public class command
    {
        // public Action task { get; set; }
        public Action<SingleSeccion> task { get; set; }

        public string name { get; set; }
        private string description { get; set; }
        /*  public command(Action command, string commandName, string CommandDescription="")
          {
              task = command;
              name = commandName;
              CommandDescription = description;
          } */

        public command(Action<SingleSeccion> command, string commandName, string CommandDescription = "")
        {
            task = command;
            name = commandName;
            CommandDescription = description;
        }
    }
}
using System;

namespace botApi.CustomBotApi
{
    [Serializable]
    public class Connection
    {
        public string first_name { get; }
        public string last_name { get; }
        public long chat_id { get; }
        public int user_id { get; }

        public Connection(string fn, string ln, long ci, int ui)
        {
            first_name = fn;
            last_name = ln;
            chat_id = ci;
            user_id = ui;
        }

        public string getInfo()
        {
            return string.Format("{0} {1}", last_name, first_name);
        }
    }
}
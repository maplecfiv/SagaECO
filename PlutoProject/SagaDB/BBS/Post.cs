using System;
using System.Collections.Generic;

namespace SagaDB.BBS
{
    public class Post
    {
        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }


    public class Mail
    {
        public uint MailID { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }

    public class Gift
    {
        public uint MailID { get; set; }

        public uint AccountID { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public Dictionary<uint, ushort> Items { get; set; } = new Dictionary<uint, ushort>();
    }
}
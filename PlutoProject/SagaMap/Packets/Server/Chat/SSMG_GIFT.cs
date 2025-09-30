using System;
using SagaDB.BBS;
using SagaLib;

namespace SagaMap.Packets.Server.Chat
{
    public class SSMG_GIFT : Packet
    {
        public SSMG_GIFT()
        {
            data = new byte[74];
            offset = 2;
            ID = 0x01F4;
        }

        public Gift mails
        {
            set
            {
                var name = Global.Unicode.GetBytes(value.Name + "\0");
                var title = Global.Unicode.GetBytes(value.Title + "\0");
                var buff = new byte[name.Length + title.Length + data.Length];
                data.CopyTo(buff, 0);
                data = buff;

                PutUInt(value.MailID, 2);
                PutUInt((uint)(value.Date - new DateTime(1970, 1, 1)).TotalSeconds, 6);

                PutByte((byte)name.Length, 10);
                PutBytes(name, 11);

                PutByte((byte)title.Length, offset);
                PutBytes(title, offset);

                PutByte(10, offset);

                var itemIDs = new uint[10];
                var counts = new ushort[10];
                byte count = 0;

                foreach (var item in value.Items)
                {
                    itemIDs[count] = item.Key;
                    counts[count] = item.Value;
                    count++;
                }

                for (var i = 0; i < 10; i++)
                    PutUInt(itemIDs[i], offset);
                PutByte(10, offset);

                for (var i = 0; i < 10; i++)
                    PutUShort(counts[i], offset);
            }
        }
    }
}
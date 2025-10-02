using System;
using System.Collections.Generic;
using SagaDB.BBS;
using SagaLib;

namespace SagaMap.Packets.Server.Community
{
    public class SSMG_COMMUNITY_BBS_PAGE_INFO : Packet
    {
        public SSMG_COMMUNITY_BBS_PAGE_INFO()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1B09;
        }

        public List<Post> Posts
        {
            set
            {
                var names = new byte[value.Count][];
                var titles = new byte[value.Count][];
                var contents = new byte[value.Count][];

                var j = 0;
                var count = 0;
                foreach (var i in value)
                {
                    names[j] = Global.Unicode.GetBytes(i.Name + "\0");
                    titles[j] = Global.Unicode.GetBytes(i.Title + "\0");
                    contents[j] = Global.Unicode.GetBytes(i.Content + "\0");
                    if (contents[j].Length >= 0xfd)
                        count += 4;
                    count += names[j].Length + titles[j].Length + contents[j].Length + 3;
                    j++;
                }

                var buf = new byte[10 + count + 4 * value.Count];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)value.Count, 6);
                for (var i = 0; i < value.Count; i++)
                    PutUInt((uint)(value[i].Date - new DateTime(1970, 1, 1)).TotalSeconds);

                PutByte((byte)value.Count);
                for (var i = 0; i < value.Count; i++)
                {
                    PutByte((byte)names[i].Length);
                    PutBytes(names[i]);
                }

                PutByte((byte)value.Count);
                for (var i = 0; i < value.Count; i++)
                {
                    PutByte((byte)titles[i].Length);
                    PutBytes(titles[i]);
                }

                PutByte((byte)value.Count);
                for (var i = 0; i < value.Count; i++)
                    if (contents[i].Length < 0xfd)
                    {
                        PutByte((byte)contents[i].Length);
                        PutBytes(contents[i]);
                    }
                    else
                    {
                        PutByte(0xfd);
                        PutInt(contents[i].Length);
                        PutBytes(contents[i]);
                    }
            }
        }
    }
}
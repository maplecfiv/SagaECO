using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Packets.Server.Community
{
    public class SSMG_COMMUNITY_RECRUIT : Packet
    {
        public SSMG_COMMUNITY_RECRUIT()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x1B9F;
        }

        public RecruitmentType Type
        {
            set => PutByte((byte)value, 2);
        }

        public int Page
        {
            set => PutInt(value, 3);
        }

        public int MaxPage
        {
            set => PutInt(value, 7);
        }

        public List<Recruitment> Entries
        {
            set
            {
                var buf = new byte[data.Length + value.Count * 4 + 1];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)value.Count, 11);
                for (var i = 0; i < value.Count; i++) PutUInt(value[i].Creator.CharID, (ushort)(12 + i * 4));

                buf = new byte[data.Length + value.Count * 4 + 1];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)value.Count, (ushort)(12 + value.Count * 4));
                for (var i = 0; i < value.Count; i++)
                    PutUInt(value[i].Creator.MapID, (ushort)(13 + value.Count * 4 + i * 4));

                buf = new byte[data.Length + value.Count + 1];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)value.Count, (ushort)(13 + value.Count * 8));
                for (var i = 0; i < value.Count; i++)
                    if (value[i].Creator.Party != null)
                        PutByte((byte)value[i].Creator.Party.MemberCount, (ushort)(14 + value.Count * 8 + i));
                    else
                        PutByte(0, (ushort)(14 + value.Count * 8 + i));

                var strings = new byte[value.Count][];
                var size = 0;
                for (var i = 0; i < value.Count; i++)
                {
                    strings[i] = Global.Unicode.GetBytes(value[i].Creator.Name);
                    size += strings[i].Length + 1;
                }

                buf = new byte[data.Length + size + 1];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)value.Count, (ushort)(14 + value.Count * 9));
                size = 0;
                for (var i = 0; i < value.Count; i++)
                {
                    PutByte((byte)strings[i].Length, (ushort)(15 + value.Count * 9 + size));
                    PutBytes(strings[i], (ushort)(16 + value.Count * 9 + size));
                    size += strings[i].Length + 1;
                }

                strings = new byte[value.Count][];
                size = 0;
                for (var i = 0; i < value.Count; i++)
                {
                    strings[i] = Global.Unicode.GetBytes(value[i].Title);
                    size += strings[i].Length + 1;
                }

                buf = new byte[data.Length + size + 1];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)value.Count);
                for (var i = 0; i < value.Count; i++)
                {
                    PutByte((byte)strings[i].Length);
                    PutBytes(strings[i]);
                }

                strings = new byte[value.Count][];
                size = 0;
                for (var i = 0; i < value.Count; i++)
                {
                    strings[i] = Global.Unicode.GetBytes(value[i].Content);
                    size += strings[i].Length + 1;
                }

                buf = new byte[data.Length + size + 1];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)value.Count);
                for (var i = 0; i < value.Count; i++)
                {
                    PutByte((byte)strings[i].Length);
                    PutBytes(strings[i]);
                }
            }
        }
    }
}
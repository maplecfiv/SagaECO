using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.DefWar
{
    public class SSMG_DEFWAR_STATES : Packet
    {
        public SSMG_DEFWAR_STATES()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1BCB;
            PutByte(1, 2);
        }

        public Dictionary<uint, byte> List
        {
            set
            {
                if (value.Count > 0)
                {
                    var buf = new byte[6 + value.Count * 6];
                    data.CopyTo(buf, 0);
                    data = buf;
                    offset = 3;
                    PutByte((byte)value.Count); //地图ID
                    offset += (ushort)(value.Count * 4);
                    PutByte((byte)value.Count); //百分比
                    offset += (ushort)value.Count;
                    PutByte((byte)value.Count); //不明
                    var index = 0;
                    foreach (var i in value)
                    {
                        PutUInt(i.Key, (ushort)(4 + index * 4));
                        PutByte(i.Value, (ushort)(5 + value.Count * 4 + index));
                        PutByte(0x1, (ushort)(6 + value.Count * 5 + index));
                        index++;
                    }
                }
            }
        }
    }
}
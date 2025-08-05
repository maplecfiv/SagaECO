using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.DefWar
{
    public class SSMG_DEFWAR_INFO : Packet
    {
        public SSMG_DEFWAR_INFO()
        {
            data = new byte[16];
            offset = 2;
            ID = 0x1BD0;
        }


        public List<SagaDB.DefWar.DefWar> List
        {
            set
            {
                if (value.Count > 0)
                {
                    var buf = new byte[11 + value.Count * 20];
                    data.CopyTo(buf, 0);
                    data = buf;
                    offset = 2;
                    PutByte((byte)value.Count); //指令顺序
                    offset += (ushort)value.Count;
                    PutByte((byte)value.Count); //指令列表
                    offset += (ushort)(value.Count * 4);
                    PutByte((byte)value.Count); //结果1
                    offset += (ushort)value.Count;
                    PutByte((byte)value.Count); //结果2
                    offset += (ushort)value.Count;
                    PutByte((byte)value.Count); //Unknown
                    offset += (ushort)value.Count;
                    PutByte((byte)value.Count); //Unknown
                    offset += (ushort)(value.Count * 4);
                    PutByte((byte)value.Count); //Unknown
                    offset += (ushort)(value.Count * 4);
                    PutByte((byte)value.Count); //Unknown
                    for (var i = 0; i < value.Count; i++)
                    {
                        var dw = value[i];
                        PutByte(dw.Number, (ushort)(3 + i));
                        PutUInt(dw.ID, (ushort)(4 + value.Count + i * 4));
                        PutByte(dw.Result1, (ushort)(5 + value.Count * 5 + i));
                        PutByte(dw.Result2, (ushort)(6 + value.Count * 6 + i));
                        PutByte(dw.unknown1, (ushort)(7 + value.Count * 7 + i));
                        PutInt(dw.unknown2, (ushort)(8 + value.Count * 8 + i * 4));
                        PutInt(dw.unknown3, (ushort)(9 + value.Count * 12 + i * 4));
                        PutInt(dw.unknown4, (ushort)(10 + value.Count * 16 + i * 4));
                    }
                }
            }
        }
    }
}
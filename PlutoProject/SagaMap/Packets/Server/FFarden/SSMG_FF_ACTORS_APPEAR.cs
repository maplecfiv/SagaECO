using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.FFGarden
{
    public class SSMG_FF_ACTORS_APPEAR : Packet
    {
        public SSMG_FF_ACTORS_APPEAR()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1BEF;
        }

        public uint MapID
        {
            set => PutUInt(value, 2);
        }

        public List<ActorFurniture> List
        {
            set
            {
                if (value.Count > 0)
                {
                    var buf = new byte[18 + value.Count * 27];
                    data.CopyTo(buf, 0);
                    data = buf;
                    offset = 6;
                    PutByte((byte)value.Count); //ActorID
                    offset += (ushort)(value.Count * 4);
                    PutByte((byte)value.Count); //ItemID
                    offset += (ushort)(value.Count * 4);
                    PutByte((byte)value.Count); //PictID
                    offset += (ushort)(value.Count * 4);
                    PutByte((byte)value.Count); //Unknown
                    offset += (ushort)value.Count;
                    PutByte((byte)value.Count); //X
                    offset += (ushort)(value.Count * 2);
                    PutByte((byte)value.Count); //Y
                    offset += (ushort)(value.Count * 2);
                    PutByte((byte)value.Count); //Z
                    offset += (ushort)(value.Count * 2);
                    PutByte((byte)value.Count); //Xaxis
                    offset += (ushort)(value.Count * 2);
                    PutByte((byte)value.Count); //Yaxis
                    offset += (ushort)(value.Count * 2);
                    PutByte((byte)value.Count); //Zaxis
                    offset += (ushort)(value.Count * 2);
                    PutByte((byte)value.Count); //Motion
                    offset += (ushort)(value.Count * 2);
                    PutByte((byte)value.Count); //Name
                    for (var i = 0; i < value.Count; i++)
                    {
                        var af = value[i];
                        PutUInt(af.ActorID, (ushort)(7 + i * 4));
                        PutUInt(af.ItemID, (ushort)(8 + value.Count * 4 + i * 4));
                        PutUInt(af.PictID, (ushort)(9 + value.Count * 8 + i * 4));
                        PutByte(0x0, (ushort)(10 + value.Count * 12 + i)); //Unknown
                        PutShort(af.X, (ushort)(11 + value.Count * 13 + i * 2));
                        PutShort(af.Y, (ushort)(12 + value.Count * 15 + i * 2));
                        PutShort(af.Z, (ushort)(13 + value.Count * 17 + i * 2));
                        PutShort(af.Xaxis, (ushort)(14 + value.Count * 19 + i * 2));
                        PutShort(af.Yaxis, (ushort)(15 + value.Count * 21 + i * 2));
                        PutShort(af.Zaxis, (ushort)(16 + value.Count * 23 + i * 2));
                        PutUShort(af.Motion, (ushort)(17 + value.Count * 25 + i * 2));

                        var ind = (ushort)data.Length;
                        var name = Global.Unicode.GetBytes(af.Name + "\0");
                        buf = new byte[data.Length + name.Length + 1];
                        data.CopyTo(buf, 0);
                        data = buf;
                        offset = ind;
                        PutByte((byte)name.Length);
                        PutBytes(name);
                    }
                }
            }
        }
    }
}
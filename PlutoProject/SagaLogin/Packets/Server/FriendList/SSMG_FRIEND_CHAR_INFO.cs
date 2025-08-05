using SagaDB.Actor;
using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Server
{
    public class SSMG_FRIEND_CHAR_INFO : Packet
    {
        public SSMG_FRIEND_CHAR_INFO()
        {
            data = new byte[20];
            ID = 0x00DC;
        }

        public ActorPC ActorPC
        {
            set
            {
                PutUInt(value.CharID, 2);
                var buf = Global.Unicode.GetBytes(value.Name + "\0");
                var buff = new byte[20 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;
                var size = (byte)buf.Length;
                PutByte(size, 6);
                PutBytes(buf, 7);
                if (Configuration.Instance.Version >= Version.Saga10)
                {
                    PutByte(4, (ushort)(7 + size));
                    PutUShort((ushort)value.Job, (ushort)(8 + size));
                    PutUShort(value.Level, (ushort)(10 + size));
                    PutUShort(value.CurrentJobLevel, (ushort)(12 + size));
                }
                else
                {
                    PutUShort((ushort)value.Job, (ushort)(7 + size));
                    PutUShort(value.Level, (ushort)(9 + size));
                    PutUShort(value.CurrentJobLevel, (ushort)(11 + size));
                }
            }
        }

        public uint MapID
        {
            set
            {
                var size = GetByte(6);
                if (Configuration.Instance.Version >= Version.Saga10)
                    PutUInt(value, (ushort)(14 + size));
                else
                    PutUInt(value, (ushort)(13 + size));
            }
        }

        public CharStatus Status
        {
            set
            {
                var size = GetByte(6);
                if (Configuration.Instance.Version >= Version.Saga10)
                    PutByte((byte)value, (ushort)(18 + size));
                else
                    PutByte((byte)value, (ushort)(17 + size));
            }
        }

        public string Comment
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga10)
                {
                    var size = GetByte(6);
                    var buf = Global.Unicode.GetBytes(value + "\0");
                    var buff = new byte[20 + size + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte((byte)buf.Length, (ushort)(19 + size));
                    PutBytes(buf, (ushort)(20 + size));
                }
                else
                {
                    var size = GetByte(6);
                    var buf = Global.Unicode.GetBytes(value + "\0");
                    var buff = new byte[19 + size + buf.Length];
                    data.CopyTo(buff, 0);
                    data = buff;
                    PutByte((byte)buf.Length, (ushort)(18 + size));
                    PutBytes(buf, (ushort)(19 + size));
                }
            }
        }
    }
}
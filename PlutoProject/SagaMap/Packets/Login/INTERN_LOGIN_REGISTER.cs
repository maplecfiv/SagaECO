using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Login
{
    public class INTERN_LOGIN_REGISTER : Packet
    {
        public INTERN_LOGIN_REGISTER()
        {
            data = new byte[8];
            offset = 2;
            ID = 0xFFF0;
        }

        public string Password
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value);
                PutByte((byte)buf.Length, 2);
                var buff = new byte[data.Length + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutBytes(buf, 3);
            }
        }

        public List<uint> HostedMaps
        {
            set
            {
                ushort index;
                index = (ushort)(3 + GetByte(2));
                var buf = Global.Unicode.GetBytes(Configuration.Configuration.Instance.Host);
                PutByte((byte)buf.Length, index);
                var buff = new byte[data.Length + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;
                PutBytes(buf, (ushort)(index + 1));
                index = (ushort)(index + 1 + buf.Length);
                PutInt(Configuration.Configuration.Instance.Port, index);

                buff = new byte[data.Length + value.Count * 4 + 1];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)value.Count, (ushort)(index + 4));
                for (var i = 0; i < value.Count; i++) PutUInt(value[i], (ushort)(index + 5 + i * 4));
            }
        }
    }
}
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_EQUIP_NAME : Packet
    {
        public SSMG_PLAYER_EQUIP_NAME()
        {
            data = new byte[4];
            ID = 0x0264;
        }

        public string ActorName
        {
            set
            {
                byte[] buf, buff;

                buf = Global.Unicode.GetBytes(value + "\0");
                size = (byte)buf.Length;
                buff = new byte[data.Length - 1 + size];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)size, 2);
                PutBytes(buf, 3);
            }
        }
    }
}
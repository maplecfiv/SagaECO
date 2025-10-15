using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_SHOP_APPEAR : Packet
    {
        public SSMG_PLAYER_SHOP_APPEAR()
        {
            data = new byte[8]; //TitleBytes.Length+2+4+5
            offset = 2;
            ID = 0x1902;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public byte button
        {
            set => PutByte(value, 6); //开关 0关1开
        }

        public string Title
        {
            set
            {
                var title = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[8 + title.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)title.Length, 7);
                PutBytes(title, 8);

                //string www = string.Format("DL:{0}    L:{1}     TL{2}     T:{3}   buf", this.data.Length, value.Length, title.Length, value);
                //Logger.getLogger().Error(www);
            }
        }
    }
}
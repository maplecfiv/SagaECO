using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TRADE_START : Packet
    {
        public SSMG_TRADE_START()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x0A0F;
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type">00だと人間? 01だとNPC</param>
        public void SetPara(string name, int type)
        {
            var buf = Global.Unicode.GetBytes(name + "\0");
            var buff = new byte[7 + buf.Length];
            data.CopyTo(buff, 0);
            data = buff;
            PutByte((byte)buf.Length, 2);
            PutBytes(buf, 3);
            PutInt(type, (ushort)(3 + buf.Length));
        }
    }
}
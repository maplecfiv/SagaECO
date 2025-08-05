using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTY_INVITE : Packet
    {
        public SSMG_PARTY_INVITE()
        {
            data = new byte[9];
            offset = 2;
            ID = 0x19CA;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }

        public string Name
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[9 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;
                var size = (byte)buf.Length;
                PutByte(size, 6);
                PutBytes(buf, 7);

                //unknown byte
                PutByte(1, (ushort)(7 + size));
            }
        }
    }
}
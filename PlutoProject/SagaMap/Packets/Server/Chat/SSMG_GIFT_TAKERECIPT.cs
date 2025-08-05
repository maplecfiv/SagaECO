using SagaLib;

namespace SagaMap.Packets.Server.Chat
{
    public class SSMG_GIFT_TAKERECIPT : Packet
    {
        public SSMG_GIFT_TAKERECIPT()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x0691;
        }

        public byte type
        {
            set => PutByte(value, 5);
        }

        public uint MailID
        {
            set => PutUInt(value, 6);
        }
    }
}
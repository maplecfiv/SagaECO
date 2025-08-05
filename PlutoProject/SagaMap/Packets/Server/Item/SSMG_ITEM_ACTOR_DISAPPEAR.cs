using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_ACTOR_DISAPPEAR : Packet
    {
        public SSMG_ITEM_ACTOR_DISAPPEAR()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x07DF;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public byte Count
        {
            set => PutByte(value, 6);
        }

        public uint Looter
        {
            set => PutUInt(value, 7);
        }
    }
}
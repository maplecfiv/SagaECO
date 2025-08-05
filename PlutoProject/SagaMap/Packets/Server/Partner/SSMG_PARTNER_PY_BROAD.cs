using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTNER_PY_BROAD : Packet
    {
        public SSMG_PARTNER_PY_BROAD()
        {
            data = new byte[19];
            offset = 2;
            ID = 0x17A3;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public byte Type
        {
            set => PutByte(value, 6);
        }

        public uint PictID
        {
            set
            {
                PutUInt(value, 7);
                PutUInt(value, 11);
            }
        }
    }
}
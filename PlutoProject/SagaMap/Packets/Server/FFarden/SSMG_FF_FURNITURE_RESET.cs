using SagaLib;

namespace SagaMap.Packets.Server.FFarden
{
    public class SSMG_FF_FURNITURE_RESET : Packet
    {
        public SSMG_FF_FURNITURE_RESET()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x2062;
        }

        public uint AID
        {
            set => PutUInt(value, 2);
        }

        public uint ActorID
        {
            set => PutUInt(value, 6);
        }

        public uint RindID
        {
            set => PutUInt(value, 10);
        }
    }
}
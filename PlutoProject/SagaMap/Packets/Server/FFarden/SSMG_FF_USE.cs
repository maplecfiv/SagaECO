using SagaLib;

namespace SagaMap.Packets.Server.FFGarden
{
    public class SSMG_FF_USE : Packet
    {
        public SSMG_FF_USE()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x205C;
        }

        public uint actorID
        {
            set => PutUInt(value, 2);
        }

        public ushort motion
        {
            set
            {
                PutUShort(value, 6);
                PutUShort(value, 8);
            }
        }
    }
}
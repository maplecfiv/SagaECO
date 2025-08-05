using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_WAIT : Packet
    {
        public SSMG_NPC_WAIT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x05EB;
        }

        public uint Wait
        {
            set => PutUInt(value, 2);
        }
    }
}
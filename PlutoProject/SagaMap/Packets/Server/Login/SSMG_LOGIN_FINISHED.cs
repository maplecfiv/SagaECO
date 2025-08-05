using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_LOGIN_FINISHED : Packet
    {
        public SSMG_LOGIN_FINISHED()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x1B67;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}
using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_JOB : Packet
    {
        public SSMG_PLAYER_JOB()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x0244;
        }

        public PC_JOB Job
        {
            set => PutUInt((uint)value, 2);
        }

        public PC_JOB JointJob
        {
            set => PutUInt((uint)value - 1000, 6);
        }

        public ushort DualJob
        {
            set => PutUShort(value, 10);
        }
    }
}
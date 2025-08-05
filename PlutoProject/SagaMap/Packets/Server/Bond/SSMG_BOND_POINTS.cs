using SagaLib;

namespace SagaMap.Packets.Server.Bond
{
    public class SSMG_BOND_POINTS : Packet
    {
        public SSMG_BOND_POINTS()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x1FEC;
        }

        public int TeachingPoint
        {
            set => PutInt(value, 2);
        }

        public int AchievementPoint
        {
            set => PutInt(value, 6);
        }

        public byte StudentLimit
        {
            set => PutByte(value, 10);
        }
    }
}
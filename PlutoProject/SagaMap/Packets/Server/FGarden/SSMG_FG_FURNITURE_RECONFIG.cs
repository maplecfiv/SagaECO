using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_FURNITURE_RECONFIG : Packet
    {
        public SSMG_FG_FURNITURE_RECONFIG()
        {
            if (Configuration.Instance.Version < Version.Saga11)
                data = new byte[14];
            else
                data = new byte[18];
            offset = 2;
            ID = 0x1C12;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public short X
        {
            set => PutShort(value, 6);
        }

        public short Y
        {
            set => PutShort(value, 8);
        }

        public short Z
        {
            set => PutShort(value, 10);
        }

        public ushort Dir
        {
            set
            {
                if (Configuration.Instance.Version < Version.Saga11)
                    PutUShort(value, 12);
                else
                    PutUShort(value, 14);
            }
        }
    }
}
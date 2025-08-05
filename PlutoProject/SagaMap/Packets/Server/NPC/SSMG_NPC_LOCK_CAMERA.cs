using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_LOCK_CAMERA : Packet
    {
        public SSMG_NPC_LOCK_CAMERA()
        {
            data = new byte[15];
            offset = 2;
            ID = 0x0619;
        }

        public short X
        {
            set => PutShort(value, 2);
        }

        public short Y
        {
            set => PutShort(value, 4);
        }

        public short Z
        {
            set => PutShort(value, 6);
        }

        public short Xdir
        {
            set => PutShort(value, 8);
        }

        public short Ydir
        {
            set => PutShort(value, 10);
        }

        public short CameraMoveSpeed
        {
            set => PutShort(value, 12);
        }

        public byte unknown
        {
            set => PutByte(value, 14);
        }
    }
}
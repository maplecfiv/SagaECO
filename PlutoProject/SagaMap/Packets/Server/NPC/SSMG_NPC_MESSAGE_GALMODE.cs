using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_MESSAGE_GALMODE : Packet
    {
        public SSMG_NPC_MESSAGE_GALMODE()
        {
            data = new byte[19];
            offset = 2;
            ID = 0x0606;
            if (Configuration.Configuration.Instance.Version <= Version.Saga18)
            {
                PutUInt(1, 2);
                PutUInt(1, 15);
            }
        }

        public uint Mode
        {
            set => PutUInt(value, 2);
            //0 normal mode
            //1 galgame mode
        }

        public UIType UIType
        {
            set
            {
                PutInt((int)value, 6);
                if (value != 0)
                    PutByte(0, 18);
                else
                    PutByte(1, 18);
            }
        }

        public int X
        {
            set => PutInt(value, 10);
        }

        public int Y
        {
            set => PutInt(value, 14);
        }

        public byte Unknown
        {
            set => PutByte(value, 18);
            //0 or 1, bool expected
        }
    }
}
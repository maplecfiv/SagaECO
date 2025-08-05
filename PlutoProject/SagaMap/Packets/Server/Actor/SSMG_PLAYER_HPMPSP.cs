using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_HPMPSP : Packet
    {
        public SSMG_PLAYER_HPMPSP()
        {
            data = new byte[35];
            offset = 2;
            ID = 0x021C;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint HP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version >= Version.Saga9)
                    //this.PutByte(4, 6);
                    PutByte(3, 6);
                else
                    PutByte(3, 6);
                /*if (value.type == ActorType.MOB)
                    this.PutUInt(value.HP, 7);
                else*/
                PutUInt(0, 7);
                PutUInt(value, 11);
                //this.PutUInt(0, 15);
            }
        }

        public uint MP
        {
            set
            {
                PutUInt(0, 15);
                PutUInt(value, 19);
                //this.PutUInt(0, 23);
            }
        }

        public uint SP
        {
            set
            {
                PutUInt(0, 23);
                PutUInt(value, 27);
            }
        }

        public uint EP
        {
            set => PutUInt(value, 31);
        }
    }
}
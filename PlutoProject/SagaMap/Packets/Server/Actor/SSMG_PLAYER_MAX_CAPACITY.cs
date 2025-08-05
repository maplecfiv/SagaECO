using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_MAX_CAPACITY : Packet
    {
        public SSMG_PLAYER_MAX_CAPACITY()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x0231;
        }

        public uint Payload
        {
            set => PutUInt(value, 2);
        }

        public uint Volume
        {
            set => PutUInt(value, 6);
        }


        /*public uint CapacityBody
        {
            set
            {
                this.PutByte(4, 2);
                this.PutUInt(value, 3);
            }
        }

        public uint CapacityRight
        {
            set
            {
                this.PutUInt(value, 7);
            }
        }

        public uint CapacityLeft
        {
            set
            {
                this.PutUInt(value, 11);
            }
        }

        public uint CapacityBack
        {
            set
            {
                this.PutUInt(value, 15);
            }
        }

        public uint PayloadBody
        {
            set
            {
                this.PutByte(4, 19);
                this.PutUInt(value, 20);
            }
        }

        public uint PayloadRight
        {
            set
            {
                this.PutUInt(value, 24);
            }
        }

        public uint PayloadLeft
        {
            set
            {
                this.PutUInt(value, 28);
            }
        }

        public uint PayloadBack
        {
            set
            {
                this.PutUInt(value, 32);
            }
        }*/
    }
}
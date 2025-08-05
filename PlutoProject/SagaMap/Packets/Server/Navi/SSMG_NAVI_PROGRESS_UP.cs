using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NAVI_PROGRESS_UP : Packet
    {
        private uint id;

        public SSMG_NAVI_PROGRESS_UP()
        {
            data = new byte[23];
            ID = 0x1EB0;
            offset = 2;
        }

        public uint NaviID
        {
            set
            {
                id = value;
                PutUInt(value, 2);
                PutByte(4, 6);
            }
        }

        public ActorPC pc
        {
            set
            {
                /*
                this.PutInt(value.Navi.UniqueSteps[id].BelongEvent.DisplaySteps);
                this.PutInt(0);
                this.PutInt(value.Navi.UniqueSteps[id].BelongEvent.FinishedSteps);
                this.PutInt(0);
                */
            }
        }
    }
}
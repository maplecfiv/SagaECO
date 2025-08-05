using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_EVENT_TITLE_CHANGE : Packet
    {
        public SSMG_ACTOR_EVENT_TITLE_CHANGE()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x0BBA;
        }

        public ActorEvent Actor
        {
            set
            {
                var title = Global.Unicode.GetBytes(value.Title + "\0");
                var buf = new byte[7 + title.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutUInt(value.ActorID, 2);
                PutByte((byte)title.Length);
                PutBytes(title);
            }
        }
    }
}
using SagaDB.Actor;
using SagaDB.Map;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_EVENT_APPEAR : Packet
    {
        public SSMG_ACTOR_EVENT_APPEAR()
        {
            data = new byte[19];
            offset = 2;
            ID = 0x0BB8;
        }

        public ActorEvent Actor
        {
            set
            {
                byte[] objName = null;
                var title = Global.Unicode.GetBytes(value.Title + "\0");
                var info = MapInfoFactory.Instance.MapInfo[value.MapID];

                switch (value.Type)
                {
                    case ActorEventTypes.ROPE:
                        objName = Global.Unicode.GetBytes("fg_rope_01\0");
                        break;
                    case ActorEventTypes.TENT:
                        objName = Global.Unicode.GetBytes("33_tent01\0");
                        break;
                }

                var buf = new byte[19 + objName.Length + title.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutUInt(value.ActorID, 2);
                PutByte((byte)objName.Length);
                PutBytes(objName);
                PutByte(Global.PosX16to8(value.X, info.width));
                PutByte(Global.PosY16to8(value.Y, info.height));
                switch (value.Type)
                {
                    case ActorEventTypes.ROPE:
                        PutByte(6);
                        break;
                    case ActorEventTypes.TENT:
                        PutByte(4);
                        break;
                }

                PutUInt(value.EventID);
                PutByte((byte)title.Length);
                PutBytes(title);
                PutUInt(value.Caster.CharID);
            }
        }
    }
}
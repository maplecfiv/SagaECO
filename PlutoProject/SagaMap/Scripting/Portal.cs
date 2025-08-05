using SagaDB.Actor;

namespace SagaMap.Scripting
{
    public abstract class Portal : Event
    {
        public uint mapID;
        public byte x;
        public byte y;

        public void Init(uint eventid, uint mapid, byte x, byte y)
        {
            EventID = eventid;
            mapID = mapid;
            this.x = x;
            this.y = y;
        }

        public override void OnEvent(ActorPC pc)
        {
            Warp(pc, mapID, x, y);
        }
    }
}
namespace SagaDB.Actor
{
    public class ActorEvent : Actor
    {
        private string title;

        public ActorEvent(ActorPC caster)
        {
            type = ActorType.EVENT;
            Caster = caster;
        }

        public ActorEventTypes Type { get; set; }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                if (e != null) e.PropertyUpdate(UpdateEvent.EVENT_TITLE, 0);
            }
        }

        public uint EventID { get; set; }

        public ActorPC Caster { get; }

        public uint TentMapID { get; set; }
    }
}
namespace SagaMap.Scripting
{
    public abstract class EventActor : Event
    {
        public ActorEvent Actor { get; set; }

        public abstract EventActor Clone();
    }
}
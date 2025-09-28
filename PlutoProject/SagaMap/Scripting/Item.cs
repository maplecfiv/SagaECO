using SagaMap.Manager;

namespace SagaMap.Scripting
{
    public class Item : Event
    {
        public delegate void OnEventHandler(ActorPC pc);

        public OnEventHandler Handler;

        protected void Init(uint eventID, OnEventHandler handler)
        {
            var newItem = new Item();
            newItem.EventID = eventID;
            newItem.Handler += handler;
            if (!ScriptManager.Instance.Events.ContainsKey(eventID))
                ScriptManager.Instance.Events.Add(eventID, newItem);
        }

        public override void OnEvent(ActorPC pc)
        {
            if (Handler != null)
                Handler.Invoke(pc);
        }
    }
}
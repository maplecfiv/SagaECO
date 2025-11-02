using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Scripting;

public class CustomEvent(EventData eventData) : Event {
    public SagaLib.EventData EventData = eventData;

    public override void OnEvent(ActorPC pc) {
        EventProgress eventProgress = EventData.EventProgresses[SagaLib.EventData.KeyEventEntrypoint];
        while (true) {
            switch (eventProgress.Type) {
                case EventProgressType.DIALOG:
                    foreach (var message in eventProgress.Data["message"]) {
                        Say(pc, 131, message.Replace("$CHARACTER_NAME", pc.Name), eventProgress.Data["sender"][0]);
                    }

                    break;
                case EventProgressType.TELEPORT:
                    byte x = (byte)Random(byte.Parse(eventProgress.Data["x"][0]),
                        byte.Parse(eventProgress.Data["x"][1]));
                    byte y = (byte)Random(byte.Parse(eventProgress.Data["y"][0]),
                        byte.Parse(eventProgress.Data["y"][1]));
                    Warp(pc, uint.Parse(eventProgress.Data["map_id"][0]), x, y);
                    break;
            }

            if (eventProgress.Next == null) {
                break;
            }


            eventProgress = EventData.EventProgresses[eventProgress.Next];
        }
    }
}
using SagaDB.Actor;
using SagaMap.Packets.Server;

namespace SagaMap.Scripting
{
    public abstract partial class Event
    {
        protected void OpenPprotectListOpen(ActorPC pc)
        {
            var client = GetMapClient(pc);

            var p = new SSMG_PPROTECT_INITI();

            client.netIO.SendPacket(p);
        }
    }
}
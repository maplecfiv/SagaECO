using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Quest
{
    public class CSMG_DAILYDUNGEON_OPEN : Packet
    {
        public CSMG_DAILYDUNGEON_OPEN()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_DAILYDUNGEON_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDailyDungeonOpen();
        }
    }
}
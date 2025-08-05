using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Quest
{
    public class CSMG_DAILYDUNGEON_JOIN : Packet
    {
        public CSMG_DAILYDUNGEON_JOIN()
        {
            offset = 2;
        }

        public byte QID => GetByte(2);

        public override Packet New()
        {
            return new CSMG_DAILYDUNGEON_JOIN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDailyDungeonJoin(this);
        }
    }
}
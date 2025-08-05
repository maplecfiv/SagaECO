using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_EQUIP_OPEN : Packet
    {
        public CSMG_PLAYER_EQUIP_OPEN()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PLAYER_EQUIP_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerEquipOpen(ActorID);
        }
    }
}
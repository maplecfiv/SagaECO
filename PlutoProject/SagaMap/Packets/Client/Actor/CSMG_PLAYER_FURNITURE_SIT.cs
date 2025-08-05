using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PLAYER_FURNITURE_SIT : Packet
    {
        public CSMG_PLAYER_FURNITURE_SIT()
        {
            data = new byte[10];
            offset = 2;
        }

        public uint FurnitureID => GetUInt(2);

        public int unknown => GetInt(6);


        public override Packet New()
        {
            return new CSMG_PLAYER_FURNITURE_SIT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPlayerFurnitureSit(this);
        }
    }
}
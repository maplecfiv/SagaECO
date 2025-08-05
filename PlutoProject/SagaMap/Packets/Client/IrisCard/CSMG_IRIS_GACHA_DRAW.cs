using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_IRIS_GACHA_DRAW : Packet
    {
        public CSMG_IRIS_GACHA_DRAW()
        {
            offset = 2;
        }

        public uint PayFlag => GetUInt(2);

        public uint SessionID => GetUInt(6);

        public uint ItemID => GetUInt(10);

        public override Packet New()
        {
            return new CSMG_IRIS_GACHA_DRAW();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisGacha(this);
        }
    }
}
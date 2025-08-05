using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_DEM_DEMIC_CONFIRM : Packet
    {
        public CSMG_DEM_DEMIC_CONFIRM()
        {
            offset = 2;
        }

        public byte Page => GetByte(2);

        public short[,] Chips
        {
            get
            {
                var res = new short[9, 9];
                offset = 4;
                for (var i = 0; i < 9; i++)
                for (var j = 0; j < 9; j++)
                    res[j, i] = GetShort();

                return res;
            }
        }

        public override Packet New()
        {
            return new CSMG_DEM_DEMIC_CONFIRM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDEMDemicConfirm(this);
        }
    }
}
using SagaLib;
using SagaMap.Scripting;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_CREATE_MATERIAL : Packet
    {
        public SSMG_FG_CREATE_MATERIAL()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1C34;
        }

        public BitMask<FGardenParts> Parts
        {
            set => PutInt(value.Value, 2);
        }
    }
}
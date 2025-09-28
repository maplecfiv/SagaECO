using SagaLib;

namespace SagaMap.Packets.Server.FFGarden
{
    public class SSMG_FF_MATERIAL_CONSUME : Packet
    {
        /// <summary>
        ///     飞空城的材料点数消耗(マテリアルコスト)
        /// </summary>
        public SSMG_FF_MATERIAL_CONSUME()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x201D;
        }

        public uint value
        {
            set => PutUInt(value, 2);
        }
    }
}
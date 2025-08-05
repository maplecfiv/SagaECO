using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FF_MATERIAL_POINT : Packet
    {
        /// <summary>
        ///     飞空城的所持材料数(所持マテリアルポイント)
        /// </summary>
        public SSMG_FF_MATERIAL_POINT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x201C;
        }

        public uint value
        {
            set => PutUInt(value, 2);
        }
    }
}
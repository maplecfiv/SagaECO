using SagaLib;

namespace SagaMap.Packets.Server
{
    /// <summary>
    ///     角色EX 属性/技能点的情况
    /// </summary>
    public class SSMG_PLAYER_EXPOINT : Packet
    {
        public SSMG_PLAYER_EXPOINT()
        {
            data = new byte[13];
            offset = 2;
            ID = 0x0695;
        }

        public ushort EXStatPoint
        {
            set => PutUShort(value, 2);
        }

        public ushort CanUseStatPoint
        {
            set => PutUShort(value, 4);
        }

        public byte EXSkillPoint
        {
            set => PutByte(value, 6);
        }
    }
}
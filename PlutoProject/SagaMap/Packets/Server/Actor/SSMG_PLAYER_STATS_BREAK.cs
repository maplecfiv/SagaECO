using System.ComponentModel;
using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    /// <summary>
    ///     str110 = 1
    ///     dex110 = 2
    ///     int110 = 4
    ///     dex110 = 8
    ///     agi110 = 16
    ///     mag110 = 32
    /// </summary>
    public class SSMG_PLAYER_STATS_BREAK : Packet
    {
        public SSMG_PLAYER_STATS_BREAK()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x025D;
        }

        public byte STATS
        {
            set => PutByte(value, 2);
        }
    }

    public enum StatsBreakType
    {
        [Description("力量")] Str = 0x1,
        [Description("灵巧")] Dex = 0x2,
        [Description("智慧")] Int = 0x4,
        [Description("体力")] Vit = 0x8,
        [Description("敏捷")] Agi = 0x16,
        [Description("魔力")] Mag = 0x32
    }
}
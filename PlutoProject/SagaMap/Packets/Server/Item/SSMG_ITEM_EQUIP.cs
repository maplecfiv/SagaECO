using SagaDB.Item;
using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_EQUIP : Packet
    {
        public SSMG_ITEM_EQUIP()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x09E8;
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 2);
        }

        public ContainerType Target
        {
            set => PutByte((byte)value, 6);
        }

        /// <summary>
        ///     GAME_SMSG_ITEM_EQUIPERR2,";装備条件を満たしていません";
        ///     GAME_SMSG_ITEM_EQUIPERR3,";行動不能時は装備を変更できません";
        ///     GAME_SMSG_ITEM_EQUIPERR4,";イベント中は装備を変更できません";
        ///     GAME_SMSG_ITEM_EQUIPERR5,";ゴーレムショップに出品中のアイテムは装備出来ません";
        ///     GAME_SMSG_ITEM_EQUIPERR6,";弓を装備していません";
        ///     GAME_SMSG_ITEM_EQUIPERR7,";銃を装備していません";
        ///     GAME_SMSG_ITEM_EQUIPERR8,";トレード中は装備を変更出来ません";
        ///     GAME_SMSG_ITEM_EQUIPERR9,";未鑑定品を装備する事は出来ません";
        ///     GAME_SMSG_ITEM_EQUIPERR10,";憑依中は装備を変更できません";
        ///     GAME_SMSG_ITEM_EQUIPERR11,";憑依者待機中は装備を変更できません";
        ///     GAME_SMSG_ITEM_EQUIPERR12,";故障品は装備できません";
        ///     GAME_SMSG_ITEM_EQUIPERR13,";装備するには種族を変えて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR14,";装備するには性別を変えて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR15,";装備するにはレベルを上げて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR16,";装備するにはSTRを上げて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR17,";装備するにはMAGを上げて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR18,";装備するにはVITを上げて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR19,";装備するにはDEXを上げて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR20,";装備するにはAGIを上げて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR21,";装備するにはINTを上げて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR22,";運が足りない為装備できません";
        ///     GAME_SMSG_ITEM_EQUIPERR23,";魅力が足りない為装備できません";
        ///     GAME_SMSG_ITEM_EQUIPERR24,";装備するには職業を変えて下さい";
        ///     GAME_SMSG_ITEM_EQUIPERR25,";違う組織専用の装備品です";
        ///     #26は状態異常中
        ///     GAME_SMSG_ITEM_EQUIPERR27,";眠っている為出現させることが出来ません";
        ///     GAME_SMSG_ITEM_EQUIPERR28,";パイロットの資格が無いため騎乗できません";
        /// </summary>
        public int Result
        {
            set => PutByte((byte)value, 7);
        }

        public uint Range
        {
            set => PutUInt(value, 8);
        }
    }
}
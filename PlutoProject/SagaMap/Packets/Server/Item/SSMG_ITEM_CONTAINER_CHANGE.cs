using SagaDB.Item;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ITEM_CONTAINER_CHANGE : Packet
    {
        public SSMG_ITEM_CONTAINER_CHANGE()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x09E3;
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 2);
        }

        /// <summary>
        ///     GAME_SMSG_ITEM_MOVEERR1,";存在しないアイテムです";
        ///     GAME_SMSG_ITEM_MOVEERR2,";アイテム数が不足しています";
        ///     GAME_SMSG_ITEM_MOVEERR3,";アイテムを移動することが出来ません";
        ///     GAME_SMSG_ITEM_MOVEERR4,";憑依中は装備を解除することが出来ません";
        ///     GAME_SMSG_ITEM_MOVEERR5,";これ以上アイテムを所持することはできません";
        ///     GAME_SMSG_ITEM_MOVEERR6,";憑依者待機中は装備を解除することが出来ません";
        ///     GAME_SMSG_ITEM_MOVEERR7,";トレード中はアイテムを移動出来ません";
        ///     GAME_SMSG_ITEM_MOVEERR8,";イベント中はアイテムを移動できません";
        /// </summary>
        public int Result
        {
            set => PutByte((byte)value, 6);
        }

        public ContainerType Target
        {
            set => PutByte((byte)value, 7);
        }
    }
}
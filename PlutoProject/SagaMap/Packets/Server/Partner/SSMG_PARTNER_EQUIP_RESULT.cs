using SagaDB.Partner;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTNER_EQUIP_RESULT : Packet
    {
        public SSMG_PARTNER_EQUIP_RESULT()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x218B;
        }

        public uint PartnerInventorySlot
        {
            set => PutUInt(value, 2);
        }

        public uint EquipItemID
        {
            set => PutUInt(value, 6);
        }

        /// <summary>
        ///     0 for weapon, 1 for costume
        /// </summary>
        public EnumPartnerEquipSlot PartnerEquipSlot
        {
            set
            {
                if (value == EnumPartnerEquipSlot.WEAPON)
                    PutByte(0, 10);
                else
                    PutByte(1, 10);
            }
        }

        /// <summary>
        ///     0 for in, 1 for out
        /// </summary>
        public byte MoveType
        {
            set => PutByte(value, 11);
        }
    }
}
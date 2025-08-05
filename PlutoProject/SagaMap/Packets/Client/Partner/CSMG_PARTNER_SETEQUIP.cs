using SagaDB.Partner;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PARTNER_SETEQUIP : Packet
    {
        public CSMG_PARTNER_SETEQUIP()
        {
            offset = 2;
        }

        public uint PartnerInventorySlot
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        /// <summary>
        ///     postivie slot id to equip, -1 to unequip
        /// </summary>
        public uint EquipItemInventorySlot
        {
            get => GetUInt(6);
            set => PutUInt(value, 6);
        }

        /// <summary>
        ///     0 for weapon, 1 for costume
        /// </summary>
        public EnumPartnerEquipSlot PartnerEquipSlot
        {
            get
            {
                var peqslot = GetByte(10);
                if (peqslot == 0) return EnumPartnerEquipSlot.WEAPON;

                return EnumPartnerEquipSlot.COSTUME;
            }
            set
            {
                if (value == EnumPartnerEquipSlot.WEAPON)
                    PutByte(0, 10);
                else
                    PutByte(1, 10);
            }
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_SETEQUIP();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartnerItemEquipt(this);
        }
    }
}
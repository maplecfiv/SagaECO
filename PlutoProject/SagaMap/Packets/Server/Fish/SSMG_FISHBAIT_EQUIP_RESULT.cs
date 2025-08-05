using SagaLib;

namespace SagaMap.Packets.Server.Fish
{
    public class SSMG_FISHBAIT_EQUIP_RESULT : Packet
    {
        public SSMG_FISHBAIT_EQUIP_RESULT()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x216D;
        }

        public uint InventoryID
        {
            set => PutUInt(value, 2);
        }

        public byte IsEquip
        {
            set => PutByte(value, 6);
        }
    }
}
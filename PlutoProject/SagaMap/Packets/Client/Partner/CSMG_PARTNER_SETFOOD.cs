using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PARTNER_SETFOOD : Packet
    {
        public CSMG_PARTNER_SETFOOD()
        {
            offset = 2;
        }

        /// <summary>
        ///     1 to get into the food list, 0 to get out of the food list
        /// </summary>
        public byte MoveType
        {
            get => GetByte(2);
            set => PutByte(value, 2);
        }

        public uint ItemID
        {
            get => GetUInt(3);
            set => PutUInt(value, 3);
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_SETFOOD();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartnerFoodListSet(this);
        }
    }
}
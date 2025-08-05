using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Partner
{
    public class CSMG_PARTNER_AI_MODE_SELECTION : Packet
    {
        public CSMG_PARTNER_AI_MODE_SELECTION()
        {
            offset = 2;
        }

        public uint PartnerInventorySlot
        {
            get => GetUInt(2);
            set => PutUInt(value, 2);
        }

        /// <summary>
        ///     start at 0
        /// </summary>
        public byte AIMode
        {
            get => GetByte(6);
            set => PutByte(value, 6);
        }

        public override Packet New()
        {
            return new CSMG_PARTNER_AI_MODE_SELECTION();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPartnerAIModeSelection(this);
        }
    }
}
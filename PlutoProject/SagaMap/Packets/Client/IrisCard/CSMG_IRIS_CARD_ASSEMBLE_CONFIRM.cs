using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.IrisCard
{
    public class CSMG_IRIS_CARD_ASSEMBLE_CONFIRM : Packet
    {
        public CSMG_IRIS_CARD_ASSEMBLE_CONFIRM()
        {
            offset = 2;
        }

        public uint CardID => GetUInt(2);

        public uint SupportItem => GetUInt(6);


        public uint ProtectItem => GetUInt(10);


        public byte BaseLevel => GetByte(14);

        public byte JobLevel => GetByte(15);

        public ushort ExpRate => GetUShort(16);

        public ushort JExpRate => GetUShort(18);

        public override Packet New()
        {
            return new CSMG_IRIS_CARD_ASSEMBLE_CONFIRM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnIrisCardAssemble(this);
        }
    }
}
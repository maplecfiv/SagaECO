using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PPROTECT_ADD_1 : Packet
    {
        public CSMG_PPROTECT_ADD_1()
        {
            offset = 2;
        }

        public uint PPID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_PPROTECT_ADD_1();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPProtectADD1(this);
        }
    }
}
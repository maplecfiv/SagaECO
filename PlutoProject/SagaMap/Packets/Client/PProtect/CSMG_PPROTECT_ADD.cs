using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PPROTECT_ADD : Packet
    {
        public CSMG_PPROTECT_ADD()
        {
            offset = 2;
        }

        public uint PPID => GetUInt(2);


        public string Password
        {
            get
            {
                var inedx = GetByte(6);
                var buf = GetBytes((ushort)(inedx - 1), 7);
                return Global.Unicode.GetString(buf);
            }
        }

        public override Packet New()
        {
            return new CSMG_PPROTECT_ADD();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPProtectADD(this);
        }
    }
}
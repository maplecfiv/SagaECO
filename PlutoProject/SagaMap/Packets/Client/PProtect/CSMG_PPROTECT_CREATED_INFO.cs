using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PPROTECT_CREATED_INFO : Packet
    {
        public byte maxMember;
        public string message;
        public string name;
        public string password;
        public uint taskID;

        public CSMG_PPROTECT_CREATED_INFO()
        {
            offset = 2;
        }

        private void load()
        {
            offset = 2;
            var inedx = GetByte();
            var buf = GetBytes((ushort)(inedx - 1));
            offset += 1;
            name = Global.Unicode.GetString(buf);
            inedx = GetByte();
            buf = GetBytes((ushort)(inedx - 1));
            offset += 1;
            message = Global.Unicode.GetString(buf);
            inedx = GetByte();
            buf = GetBytes((ushort)(inedx - 1));
            offset += 1;
            password = Global.Unicode.GetString(buf);
            taskID = GetUInt();
            maxMember = GetByte();
        }


        public override Packet New()
        {
            return new CSMG_PPROTECT_CREATED_INFO();
        }

        public override void Parse(SagaLib.Client client)
        {
            load();
            ((MapClient)client).OnPProtectCreated(this);
        }
    }
}
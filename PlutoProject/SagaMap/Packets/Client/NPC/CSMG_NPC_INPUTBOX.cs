using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.NPC
{
    public class CSMG_NPC_INPUTBOX : Packet
    {
        public CSMG_NPC_INPUTBOX()
        {
            offset = 2;
        }

        public string Content => Global.Unicode.GetString(GetBytes(GetByte(2), 3)).Replace("\0", "");

        public override Packet New()
        {
            return new CSMG_NPC_INPUTBOX();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNPCInputBox(this);
        }
    }
}
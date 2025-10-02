using SagaLib;
using SagaValidation.Network.Client;

namespace SagaValidation.Packets.Client
{
    public class CSMG_LOGIN : Packet
    {
        public string UserName;
        public string Password;
        public CSMG_LOGIN()
        {
            size = 55;//JP用 12/4Updateで64Byteに変更された
            offset = 8;
        }

        public override Packet New()
        {
            return (Packet)new CSMG_LOGIN();
        }

        public void GetContent()
        {
            byte size;
            ushort offset = 2;
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            size = GetByte(offset);
            offset++;
            UserName = enc.GetString(GetBytes((ushort)(size - 1), offset));
            offset += size;
            size = GetByte(offset);
            offset++;
            Password = enc.GetString(GetBytes((ushort)(size - 1), offset));
        }

        public override void Parse(SagaLib.Client client)
        {
            //((LoginClient)(client)).NewOnLogin(this);
            ((ValidationClient)(client)).OnLogin(this);
        }

    }
}
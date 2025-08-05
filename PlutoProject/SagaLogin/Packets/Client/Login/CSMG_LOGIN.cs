using System;
using System.Text;
using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client.Login
{
    public class CSMG_LOGIN : Packet
    {
        public string MacAddress;
        public string Password;
        public string UserName;

        public CSMG_LOGIN()
        {
            size = 55; //JP用 12/4Updateで64Byteに変更された
            offset = 8;
        }

        public override Packet New()
        {
            return new CSMG_LOGIN();
        }

        public void GetContent()
        {
            byte size;
            ushort offset = 2;
            var enc = Encoding.ASCII;
            size = GetByte(offset);
            offset++;
            UserName = enc.GetString(GetBytes((ushort)(size - 1), offset));
            offset += size;
            size = GetByte(offset);
            offset++;
            Password = enc.GetString(GetBytes((ushort)(size - 1), offset));
            offset++;
            offset += size;
            var a = GetUShort(offset);
            offset += 2;
            var b = GetUInt(offset);
            MacAddress = Convert.ToString(a, 16) + Convert.ToString(b, 16);
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnLogin(this);
        }
    }
}
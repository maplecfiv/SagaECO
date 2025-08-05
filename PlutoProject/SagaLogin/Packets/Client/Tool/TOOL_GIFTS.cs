using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client.Tool
{
    public class TOOL_GIFTS : Packet
    {
        public TOOL_GIFTS()
        {
            offset = 2;
        }

        public byte type => GetByte(2);

        public string Title
        {
            get
            {
                var size = GetByte(4);
                var buf = Global.Unicode.GetString(GetBytes(size, 5));
                return buf.Replace("\0", "");
            }
        }

        public string Sender
        {
            get
            {
                var size = GetByte();
                var buf = Global.Unicode.GetString(GetBytes(size));
                return buf.Replace("\0", "");
            }
        }

        public string Content
        {
            get
            {
                var size = GetByte();
                var buf = Global.Unicode.GetString(GetBytes(size));
                return buf.Replace("\0", "");
            }
        }

        public string CharIDs
        {
            get
            {
                var size = GetByte();
                var buf = Global.Unicode.GetString(GetBytes(size));
                return buf.Replace("\0", "");
            }
        }

        public string GiftIDs
        {
            get
            {
                var size = GetByte();
                var buf = Global.Unicode.GetString(GetBytes(size));
                return buf.Replace("\0", "");
            }
        }

        public string Days
        {
            get
            {
                var size = GetByte();
                var buf = Global.Unicode.GetString(GetBytes(size));
                return buf.Replace("\0", "");
            }
        }

        public override Packet New()
        {
            return new TOOL_GIFTS();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnGetGiftsRequest(this);
        }
    }
}
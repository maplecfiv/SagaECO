using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_POSSESSION_REQUEST : Packet
    {
        public CSMG_POSSESSION_REQUEST()
        {
            offset = 2;
        }

        public uint ActorID => GetUInt(2);

        public PossessionPosition PossessionPosition => (PossessionPosition)GetByte(6);

        public string Comment
        {
            get
            {
                var len = GetByte(7);
                var buf = GetBytes(len, 8);
                var tmp = Global.Unicode.GetString(buf);
                tmp = tmp.Replace("\0", "");
                return tmp;
            }
        }

        public override Packet New()
        {
            return new CSMG_POSSESSION_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPossessionRequest(this);
        }
    }
}
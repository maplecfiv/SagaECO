using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Ring
{
    public class CSMG_RING_EMBLEM_UPLOAD : Packet
    {
        public CSMG_RING_EMBLEM_UPLOAD()
        {
            offset = 2;
        }

        public byte[] Data
        {
            get
            {
                if (GetInt(3) == 0xFD)
                {
                    var len = GetInt(7);
                    var buf = GetBytes((ushort)len, 11);
                    return buf;
                }
                else
                {
                    var len = GetInt(3);
                    var buf = GetBytes((ushort)len, 7);
                    return buf;
                }
            }
        }

        public override Packet New()
        {
            return new CSMG_RING_EMBLEM_UPLOAD();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRingEmblemUpload(this);
        }
    }
}
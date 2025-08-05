using System;
using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_RING_EMBLEM : Packet
    {
        public CSMG_RING_EMBLEM()
        {
            size = 6;
            offset = 2;
        }

        public uint RingID => GetUInt(2);

        public DateTime UpdateTime
        {
            get
            {
                var date = new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(GetUInt(6));
                return date;
            }
        }

        public override Packet New()
        {
            return new CSMG_RING_EMBLEM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnRingEmblem(this);
        }
    }
}
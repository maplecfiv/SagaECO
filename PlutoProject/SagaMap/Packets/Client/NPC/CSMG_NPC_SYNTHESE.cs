using System.Collections.Generic;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.NPC
{
    public class CSMG_NPC_SYNTHESE : Packet
    {
        public CSMG_NPC_SYNTHESE()
        {
            offset = 2;
        }

        public Dictionary<uint, ushort> SynIDs
        {
            get
            {
                var count = GetByte(2);
                var ids = new Dictionary<uint, ushort>();
                for (var i = 0; i < count; i++)
                {
                    var id = GetUInt((ushort)(3 + i * 4));
                    var c = GetUShort((ushort)(5 + count * 8 + i * 2));
                    ids.Add(id, c);
                }

                return ids;
            }
        }

        public override Packet New()
        {
            return new CSMG_NPC_SYNTHESE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNPCSynthese(this);
        }
    }
}
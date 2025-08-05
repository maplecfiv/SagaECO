using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.DualJob
{
    public class CSMG_DUALJOB_CHANGE_CONFIRM : Packet
    {
        public CSMG_DUALJOB_CHANGE_CONFIRM()
        {
            offset = 2;
        }

        public byte DualJobID => byte.Parse(GetShort(offset).ToString());

        public uint[] DualJobSkillList
        {
            get
            {
                var skills = new uint[GetByte(offset)];

                for (var i = 0; i < skills.Length; i++)
                {
                    var x = GetShort(offset).ToString();
                    if (x == "-1")
                        skills[i] = 0;
                    else
                        skills[i] = uint.Parse(x);
                }

                return skills;
            }
        }

        public override Packet New()
        {
            return new CSMG_DUALJOB_CHANGE_CONFIRM();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnDualChangeRequest(this);
        }
    }
}
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.Party
{
    public class SSMG_PARTY_MEMBER_BUFF : Packet
    {
        public SSMG_PARTY_MEMBER_BUFF()
        {
            if (Configuration.Configuration.Instance.Version < Version.Saga11)
                data = new byte[42];
            else
                data = new byte[58];
            offset = 2;
            ID = 0x19FA;
        }

        public ActorPC Actor
        {
            set
            {
                PutUInt(value.Party.IndexOf(value), 2);
                PutUInt(value.CharID, 6);
                PutInt(value.Buff.Buffs[0].Value, 10);
                PutInt(value.Buff.Buffs[1].Value, 14);
                PutInt(value.Buff.Buffs[2].Value, 18);
                PutInt(value.Buff.Buffs[3].Value, 22);
                PutInt(value.Buff.Buffs[4].Value, 26);
                PutInt(value.Buff.Buffs[5].Value, 30);
                PutInt(value.Buff.Buffs[6].Value, 34);
                PutInt(value.Buff.Buffs[7].Value, 38);
                PutInt(value.Buff.Buffs[8].Value, 42);
                PutInt(value.Buff.Buffs[9].Value, 46);
            }
        }
    }
}
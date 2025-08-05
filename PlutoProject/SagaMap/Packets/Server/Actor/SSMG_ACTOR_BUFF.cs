using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_BUFF : Packet
    {
        public SSMG_ACTOR_BUFF()
        {
            if (Configuration.Instance.Version >= Version.Saga17)
                data = new byte[54];
            else if (Configuration.Instance.Version >= Version.Saga14_2)
                data = new byte[46];
            else if (Configuration.Instance.Version >= Version.Saga11)
                data = new byte[42];
            else
                data = new byte[38];
            offset = 2;
            ID = 0x157C;
        }

        public Actor Actor
        {
            set
            {
                PutUInt(value.ActorID, 2);
                PutInt(value.Buff.Buffs[0].Value, 6);
                PutInt(value.Buff.Buffs[1].Value, 10);
                PutInt(value.Buff.Buffs[2].Value, 14);
                PutInt(value.Buff.Buffs[3].Value, 18);
                PutInt(value.Buff.Buffs[4].Value, 22);
                PutInt(value.Buff.Buffs[5].Value, 26);
                PutInt(value.Buff.Buffs[6].Value, 30);
                PutInt(value.Buff.Buffs[7].Value, 34);
                if (Configuration.Instance.Version >= Version.Saga11)
                    PutInt(value.Buff.Buffs[8].Value, 38);
                if (Configuration.Instance.Version >= Version.Saga14_2)
                    PutInt(value.Buff.Buffs[9].Value, 42);
                if (Configuration.Instance.Version >= Version.Saga14_2)
                    PutInt(value.Buff.Buffs[10].Value, 46);
                if (Configuration.Instance.Version >= Version.Saga14_2)
                    PutInt(value.Buff.Buffs[11].Value, 50);
            }
        }
    }
}
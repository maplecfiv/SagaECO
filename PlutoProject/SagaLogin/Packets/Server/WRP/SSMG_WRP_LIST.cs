using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaLogin.Packets.Server.WRP
{
    public class SSMG_WRP_LIST : Packet
    {
        public SSMG_WRP_LIST()
        {
            data = new byte[9];
            ID = 0x0173;
        }

        public List<ActorPC> RankingList
        {
            set
            {
                var names = new byte[value.Count][];
                var lvs = new byte[value.Count];
                var jlvs = new byte[value.Count];
                var jobs = new byte[value.Count];
                var wrps = new int[value.Count];
                var types = new uint[value.Count];

                var count = 0;
                foreach (var i in value)
                {
                    names[count] = Global.Unicode.GetBytes(i.Name);
                    lvs[count] = i.DominionLevel;
                    jlvs[count] = i.DominionJobLevel;
                    jobs[count] = (byte)i.Job;
                    wrps[count] = i.WRP;
                    if (count == 0)
                        types[count] = 10;
                    else if (count < 10)
                        types[count] = 1;
                    else
                        types[count] = 0;
                    count++;
                }

                var len = 0;
                foreach (var i in names) len += i.Length;
                data = new byte[9 + 16 * value.Count + len];
                ID = 0x0173;
                offset = 2;

                PutByte((byte)value.Count);
                for (var i = 1; i <= value.Count; i++) PutInt(i);

                PutByte((byte)value.Count);
                foreach (var i in names)
                {
                    PutByte((byte)i.Length);
                    PutBytes(i);
                }

                PutByte((byte)value.Count);
                foreach (var i in lvs) PutByte(i);
                PutByte((byte)value.Count);
                foreach (var i in jlvs) PutByte(i);
                PutByte((byte)value.Count);
                foreach (var i in jobs) PutByte(i);
                PutByte((byte)value.Count);
                foreach (var i in wrps) PutInt(i);
                PutByte((byte)value.Count);
                foreach (var i in types) PutUInt(i);
            }
        }
    }
}
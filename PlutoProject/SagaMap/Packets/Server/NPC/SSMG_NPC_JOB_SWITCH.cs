using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_JOB_SWITCH : Packet
    {
        public SSMG_NPC_JOB_SWITCH()
        {
            data = new byte[16];
            offset = 2;
            ID = 0x02BC;
        }

        public PC_JOB Job
        {
            set => PutUShort((ushort)value, 2);
        }

        public byte LevelReduced
        {
            set => PutByte(value, 4);
        }

        public byte Level
        {
            set => PutByte(value, 5);
        }

        public uint LevelItem
        {
            set => PutUInt(value, 6);
        }

        public uint ItemCount
        {
            set => PutUInt(value, 10);
        }

        public ushort PossibleReserveSkills
        {
            set => PutUShort(value, 14);
        }

        public List<SagaDB.Skill.Skill> PossibleSkills
        {
            set
            {
                var buff = new byte[18 + 3 * value.Count];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)value.Count, (ushort)16);
                PutByte((byte)value.Count, (ushort)(17 + 2 * value.Count));

                var j = 0;
                foreach (var i in value)
                {
                    PutUShort((ushort)i.ID, (ushort)(17 + 2 * j));
                    PutByte(i.Level, (ushort)(18 + 2 * value.Count + j));
                    j++;
                }
            }
        }
    }
}
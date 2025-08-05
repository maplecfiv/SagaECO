using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.DualJob
{
    public class SSMG_DUALJOB_WINDOW_OPEN : Packet
    {
        private int packetoffset;

        public SSMG_DUALJOB_WINDOW_OPEN()
        {
            data = new byte[87];
            offset = 2;
            ID = 0x22CE;
        }

        public byte CanChange
        {
            set => PutByte(value, 2);
        }

        public byte[] DualJobLevel
        {
            set
            {
                PutByte(0x0C, packetoffset);
                packetoffset++;
                PutBytes(value, packetoffset);
                packetoffset += 12;
            }
        }

        public byte CurrentDualJobSerial
        {
            set
            {
                PutUShort(value, packetoffset);
                packetoffset += 2;
            }
        }

        public List<SagaDB.Skill.Skill> CurrentSkillList
        {
            set
            {
                PutByte(byte.Parse(value.Count.ToString()), packetoffset++);
                for (var i = 0; i < value.Count; i++)
                {
                    PutUShort((ushort)value[i].ID, packetoffset);
                    packetoffset += 2;
                }
            }
        }

        public void SetDualJobList(byte jobCount, byte[] jobSerialList)
        {
            PutByte(jobCount, 3);
            PutBytes(jobSerialList, 4);
            packetoffset = 4 + jobCount * 2;
        }
    }
}
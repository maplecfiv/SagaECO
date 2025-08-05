using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_STATES : Packet
    {
        public SSMG_NPC_STATES()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x05E2;
        }


        public void PutNPCStates(Dictionary<uint, bool> NPCStates)
        {
            var npcs = new List<uint>(NPCStates.Keys);
            var states = new List<bool>(NPCStates.Values);

            var buf = new byte[(byte)(data.Length + NPCStates.Count * 5 + 2)];
            data.CopyTo(buf, 0);
            data = buf;
            offset = 2;

            PutByte((byte)NPCStates.Count, offset);
            offset++;
            foreach (var npc in npcs)
            {
                PutUInt(npc, offset);
                offset += 4;
            }

            PutByte((byte)NPCStates.Count, offset);
            offset++;
            foreach (var visible in states)
            {
                if (visible)
                    PutByte(0x01, offset);
                else
                    PutByte(0x00, offset);
                offset++;
            }
        }
    }
}
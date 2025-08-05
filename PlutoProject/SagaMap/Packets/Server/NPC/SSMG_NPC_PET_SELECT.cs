using System.Collections.Generic;
using SagaDB.Item;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_PET_SELECT : Packet
    {
        public enum SelType
        {
            Recover,
            Rebirth,
            None
        }

        public SSMG_NPC_PET_SELECT()
        {
            data = new byte[9];
            offset = 2;
            ID = 0x12CA;
        }

        public SelType Type
        {
            set => PutInt((int)value, 2);
        }

        public List<Item> Pets
        {
            set
            {
                var names = new byte[value.Count][];
                var slots = new uint[value.Count];

                byte len = 0;
                for (var i = 0; i < names.Length; i++)
                {
                    names[i] = Global.Unicode.GetBytes(value[i].BaseData.name);
                    len += (byte)(names[i].Length + 1);
                    slots[i] = value[i].Slot;
                }

                var buf = new byte[9 + value.Count * 8 + len];
                data.CopyTo(buf, 0);
                data = buf;

                offset = 6;
                PutByte((byte)slots.Length);
                for (var i = 0; i < value.Count; i++) PutUInt(slots[i]);
                PutByte((byte)names.Length);
                for (var i = 0; i < value.Count; i++)
                {
                    PutByte((byte)names[i].Length);
                    PutBytes(names[i]);
                }

                PutByte((byte)value.Count);
            }
        }
    }
}
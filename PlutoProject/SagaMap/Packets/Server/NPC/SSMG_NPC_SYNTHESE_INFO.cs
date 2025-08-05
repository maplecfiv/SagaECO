using System;
using SagaDB.Synthese;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_SYNTHESE_INFO : Packet
    {
        public SSMG_NPC_SYNTHESE_INFO()
        {
            data = new byte[42];
            offset = 2;
            ID = 0x13B6;
        }

        public SyntheseInfo Synthese
        {
            set
            {
                if (value.Materials.Count > 4 || value.Products.Count > 4)
                    throw new ArgumentOutOfRangeException();
                PutByte(4, 2);
                var j = 0;
                foreach (var i in value.Materials)
                {
                    PutUInt(i.ID, (ushort)(3 + j * 4));
                    j++;
                }

                PutByte(4, 19);
                j = 0;
                foreach (var i in value.Materials)
                {
                    PutUShort(i.Count, (ushort)(20 + j * 2));
                    j++;
                }

                PutUInt(value.RequiredTool, 28);
                PutUInt(value.ID, 32);
                PutUInt(value.Gold, 36);
                PutByte(1, 40);
            }
        }
    }
}
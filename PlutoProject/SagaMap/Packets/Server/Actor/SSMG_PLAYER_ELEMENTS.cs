using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_ELEMENTS : Packet
    {
        public SSMG_PLAYER_ELEMENTS()
        {
            data = new byte[32];
            offset = 2;
            ID = 0x0223;

            PutByte(7, 2);
            PutByte(7, 17);
        }

        public Dictionary<Elements, int> AttackElements
        {
            set
            {
                var j = 0;
                foreach (var i in value.Keys) PutShort((short)value[i], (ushort)(3 + j++ * 2));
            }
        }

        public Dictionary<Elements, int> DefenceElements
        {
            set
            {
                var j = 0;
                foreach (var i in value.Keys) PutShort((short)value[i], (ushort)(18 + j++ * 2));
            }
        }
    }
}
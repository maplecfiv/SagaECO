using SagaLib;

namespace SagaMap.Packets.Server.DefWar
{
    public class SSMG_DEFWAR_STATE : Packet
    {
        public SSMG_DEFWAR_STATE()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x1BCA;
        }


        public uint MapID
        {
            set => PutUInt(value, 2);
        }

        /// <summary>
        ///     百分比
        /// </summary>
        public byte Rate
        {
            set => PutUInt(value, 6);
        }
    }
}
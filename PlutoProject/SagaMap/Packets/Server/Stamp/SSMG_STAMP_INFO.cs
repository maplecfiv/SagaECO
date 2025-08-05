using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.Stamp
{
    public class SSMG_STAMP_INFO : Packet
    {
        private int page;

        public SSMG_STAMP_INFO()
        {
            data = new byte[29];
            offset = 2;
            ID = 0x1BBC;
        }

        public int Page
        {
            get => page;
            set
            {
                PutInt(value, 2);
                page = value;
            }
        }

        public SagaDB.Actor.Stamp Stamp
        {
            set
            {
                PutByte(0x0B, 6);
                if (Page == 0)
                    for (var i = 0; i < 10; i++)
                        PutShort((short)value[(StampGenre)i].Value);
                else if (Page == 1)
                    for (var i = 11; i < 21; i++)
                        PutShort((short)value[(StampGenre)i].Value);
            }
        }
    }
}
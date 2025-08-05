using SagaLib;

namespace SagaMap.Packets.Server.DEM
{
    public class SSMG_DEM_CHIP_SHOP_DATA : Packet
    {
        public SSMG_DEM_CHIP_SHOP_DATA()
        {
            data = new byte[15];
            offset = 2;
            ID = 0x063A;
        }

        public uint EXP
        {
            set => PutUInt(value, 2);
        }

        public uint JEXP
        {
            set => PutUInt(value, 6);
        }

        public uint ItemID
        {
            set => PutUInt(value, 10);
        }

        public string Description
        {
            set
            {
                var comment = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[15 + comment.Length];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)comment.Length, 14);
                PutBytes(comment, 15);
            }
        }
    }
}
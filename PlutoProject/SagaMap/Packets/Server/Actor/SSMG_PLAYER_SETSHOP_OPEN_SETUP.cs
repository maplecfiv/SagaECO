using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_SETSHOP_OPEN_SETUP : Packet
    {
        public SSMG_PLAYER_SETSHOP_OPEN_SETUP()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x190B;
        }

        public uint Unknown
        {
            set => PutUInt(0, 2);
        }


        public string Comment
        {
            set
            {
                var comment = Global.Unicode.GetBytes(value + "\0");
                var buf = new byte[7 + comment.Length];
                data.CopyTo(buf, 0);
                data = buf;

                PutByte((byte)comment.Length, 6);
                PutBytes(comment, 7);
            }
        }
    }
}
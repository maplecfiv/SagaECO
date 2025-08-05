using SagaLib;

namespace SagaMap.Packets.Server.Ring
{
    public class SSMG_RING_INFO : Packet
    {
        public enum Reason
        {
            CREATE,
            JOIN,
            NONE,
            UPDATED
        }

        public SSMG_RING_INFO()
        {
            data = new byte[28];
            offset = 2;
            ID = 0x1ACC;
        }

        public void Ring(SagaDB.Ring.Ring ring, Reason reason)
        {
            PutUInt(ring.ID, 2);
            var buf = Global.Unicode.GetBytes(ring.Name + "\0");
            var buff = new byte[28 + buf.Length];
            var size = (byte)buf.Length;
            data.CopyTo(buff, 0);
            data = buff;
            PutByte(size, 6);
            PutBytes(buf, 7);
            PutUInt(0);
            PutByte(2);
            PutInt(0x0D);
            //this.PutUInt(0);
            PutUInt(ring.Fame);
            PutInt(ring.MemberCount);
            PutInt(ring.MaxMemberCount);
        }
    }
}
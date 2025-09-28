using SagaLib;

namespace SagaMap.Packets.Server.Ring
{
    public class SSMG_RING_MEMBER_INFO : Packet
    {
        public SSMG_RING_MEMBER_INFO()
        {
            data = new byte[17];
            offset = 2;
            ID = 0x1ACE;
        }

        public void Member(ActorPC value, SagaDB.Ring.Ring ring)
        {
            int index;
            if (ring != null)
                index = ring.IndexOf(value);
            else
                index = -1;
            PutInt(index, 2);
            PutUInt(value.CharID);
            var buf = Global.Unicode.GetBytes(value.Name + "\0");
            var buff = new byte[17 + buf.Length];
            data.CopyTo(buff, 0);
            data = buff;

            PutByte((byte)buf.Length);
            PutBytes(buf);
            PutByte((byte)value.Race);
            PutByte((byte)value.Gender);
            if (index != -1)
                PutInt(ring.Rights[index].Value);
        }
    }
}
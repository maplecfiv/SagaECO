using SagaDB.Actor;
using SagaDB.Party;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTY_NAME : Packet
    {
        public SSMG_PARTY_NAME()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x19D9;
        }

        public void Party(Party party, Actor pc)
        {
            PutUInt(pc.ActorID, 2);
            byte[] buf;
            if (party != null)
                buf = Global.Unicode.GetBytes(party.Name + "\0");
            else
                buf = new byte[1];
            var buff = new byte[8 + buf.Length];
            var size = (byte)buf.Length;
            data.CopyTo(buff, 0);
            data = buff;
            PutByte(size, 6);
            PutBytes(buf, 7);
        }
    }
}
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.Party
{
    public class SSMG_PARTY_INFO : Packet
    {
        public SSMG_PARTY_INFO()
        {
            data = new byte[13];
            offset = 2;
            ID = 0x19DC;
        }

        public void Party(SagaDB.Party.Party party, ActorPC pc)
        {
            PutUInt(party.ID, 2);
            var buf = Global.Unicode.GetBytes(party.Name + "\0");
            var buff = new byte[13 + buf.Length];
            var size = (byte)buf.Length;
            data.CopyTo(buff, 0);
            data = buff;
            PutByte(size, 6);
            PutBytes(buf, 7);
            if (party.Leader == pc)
                PutByte(1, (ushort)(7 + size));
            else
                PutByte(0, (ushort)(7 + size));
            PutByte(party.IndexOf(pc), (ushort)(8 + size));
            PutInt(party.MemberCount, (ushort)(9 + size));
        }
    }
}
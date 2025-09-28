using SagaLib;

namespace SagaMap.Packets.Server.PProtect
{
    public class SSMG_PPROTECT_CHAT_INFO : Packet
    {
        public SSMG_PPROTECT_CHAT_INFO()
        {
            data = new byte[17];
            offset = 2;
            ID = 0x236D;
        }


        public void SetData(ActorPC pc, byte id
            , byte unknown1, byte unknown2, byte unknown3, byte unknown4, byte unknown5)
        {
            PutUInt(pc.CharID);
            PutByte(id);
            setString(pc.Name, offset);
            if (pc.Pet != null)
                PutUInt(pc.Pet.PetID);
            else
                offset += 4;
            PutByte(unknown1);
            PutByte(unknown2);
            PutByte(unknown3);
            PutByte(unknown4);
            PutByte(unknown5);
        }


        private void setString(string str, int i)
        {
            var buf = Global.Unicode.GetBytes(str + "\0");
            var buff = new byte[data.Length + buf.Length];
            var size = (byte)buf.Length;
            data.CopyTo(buff, 0);
            data = buff;
            PutByte(size, i);
            PutBytes(buf, i + 1);
        }
    }
}
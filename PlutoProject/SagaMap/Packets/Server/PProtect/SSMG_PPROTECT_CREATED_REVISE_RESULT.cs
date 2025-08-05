using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PPROTECT_CREATED_REVISE_RESULT : Packet
    {
        public SSMG_PPROTECT_CREATED_REVISE_RESULT()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x2364;
        }


        public void SetData(string name, string message, uint id, byte maxMember
            , byte unknown1, byte unknown2)
        {
            PutByte(unknown1);
            setString(name, offset);
            setString(message, offset);
            PutByte(unknown2);
            PutUInt(id);
            PutByte(maxMember);
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
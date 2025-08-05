using SagaLib;

namespace SagaMap.Packets.Server.PProtect
{
    public class SSMG_PPROTECT_CREATED_ADD_RESULT : Packet
    {
        public SSMG_PPROTECT_CREATED_ADD_RESULT()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x2368;
        }


        public void SetData(string name, string password, byte errid
            , byte unknown1, byte unknown2)
        {
            PutByte(errid);
            if (string.IsNullOrEmpty(name))
                PutByte(0);
            else
                setString(name, offset);
            if (string.IsNullOrEmpty(password))
                PutByte(0);
            else
                setString(password, offset);
            PutByte(unknown1);
            PutByte(unknown2);
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
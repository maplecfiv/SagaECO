using SagaLib;

namespace SagaMap.Packets.Server.PProtect
{
    public class SSMG_PPROTECT_CREATED_OUT_RESULT : Packet
    {
        public SSMG_PPROTECT_CREATED_OUT_RESULT()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x236C;
        }


        public void SetName(string str)
        {
            var buf = Global.Unicode.GetBytes(str + "\0");
            var buff = new byte[4 + buf.Length];
            var size = (byte)buf.Length;
            data.CopyTo(buff, 0);
            data = buff;
            PutByte(size, 3);
            PutBytes(buf, 4);
        }
    }
}
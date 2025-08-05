using SagaLib;

namespace SagaLogin.Packets.Server.Login
{
    public class SSMG_VERSION_ACK : Packet
    {
        public enum Result
        {
            OK = 0,
            VERSION_MISSMATCH = -1
        }

        public SSMG_VERSION_ACK()
        {
            data = new byte[10];
            offset = 14;
            ID = 0x0002;
        }

        public void SetResult(Result res)
        {
            PutShort((short)res, 2);
        }

        public void SetVersion(string version)
        {
            PutBytes(Conversions.HexStr2Bytes(version), 4);
        }
    }
}
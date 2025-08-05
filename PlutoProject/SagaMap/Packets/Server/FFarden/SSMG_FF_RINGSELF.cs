using SagaLib;

namespace SagaMap.Packets.Server.FFarden
{
    public class SSMG_FF_RINGSELF : Packet
    {
        public SSMG_FF_RINGSELF()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x201B;
        }

        public string name
        {
            set
            {
                var ffname = Global.Unicode.GetBytes(value); //定于飞空城名称
                var buf = new byte[(byte)ffname.Length + 3]; //定义buf，长度为飞空城名称的长度+3
                data.CopyTo(buf, 0); //copy
                data = buf;
                PutByte((byte)ffname.Length, 2); //发送名称字节个数
                PutBytes(ffname, 3); //发送名称字节
            }
        }
    }
}
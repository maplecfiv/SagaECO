using SagaLib;

namespace SagaMap.Scripting
{
    public enum FG_Weather
    {
        None,
        Rain,
        Snow
    }
}

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_CHANGE_WEATHER : Packet
    {
        public SSMG_FG_CHANGE_WEATHER()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x13BC;
        }

        public byte Weather
        {
            set => PutByte(value, 2);
        }
    }
}
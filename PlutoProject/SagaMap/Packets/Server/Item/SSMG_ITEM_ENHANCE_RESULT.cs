using SagaLib;

namespace SagaMap.Packets.Server.Item
{
    public class SSMG_ITEM_ENHANCE_RESULT : Packet
    {
        public SSMG_ITEM_ENHANCE_RESULT()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x13C6;
        }

        /// <summary>
        ///     01 SUCCESS
        ///     00 FAILD
        ///     FF NO GOLD
        ///     FE ITEM IS NOT EXISTS
        ///     FD NO MERIATIAL
        ///     FC ????
        ///     FB EXCEPTION CAN NOT GET EXP
        /// </summary>

        public int Result
        {
            set
            {
                PutByte((byte)value, 2);
                PutUShort(0x64, 3);
            }
        }

        public ushort OrignalRefine
        {
            set => PutUShort(value, 5);
        }

        public ushort ExpectedRefine
        {
            set => PutUShort(value, 7);
        }
    }
}
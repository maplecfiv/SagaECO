using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_EXP : Packet
    {
        public SSMG_PLAYER_EXP()
        {
            data = new byte[18 + 8]; //8bytes unknowns
            offset = 2;
            ID = 0x0235;
            PutByte(1, 10);
        }

        /// <summary>
        ///     345 means 34.5%
        /// </summary>
        public uint EXPPercentage
        {
            set => PutUInt(value, 2);
        }

        public uint JEXPPercentage
        {
            set => PutUInt(value, 6);
        }

        public int WRP
        {
            set => PutInt(value, 11);
        }

        public uint ECoin
        {
            set
            {
                //this.PutUInt(value, 14);
            }
        }

        public long Exp
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga10)
                {
                    //this.PutLong(value, 18);
                }
            }
        }

        public long JExp
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga10)
                {
                    // this.PutLong(value, 26);
                }
            }
        }
    }
}
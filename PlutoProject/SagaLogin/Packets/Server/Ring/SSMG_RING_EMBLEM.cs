using System;
using SagaLib;

namespace SagaLogin.Packets.Server
{
    public class SSMG_RING_EMBLEM : Packet
    {
        public SSMG_RING_EMBLEM()
        {
            data = new byte[16];
            ID = 0x010A;
        }

        /// <summary>
        ///     0 is not up to date, 1 is latest
        /// </summary>
        public int Result
        {
            set => PutInt(value, 2);
        }

        public uint RingID
        {
            set => PutUInt(value, 6);
        }

        /// <summary>
        ///     0 is exists, 1 is doesn't exists
        /// </summary>
        public byte Result2
        {
            set => PutByte(value, 10);
        }

        public byte[] Data
        {
            set
            {
                var buf = new byte[20 + value.Length];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte(0xFD, 11);
                PutInt(value.Length, 12);
                PutBytes(value, 16);
            }
        }

        public DateTime UpdateTime
        {
            set
            {
                var date = (uint)(value - new DateTime(1970, 1, 1)).TotalSeconds;
                if (GetByte(11) == 0xFD)
                {
                    var len = GetInt(12);
                    PutUInt(date, (ushort)(16 + len));
                }
                else
                {
                    PutUInt(date, 12);
                }
            }
        }
    }
}
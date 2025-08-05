using System.Collections.Generic;
using SagaDB.Tamaire;
using SagaLib;

namespace SagaLogin.Packets.Server.Tamaire
{
    public class SSMG_TAMAIRE_LIST : Packet
    {
        public SSMG_TAMAIRE_LIST()
        {
            data = new byte[9];
            offset = 2;
            ID = 0x0227;
        }

        public void PutData(List<TamaireLending> data, byte baselv)
        {
            var buf = new byte[this.data.Length + data.Count * 4];
            this.data.CopyTo(buf, 0);
            this.data = buf;
            offset = 3;
            PutByte((byte)data.Count, offset);
            for (var i = 0; i < data.Count; i++)
                PutUInt(data[i].Lender, offset);

            buf = new byte[this.data.Length + data.Count];
            this.data.CopyTo(buf, 0);
            this.data = buf;
            PutByte((byte)data.Count, offset);
            for (var i = 0; i < data.Count; i++)
                PutByte(data[i].Baselv, offset);

            buf = new byte[this.data.Length + data.Count];
            this.data.CopyTo(buf, 0);
            this.data = buf;
            PutByte((byte)data.Count, offset);
            for (var i = 0; i < data.Count; i++)
                PutByte(data[i].JobType, offset);

            PutByte((byte)data.Count, offset);
            byte[] name;
            int size;
            for (var i = 0; i < data.Count; i++)
            {
                name = Global.Unicode.GetBytes(LoginServer.charDB.GetChar(data[i].Lender).Name);
                size = name.Length;
                buf = new byte[this.data.Length + size + 1];
                this.data.CopyTo(buf, 0);
                this.data = buf;
                PutByte((byte)size, offset);
                PutBytes(name, offset);
            }

            PutByte((byte)data.Count, offset);
            byte[] comment;
            for (var i = 0; i < data.Count; i++)
            {
                comment = Global.Unicode.GetBytes(data[i].Comment);
                size = comment.Length;
                buf = new byte[this.data.Length + size + 1];
                this.data.CopyTo(buf, 0);
                this.data = buf;
                PutByte((byte)size, offset);
                PutBytes(comment, offset);
            }


            PutByte((byte)data.Count, offset);
            buf = new byte[this.data.Length + data.Count * 2];
            this.data.CopyTo(buf, 0);
            this.data = buf;
            for (var i = 0; i < data.Count; i++)
            {
                var leveldiff = data[i].Baselv - baselv;
                if (leveldiff < 0)
                    leveldiff = -leveldiff;
                if (leveldiff == 0)
                    PutShort(0, offset);
                if (leveldiff >= 1 && leveldiff <= 5)
                    PutShort(10, offset);
                if (leveldiff >= 6 && leveldiff <= 10)
                    PutShort(30, offset);
                if (leveldiff >= 11 && leveldiff <= 15)
                    PutShort(50, offset);
                if (leveldiff >= 16 && leveldiff <= 20)
                    PutShort(100, offset);
                if (leveldiff >= 20 && leveldiff <= 105)
                    PutShort((short)((leveldiff - 20) / 5 * 50 + 100), offset);
                if (leveldiff >= 106 && leveldiff <= 109)
                    PutShort(980, offset);
            }
        }
    }
}
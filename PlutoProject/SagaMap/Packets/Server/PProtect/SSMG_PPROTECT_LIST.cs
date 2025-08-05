using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.PProtect
{
    public class SSMG_PPROTECT_LIST : Packet
    {
        public SSMG_PPROTECT_LIST()
        {
            data = new byte[16];
            offset = 2;
            ID = 0x235C;
        }

        /// <summary>
        ///     总页数
        /// </summary>
        public ushort PageMax
        {
            set => PutUShort(value, 3);
        }

        /// <summary>
        ///     当前页
        /// </summary>
        public ushort Page
        {
            set => PutUShort(value, 5);
        }

        public List<SagaDB.PProtect.PProtect> List
        {
            set
            {
                var count = value.Count;
                if (count == 0)
                    return;

                var buff = new byte[data.Length + count * 15];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)count, 7);
                offset = 8;
                for (var i = 0; i < count; i++) offset += setString(value[i].Name, offset);
                PutByte((byte)count, offset);
                //this.offset += 1;
                for (var i = 0; i < count; i++) PutUInt(value[i].ID, offset);
                //this.offset += 4;
                PutByte((byte)count, offset);
                //this.offset += 1;
                for (var i = 0; i < count; i++) offset += setString(value[i].Leader.Name, offset);
                PutByte((byte)count, offset);
                //this.offset += 1;
                for (var i = 0; i < count; i++) PutByte((byte)value[i].MemberCount, offset);
                //this.offset += 1;
                PutByte((byte)count, offset);
                //this.offset += 1;
                for (var i = 0; i < count; i++) PutByte(value[i].MaxMember, offset);
                //this.offset += 1;
                PutByte((byte)count, offset);
                //this.offset += 1;
                for (var i = 0; i < count; i++)
                    if (value[i].IsPassword)
                        PutByte(0x01, offset);
                    else
                        PutByte(0x00, offset);
                //this.offset += 1;
                PutByte((byte)count, offset);
                //this.offset += 1;
                for (var i = 0; i < count; i++) PutUInt(value[i].TaskID, offset);
                //this.offset += 4;
                PutByte((byte)count, offset);
                //this.offset += 1;
                for (var i = 0; i < count; i++) offset += setString(value[i].Message, offset);
                PutByte((byte)count, offset);
                //this.offset += 1;
                for (var i = 0; i < count; i++) PutByte(value[i].IsRun, offset);
                //this.offset++;
            }
        }

        private ushort setString(string str, int i)
        {
            var buf = Global.Unicode.GetBytes(str);
            var buff = new byte[data.Length + buf.Length];
            var size = (byte)buf.Length;
            data.CopyTo(buff, 0);
            data = buff;
            PutByte(size, i);
            PutBytes(buf, i + 1);

            return (ushort)(size + 1);
        }
    }
}
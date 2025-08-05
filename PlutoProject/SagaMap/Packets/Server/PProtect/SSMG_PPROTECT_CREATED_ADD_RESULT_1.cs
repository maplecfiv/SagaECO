using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.PProtect
{
    public class SSMG_PPROTECT_CREATED_ADD_RESULT_1 : Packet
    {
        public SSMG_PPROTECT_CREATED_ADD_RESULT_1()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x2366;
        }

        public List<ActorPC> List
        {
            set
            {
                var count = value.Count;
                if (count == 0)
                    return;

                var buff = new byte[data.Length + count * 9];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)count, 4);
                //this.offset = 5;
                for (var i = 0; i < count; i++) setString(value[i].Name);
                PutByte((byte)count);
                for (var i = 0; i < count; i++)
                    if (value[i].Pet != null)
                        PutUInt(value[i].Pet.PetID);
                    else
                        PutUInt(0);
                PutByte((byte)count);
                for (var i = 0; i < count; i++) PutByte(0x0); //base lv
                PutByte((byte)count);
                for (var i = 0; i < count; i++) PutByte(0x0); //转生状态
                PutByte((byte)count);
                for (var i = 0; i < count; i++) PutByte(0x0); //rake？
                PutByte((byte)count);
                for (var i = 0; i < count; i++) PutByte(0x0); //好感度？
            }
        }

        private void setString(string str)
        {
            var buf = Global.Unicode.GetBytes(str);
            var buff = new byte[data.Length + buf.Length];
            var size = (byte)buf.Length;
            data.CopyTo(buff, 0);
            data = buff;
            PutByte(size);
            PutBytes(buf);
        }
    }
}
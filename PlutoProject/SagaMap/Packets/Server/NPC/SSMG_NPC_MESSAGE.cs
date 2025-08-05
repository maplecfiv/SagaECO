using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_MESSAGE : Packet
    {
        public SSMG_NPC_MESSAGE()
        {
            if (Configuration.Instance.Version >= Version.Saga17)
                data = new byte[18];
            else if (Configuration.Instance.Version >= Version.Saga14_2)
                data = new byte[13];
            else
                data = new byte[11];
            offset = 2;
            ID = 0x03F9;
        }

        public void SetMessage(uint npcID, byte num, string message, ushort motion, string title)
        {
            ushort oldoffset;
            byte[] buf;
            byte[] buff;
            byte size;

            PutUInt(npcID, 2);

            var temp = new List<string>();
            var i = message.IndexOf("$R");
            while (i > 0)
            {
                var t = message.Substring(0, i + 2);
                message = message.Remove(0, i + 2);
                temp.Add(t);
                i = message.IndexOf("$R");
            }

            temp.Add(message);

            PutByte((byte)temp.Count, 8);
            oldoffset = 9;
            for (var j = 0; j < temp.Count; j++)
            {
                buf = Global.Unicode.GetBytes(temp[j]);
                buff = new byte[buf.Length + data.Length + 1];
                data.CopyTo(buff, 0);
                data = buff;
                size = (byte)buf.Length;
                PutByte(size, oldoffset);
                oldoffset++;
                PutBytes(buf, oldoffset);
                //oldoffset++;
                oldoffset += size;
            }

            //oldoffset++;
            PutUShort(motion, oldoffset);
            oldoffset++;


            buf = Global.Unicode.GetBytes(title);
            buff = new byte[buf.Length + data.Length + 1];
            data.CopyTo(buff, 0);
            data = buff;
            size = (byte)(buf.Length + 1);
            oldoffset++;
            PutByte(size, oldoffset);
            oldoffset++;
            PutBytes(buf, oldoffset);
            oldoffset += size;
        }

        public void SetMessageOld(uint npcID, byte num, string message, ushort motion, string title)
        {
            //this.PutUInt(0, 2);
            PutUInt(npcID, 2);
            ushort oldoffset;
            if (Configuration.Instance.Version >= Version.Saga14_2)
            {
                oldoffset = 7;
                PutByte(0, 6);
            }
            else
            {
                oldoffset = 6;
            }

            PutByte(num, oldoffset);
            var buf = Global.Unicode.GetBytes(message);
            var buff = new byte[buf.Length + data.Length + 1];
            data.CopyTo(buff, 0);
            data = buff;
            var size = (byte)(buf.Length + 1);
            oldoffset++;
            PutByte(size, oldoffset);
            oldoffset++;
            PutBytes(buf, oldoffset);

            var offset = (ushort)(8 + size);
            if (Configuration.Instance.Version >= Version.Saga14_2)
            {
                PutByte(0, offset);
                offset++;
            }

            PutUShort(motion, offset);

            buf = Global.Unicode.GetBytes(title);
            buff = new byte[buf.Length + data.Length + 1];
            data.CopyTo(buff, 0);
            data = buff;
            size = (byte)(buf.Length + 1);
            PutByte(size, (ushort)(offset + 2));
            PutBytes(buf, (ushort)(offset + 3));
        }
    }
}
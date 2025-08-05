using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public enum InputType
    {
        Bank = 2,
        ItemCode,
        PetRename
    }

    public class SSMG_NPC_INPUTBOX : Packet
    {
        public SSMG_NPC_INPUTBOX()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x5F4;
        }

        public string Title
        {
            set
            {
                var buf = Global.Unicode.GetBytes(value + "\0");
                var buff = new byte[7 + buf.Length];
                data.CopyTo(buff, 0);
                data = buff;

                PutByte((byte)buf.Length, 2);
                PutBytes(buf, 3);
            }
        }

        public InputType Type
        {
            set
            {
                var offset = GetByte(2);
                PutInt((int)value, (ushort)(3 + offset));
            }
        }
    }
}
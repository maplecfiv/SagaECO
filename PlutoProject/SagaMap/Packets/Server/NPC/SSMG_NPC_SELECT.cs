using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_SELECT : Packet
    {
        public SSMG_NPC_SELECT()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x05F6;
        }

        public void SetSelect(string title, string confirm, string[] options, bool canCancel)
        {
            if (title != "")
                if (title.Substring(title.Length - 1) != "\0")
                    title += "\0";
            if (confirm != "")
            {
                if (confirm.Substring(confirm.Length - 1) != "\0")
                    confirm += "\0";
            }
            else
            {
                confirm = "\0";
            }

            for (var i = 0; i < options.Length; i++)
                if (options[i].Substring(options[i].Length - 1) != "\0")
                    options[i] += "\0";

            var titleB = Global.Unicode.GetBytes(title);
            var confirmB = Global.Unicode.GetBytes(confirm);
            var optionsB = new byte[options.Length][];
            var count = 0;
            for (var i = 0; i < options.Length; i++)
            {
                optionsB[i] = Global.Unicode.GetBytes(options[i]);
                count += optionsB[i].Length + 1;
            }

            count += titleB.Length + 1;
            count += confirmB.Length + 1;
            count += 8;
            count += options.Length + 1;
            data = new byte[count];
            ID = 0x05F6;
            offset = 2;

            PutByte((byte)titleB.Length);
            PutBytes(titleB);
            PutByte((byte)options.Length);
            for (var i = 0; i <= options.Length; i++) PutByte((byte)i);
            foreach (var i in optionsB)
            {
                PutByte((byte)i.Length);
                PutBytes(i);
            }

            PutByte((byte)confirmB.Length);
            if (confirmB.Length != 0)
                PutBytes(confirmB);
            if (canCancel)
                PutByte(1);
        }
    }
}
using SagaDB.Actor;
using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client.Login
{
    public class CSMG_CHAR_CREATE : Packet
    {
        public CSMG_CHAR_CREATE()
        {
            offset = 2;
        }

        public byte Slot => GetByte(2);

        public string Name
        {
            get
            {
                byte size;
                byte[] buf;
                size = GetByte(3);
                buf = GetBytes(size, 4);
                var res = Global.Unicode.GetString(buf);
                res = res.Replace("\0", "");
                return res;
            }
        }

        public PC_RACE Race
        {
            get
            {
                int offset = GetDataOffset();
                return (PC_RACE)GetByte((ushort)offset);
            }
        }

        public PC_GENDER Gender
        {
            get
            {
                var offset = GetDataOffset() + 1;
                return (PC_GENDER)GetByte((ushort)offset);
            }
        }

        public byte HairStyle
        {
            get
            {
                int offset;
                if (Configuration.Configuration.Instance.Version >= Version.Saga11)
                    offset = GetDataOffset() + 3;
                else
                    offset = GetDataOffset() + 2;
                return GetByte((ushort)offset);
            }
        }

        public byte HairColor
        {
            get
            {
                int offset;
                if (Configuration.Configuration.Instance.Version >= Version.Saga11)
                    offset = GetDataOffset() + 4;
                else
                    offset = GetDataOffset() + 3;
                return GetByte((ushort)offset);
            }
        }

        public ushort Face
        {
            get
            {
                int offset;
                if (Configuration.Configuration.Instance.Version >= Version.Saga11)
                    offset = GetDataOffset() + 5;
                else
                    offset = GetDataOffset() + 4;
                return GetUShort((ushort)offset);
            }
        }

        private byte GetDataOffset()
        {
            return (byte)(4 + GetByte(3));
        }

        public override Packet New()
        {
            return new CSMG_CHAR_CREATE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnCharCreate(this);
        }
    }
}
using SagaLib;

namespace SagaMap.Packets.Server
{
    /// <summary>
    ///     [00][0E][02][3A]
    ///     [2D] //Lv
    ///     [2D] //JobLv(1次職)
    ///     [01] //JobLv(エキスパート)
    ///     [01] //JobLv(テクニカル)
    ///     [00][2E] //ボーナスポイント
    ///     [00][08] //スキルポイント(1次職)
    ///     [00][00] //スキルポイント(エキスパート)
    ///     [00][00] //スキルポイント(テクニカル)
    /// </summary>
    public class SSMG_PLAYER_LEVEL : Packet
    {
        public SSMG_PLAYER_LEVEL()
        {
            if (Configuration.Instance.Version >= Version.Saga17)
                data = new byte[19];
            else if (Configuration.Instance.Version >= Version.Saga11)
                data = new byte[18];
            else
                data = new byte[15];
            offset = 2;
            ID = 0x023A;
        }

        public byte Level
        {
            set => PutByte(value, 2);
        }

        public byte JobLevel
        {
            set => PutByte(value, 3);
        }

        public byte JobLevel2X
        {
            set => PutByte(value, 4);
        }

        public byte JobLevel2T
        {
            set => PutByte(value, 5);
        }

        public byte JobLevel3
        {
            set => PutByte(value, 6);
        }

        public byte IsDualJob
        {
            set => PutByte(value, 7);
            get => GetByte(7);
        }

        public byte DualjobLevel
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga11)
                    PutByte(value, 8);
            }
        }

        public ushort UseableStatPoint
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga17)
                    PutUShort(value, 9);
                else if (Configuration.Instance.Version >= Version.Saga11)
                    PutUShort(value, 8);
                else
                    PutUShort(value, 7);
            }
        }

        public ushort SkillPoint
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga17)
                    PutUShort(value, 11);
                else if (Configuration.Instance.Version >= Version.Saga11)
                    PutUShort(value, 10);
                else
                    PutUShort(value, 9);
            }
        }

        public ushort Skill2XPoint
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga17)
                    PutUShort(value, 13);
                else if (Configuration.Instance.Version >= Version.Saga11)
                    PutUShort(value, 12);
                else
                    PutUShort(value, 11);
            }
        }

        public ushort Skill2TPoint
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga17)
                    PutUShort(value, 15);
                else if (Configuration.Instance.Version >= Version.Saga11)
                    PutUShort(value, 14);
                else
                    PutUShort(value, 13);
            }
        }

        public ushort Skill3Point
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga17)
                    PutUShort(value, 17);
                else if (Configuration.Instance.Version >= Version.Saga11)
                    PutUShort(value, 16);
                else
                    PutUShort(value, 13);
            }
        }
    }
}
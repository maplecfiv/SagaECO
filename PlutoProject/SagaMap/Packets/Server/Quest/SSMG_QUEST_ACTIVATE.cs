using SagaDB.Quests;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_QUEST_ACTIVATE : Packet
    {
        public SSMG_QUEST_ACTIVATE()
        {
            data = new byte[83];
            offset = 2;
            ID = 0x1969;
        }

        /// <summary>
        ///     显示任务详细资料
        /// </summary>
        /// <param name="type">任务类型</param>
        /// <param name="name">任务名字</param>
        /// <param name="mapid1">未知地图ID</param>
        /// <param name="mapid2">传送任务起始地图</param>
        /// <param name="mapid3">传送任务目标地图</param>
        /// <param name="info1">未知NPC名</param>
        /// <param name="info2">传送任务起始NPC</param>
        /// <param name="info3">传送任务目标NPC</param>
        /// <param name="unk1">击退任务怪物地图1</param>
        /// <param name="unk2">击退任务怪物地图2</param>
        /// <param name="unk3">击退任务怪物地图3</param>
        /// <param name="item1">物品或怪物ID</param>
        /// <param name="item2">物品或怪物ID</param>
        /// <param name="item3">物品或怪物ID</param>
        /// <param name="amount1">物品或怪物数量</param>
        /// <param name="amount2">物品或怪物数量</param>
        /// <param name="amount3">物品或怪物数量</param>
        /// <param name="time">剩余时间</param>
        /// <param name="unk4"></param>
        public void SetDetail(QuestType type, string name,
            uint mapid1, uint mapid2, uint mapid3,
            string info1, string info2, string info3,
            QuestStatus status,
            uint unk1, uint unk2, uint unk3,
            uint item1, uint item2, uint item3,
            uint amount1, uint amount2, uint amount3,
            int time, uint unk4)
        {
            var nameb = Global.Unicode.GetBytes(name + "\0");
            var info1b = Global.Unicode.GetBytes(info1);
            var info2b = Global.Unicode.GetBytes(info2);
            var info3b = Global.Unicode.GetBytes(info3);
            var buff = new byte[69 + nameb.Length + info1b.Length + info2b.Length + info3b.Length];
            data.CopyTo(buff, 0);
            data = buff;

            PutByte((byte)type, 2);
            PutByte((byte)nameb.Length, 3);
            PutBytes(nameb, 4);
            PutByte(3, (ushort)(4 + nameb.Length));
            PutUInt(mapid1, (ushort)(5 + nameb.Length));
            PutUInt(mapid2, (ushort)(9 + nameb.Length));
            PutUInt(mapid3, (ushort)(13 + nameb.Length));
            PutByte(3, (ushort)(17 + nameb.Length));
            PutByte((byte)info1b.Length, (ushort)(18 + nameb.Length));
            PutBytes(info1b, (ushort)(19 + nameb.Length));
            PutByte((byte)info2b.Length, (ushort)(19 + nameb.Length + info1b.Length));
            PutBytes(info2b, (ushort)(20 + nameb.Length + info1b.Length));
            PutByte((byte)info3b.Length, (ushort)(20 + nameb.Length + info1b.Length + info2b.Length));
            PutBytes(info3b, (ushort)(21 + nameb.Length + info1b.Length + info2b.Length));
            PutByte((byte)status, (ushort)(21 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutByte(3, (ushort)(22 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(unk1, (ushort)(23 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(unk2, (ushort)(27 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(unk3, (ushort)(31 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutByte(3, (ushort)(35 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(item1, (ushort)(36 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(item2, (ushort)(40 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(item3, (ushort)(44 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutByte(3, (ushort)(48 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(amount1, (ushort)(49 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(amount2, (ushort)(53 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(amount3, (ushort)(57 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutInt(time, (ushort)(61 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
            PutUInt(unk4, (ushort)(65 + nameb.Length + info1b.Length + info2b.Length + info3b.Length));
        }

        public void SetDetail(uint questID,
            uint npcid1, uint npcid2, uint mpcid3,
            QuestStatus status,
            uint mapid1, uint mapid2, uint mapid3,
            uint item1, uint item2, uint item3,
            uint amount1, uint amount2, uint amount3,
            int time, uint unk1, uint exp, uint unk2, uint jobexp, uint gold)
        {
            PutUInt(questID);
            PutByte(3);
            PutUInt(npcid1);
            PutUInt(npcid2);
            PutUInt(mpcid3);
            PutByte((byte)status);
            PutByte(3);
            PutUInt(mapid1);
            PutUInt(mapid2);
            PutUInt(mapid3);
            PutByte(3);
            PutUInt(item1);
            PutUInt(item2);
            PutUInt(item3);
            PutByte(3);
            PutUInt(amount1);
            PutUInt(amount2);
            PutUInt(amount3);
            PutInt(time);
            PutUInt(unk1);
            PutUInt(exp);
            PutUInt(unk2);
            PutUInt(jobexp);
            PutUInt(gold);
        }
    }
}
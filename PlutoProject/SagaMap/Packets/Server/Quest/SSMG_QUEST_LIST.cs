using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Quest
{
    public class SSMG_QUEST_LIST : Packet
    {
        public SSMG_QUEST_LIST()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x1964;
        }

        public List<QuestInfo> Quests
        {
            set
            {
                //ADWORD QuestID
                PutByte((byte)value.Count);
                foreach (var i in value) PutUInt(i.ID);

                //ABYTE QuestType
                PutByte((byte)value.Count);
                foreach (var item in value) PutByte((byte)item.QuestType);


                //ATSTR QuestName
                PutByte((byte)value.Count);
                foreach (var item in value) PutTSTR(item.Name);

                //ADWORD QuestTime
                PutByte((byte)value.Count);
                foreach (var item in value) PutInt(item.TimeLimit);


                //ABYTE QuestLevel
                PutByte((byte)value.Count);
                foreach (var item in value) PutByte(item.MinLevel);
            }
        }
    }
}
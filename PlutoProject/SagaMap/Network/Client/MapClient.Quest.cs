using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Npc;
using SagaDB.Quests;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public uint questID;

        public void OnDailyDungeonOpen()
        {
            //if(Character.MapID != 10054000)
            //{
            //    SendSystemMessage("当前区域无法打开每日地牢。");
            //    return;
            //}
            if (Character.AStr["每日地牢记录"] == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                SendSystemMessage("你今天已经入场过了，请明天再来吧。");
                return;
            }

            if (Character.Party != null)
            {
                SendSystemMessage("请先退出队伍。");
                return;
            }

            var p = new SSMG_DAILYDUNGEON_INFO();
            p.RemainSecond = (uint)(86400 - DateTime.Now.Hour * 3600 - DateTime.Now.Minute * 60 - DateTime.Now.Second);
            var ids = new List<byte>();
            ids.Add(0);
            p.IDs = ids;
            netIO.SendPacket(p);
        }

        public void OnDailyDungeonJoin(CSMG_DAILYDUNGEON_JOIN p)
        {
            if (p.QID == 0)
                EventActivate(980000101);
        }

        public void OnQuestDetailRequest(CSMG_QUEST_DETAIL_REQUEST p)
        {
            if (QuestFactory.Instance.Items.ContainsKey(p.QuestID))
            {
                var quest = QuestFactory.Instance.Items[p.QuestID];
                uint map1 = 0, map2 = 0, map3 = 0;
                string name1 = " ", name2 = " ", name3 = " ";
                NPC npc1 = null, npc2 = null, npc3 = null;
                if (NPCFactory.Instance.Items.ContainsKey(quest.NPCSource))
                    npc2 = NPCFactory.Instance.Items[quest.NPCSource];
                if (NPCFactory.Instance.Items.ContainsKey(quest.NPCDestination))
                    npc3 = NPCFactory.Instance.Items[quest.NPCDestination];
                if (npc1 != null)
                {
                    map1 = npc1.MapID;
                    name1 = npc1.Name;
                }

                if (npc2 != null)
                {
                    map2 = npc2.MapID;
                    name2 = npc2.Name;
                }

                if (npc3 != null)
                {
                    map3 = npc3.MapID;
                    name3 = npc3.Name;
                }

                var p2 = new SSMG_QUEST_DETAIL();
                p2.SetDetail(quest.QuestType, quest.Name, map1, map2, map3, name1, name2, name3, quest.MapID1,
                    quest.MapID2, quest.MapID3, quest.ObjectID1, quest.ObjectID2, quest.ObjectID3, (uint)quest.Count1,
                    (uint)quest.Count2, (uint)quest.Count3, quest.TimeLimit, 0);
                netIO.SendPacket(p2);
            }
        }

        public void OnQuestSelect(CSMG_QUEST_SELECT p)
        {
            questID = p.QuestID;
        }

        public void SendQuestInfo()
        {
            var quest = Character.Quest;
            uint map1 = 0, map2 = 0, map3 = 0;
            string name1 = " ", name2 = " ", name3 = " ";
            if (quest == null)
                return;
            var p2 = new SSMG_QUEST_ACTIVATE();
            NPC npc1 = null, npc2 = null, npc3 = null;
            npc1 = quest.NPC;
            if (npc1 == null && NPCFactory.Instance.Items.ContainsKey(currentEventID))
                npc1 = NPCFactory.Instance.Items[currentEventID];
            if (NPCFactory.Instance.Items.ContainsKey(quest.Detail.NPCSource))
                npc2 = NPCFactory.Instance.Items[quest.Detail.NPCSource];
            if (NPCFactory.Instance.Items.ContainsKey(quest.Detail.NPCDestination))
                npc3 = NPCFactory.Instance.Items[quest.Detail.NPCDestination];
            if (npc1 != null)
            {
                map1 = npc1.MapID;
                name1 = npc1.Name;
            }

            if (npc2 != null)
            {
                map2 = npc2.MapID;
                name2 = npc2.Name;
            }

            if (npc3 != null)
            {
                map3 = npc3.MapID;
                name3 = npc3.Name;
            }

            //p2.SetDetail(quest.QuestType, quest.Name, map1, map2, map3, name1, name2, name3, quest.Status, quest.Detail.MapID1, quest.Detail.MapID2, quest.Detail.MapID3, quest.Detail.ObjectID1, quest.Detail.ObjectID2, quest.Detail.ObjectID3, (uint)quest.Detail.Count1, (uint)quest.Detail.Count2, (uint)quest.Detail.Count3, quest.Detail.TimeLimit, 0);
            p2.SetDetail(quest.ID, currentEventID, quest.Detail.NPCSource, quest.Detail.NPCDestination, quest.Status,
                quest.Detail.MapID1, quest.Detail.MapID2, quest.Detail.MapID3, quest.Detail.ObjectID1,
                quest.Detail.ObjectID2, quest.Detail.ObjectID3, (uint)quest.Detail.Count1, (uint)quest.Detail.Count2,
                (uint)quest.Detail.Count3, quest.Detail.TimeLimit, 0, quest.Detail.EXP, 0, quest.Detail.JEXP,
                quest.Detail.Gold);
            var ss = p2.DumpData();
            netIO.SendPacket(p2);
        }

        public void SendQuestPoints()
        {
            var p = new SSMG_QUEST_POINT();
            if (Character.QuestNextResetTime > DateTime.Now)
            {
                p.ResetTime = (uint)(Character.QuestNextResetTime - DateTime.Now).TotalHours;
            }
            else
            {
                var hours = (int)(DateTime.Now - Character.QuestNextResetTime).TotalHours;
                if (hours > 24000)
                {
                    Character.QuestNextResetTime =
                        DateTime.Now + new TimeSpan(0, Configuration.Instance.QuestUpdateTime, 0, 0);
                }
                else
                {
                    if (Character.Account.questNextTime <= Character.QuestNextResetTime)
                    {
                        Character.QuestRemaining += (ushort)((hours / Configuration.Instance.QuestUpdateTime + 1) *
                                                             Configuration.Instance.QuestUpdateAmount);
                        if (Character.QuestRemaining > Configuration.Instance.QuestPointsMax)
                            Character.QuestRemaining = (ushort)Configuration.Instance.QuestPointsMax;
                        Character.QuestNextResetTime = Character.QuestNextResetTime + new TimeSpan(0,
                            (hours / Configuration.Instance.QuestUpdateTime + 1) *
                            Configuration.Instance.QuestUpdateTime, 0, 0);
                        Character.Account.questNextTime = Character.QuestNextResetTime;
                    }
                    else
                    {
                        Character.QuestNextResetTime = Character.Account.questNextTime;
                    }
                }

                p.ResetTime = (uint)(Character.QuestNextResetTime - DateTime.Now).TotalHours;
            }

            p.QuestPoint = Character.QuestRemaining;
            netIO.SendPacket(p);
        }

        public void SendQuestCount()
        {
            if (Character.Quest != null)
            {
                var p = new SSMG_QUEST_COUNT_UPDATE();
                p.Count1 = Character.Quest.CurrentCount1;
                p.Count2 = Character.Quest.CurrentCount2;
                p.Count3 = Character.Quest.CurrentCount3;
                netIO.SendPacket(p);
                if (Character.Quest.Status != QuestStatus.FAILED)
                    if (Character.Quest.CurrentCount1 == Character.Quest.Detail.Count1 &&
                        Character.Quest.CurrentCount2 == Character.Quest.Detail.Count2 &&
                        Character.Quest.CurrentCount3 == Character.Quest.Detail.Count3 &&
                        Character.Quest.QuestType != QuestType.TRANSPORT)
                    {
                        Character.Quest.Status = QuestStatus.COMPLETED;
                        SendQuestStatus();
                    }
            }
        }

        public void SendQuestTime()
        {
            if (Character.Quest != null)
            {
                var p = new SSMG_QUEST_RESTTIME_UPDATE();
                if (Character.Quest.EndTime > DateTime.Now)
                {
                    p.RestTime = (int)(Character.Quest.EndTime - DateTime.Now).TotalMinutes;
                }
                else
                {
                    if (Character.Quest.Status != QuestStatus.COMPLETED)
                    {
                        Character.Quest.Status = QuestStatus.FAILED;
                        SendQuestStatus();
                    }
                }

                netIO.SendPacket(p);
            }
        }

        public void SendQuestStatus()
        {
            if (Character.Quest != null)
            {
                var p = new SSMG_QUEST_STATUS_UPDATE();
                p.Status = Character.Quest.Status;
                netIO.SendPacket(p);
            }
        }

        public void SendQuestList(List<QuestInfo> quests)
        {
            var p = new SSMG_QUEST_LIST();
            p.Quests = quests;
            netIO.SendPacket(p);
        }

        public void SendQuestWindow()
        {
            var p = new SSMG_QUEST_WINDOW();
            netIO.SendPacket(p);
        }

        public void SendQuestDelete()
        {
            var p = new SSMG_QUEST_DELETE();
            netIO.SendPacket(p);
        }

        public void QuestMobKilled(ActorMob mob, bool party)
        {
            if (Character.Quest != null)
                if (Character.Quest.QuestType == QuestType.HUNT)
                {
                    if (party && !Character.Quest.Detail.Party)
                        return;
                    if (mob.MapID == Character.Quest.Detail.MapID1 ||
                        mob.MapID == Character.Quest.Detail.MapID2 ||
                        mob.MapID == Character.Quest.Detail.MapID3 ||
                        (Character.Quest.Detail.MapID1 == 0 && Character.Quest.Detail.MapID2 == 0 &&
                         Character.Quest.Detail.MapID3 == 0) ||
                        (Character.Quest.Detail.MapID1 == 60000000 && map.IsDungeon) ||
                        (Character.Quest.Detail.MapID1 == map.ID / 1000 * 1000 && map.IsMapInstance) ||
                        (Character.Quest.Detail.MapID2 == map.ID / 1000 * 1000 && map.IsMapInstance) ||
                        (Character.Quest.Detail.MapID3 == map.ID / 1000 * 1000 && map.IsMapInstance))
                    {
                        if (Character.Quest.Detail.ObjectID1 == mob.MobID)
                            Character.Quest.CurrentCount1++;
                        if (Character.Quest.Detail.ObjectID1 == 0 && Character.Quest.Detail.Count1 != 0)
                            Character.Quest.CurrentCount1++;

                        if (Character.Quest.Detail.ObjectID2 == mob.MobID)
                            Character.Quest.CurrentCount2++;
                        if (Character.Quest.Detail.ObjectID2 == 0 && Character.Quest.Detail.Count2 != 0)
                            Character.Quest.CurrentCount2++;

                        if (Character.Quest.Detail.ObjectID3 == mob.MobID)
                            Character.Quest.CurrentCount3++;
                        if (Character.Quest.Detail.ObjectID3 == 0 && Character.Quest.Detail.Count3 != 0)
                            Character.Quest.CurrentCount3++;

                        if (Character.Quest.CurrentCount1 > Character.Quest.Detail.Count1)
                            Character.Quest.CurrentCount1 = Character.Quest.Detail.Count1;
                        if (Character.Quest.CurrentCount2 > Character.Quest.Detail.Count2)
                            Character.Quest.CurrentCount2 = Character.Quest.Detail.Count2;
                        if (Character.Quest.CurrentCount3 > Character.Quest.Detail.Count3)
                            Character.Quest.CurrentCount3 = Character.Quest.Detail.Count3;
                        SendQuestCount();
                    }
                }
        }

        public void EventMobKilled(ActorMob mob)
        {
            var MobId = mob.MobID;
            foreach (var i in Character.KillList)
                if (!i.Value.isFinish)
                {
                    if (i.Key == MobId)
                    {
                        i.Value.Count++;
                        SendSystemMessage("击杀任务：已击杀 " + mob.BaseData.name + " (" + i.Value.Count + "/" +
                                          i.Value.TotalCount + ")");
                        if (i.Value.Count == i.Value.TotalCount)
                            SendSystemMessage("击杀任务：击杀 " + mob.BaseData.name + " 已完成！");
                    }

                    if (i.Value.Count >= i.Value.TotalCount)
                        i.Value.isFinish = true;
                }
        }
    }
}
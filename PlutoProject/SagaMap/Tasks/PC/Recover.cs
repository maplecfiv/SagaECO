using System;
using System.Collections.Generic;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.Network.Client;

namespace SagaMap.Tasks.PC
{
    public class Recover : MultiRunTask
    {
        private readonly MapClient client;

        public Recover(MapClient client)
        {
            dueTime = 3000;
            period = 3000;
            this.client = client;
        }

        public override void CallBack()
        {
            //ClientManager.EnterCriticalArea();
            try
            {
                if (client != null)
                {
                    var pc = client.Character;
                    if (pc == null) return;
                    if ((DateTime.Now - pc.TTime["上次自然回复时间"]).TotalSeconds < 3)
                    {
                        Deactivate();
                        client.Character.Tasks.Remove("Recover");
                        return;
                    }

                    if (client.Character.Tasks.ContainsKey("Recover"))
                        if (client.Character.Tasks["Recover"] != this)
                        {
                            Deactivate();
                            return;
                        }

                    if (!client.Character.Tasks.ContainsKey("Recover")) client.Character.Tasks.Add("Recover", this);
                    pc.TTime["上次自然回复时间"] = DateTime.Now;
                    var s = DateTime.Now;
                    BuffChecker(pc);
                    if ((Logger.defaultlogger.LogLevel | Logger.LogContent.Custom) == Logger.defaultlogger.LogLevel)
                        Logger.ShowError("玩家" + client.Character.Name + "BUFF检测耗时：" +
                                         (DateTime.Now - s).TotalMilliseconds);
                    if (pc.MapID == 91000999 && pc.FurnitureID != 0 && pc.FurnitureID != 255)
                        client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.FURNITURE_SIT, null, pc, true);
                    if (pc.Mode == PlayerMode.KNIGHT_EAST) //除夕活动
                    {
                        Deactivate();
                        client.Character.Tasks.Remove("Recover");
                    }

                    if (pc.HP > 0 && pc.Buff.Dead)
                    {
                        pc.Buff.Dead = false;
                        pc.Buff.TurningPurple = false;
                    }

                    if (pc.HP > 0 && !pc.Buff.TurningPurple && !pc.Buff.Dead)
                    {
                    }
                    else
                    {
                        Deactivate();
                        client.Character.Tasks.Remove("Recover");
                    }

                    if ((Logger.defaultlogger.LogLevel | Logger.LogContent.Custom) == Logger.defaultlogger.LogLevel)
                        Logger.ShowError("玩家" + client.Character.Name + "自然恢复总耗时：" +
                                         (DateTime.Now - s).TotalMilliseconds);
                }

                else
                {
                    Deactivate();
                    client.Character.Tasks.Remove("Recover");
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
                Deactivate();
                client.Character.Tasks.Remove("Recover");
            }
            //ClientManager.LeaveCriticalArea();
        }

        private List<int> MapList(ActorPC pc)
        {
            var maps = new List<int>();
            maps.Add(pc.TInt["S20090000"]);
            maps.Add(pc.TInt["S20091000"]);
            maps.Add(pc.TInt["S20092000"]);
            maps.Add(pc.TInt["S60903000"]);
            maps.Add(pc.TInt["S10054100"]);
            maps.Add(pc.TInt["S20004000"]);
            maps.Add(pc.TInt["S20003000"]);
            maps.Add(pc.TInt["S20002000"]);
            maps.Add(pc.TInt["S20001000"]);
            maps.Add(pc.TInt["S20000000"]);
            maps.Add(pc.TInt["S30131002"]);
            maps.Add(pc.TInt["S21180001"]);
            maps.Add(pc.TInt["每日地牢地图ID"]);
            return maps;
        }

        private void BuffChecker(ActorPC pc)
        {
            //if (pc.Buff.单枪匹马)
            //{
            //    List<int> maps = MapList(pc);
            //    if (!maps.Contains((int)pc.MapID) || pc.Party != null)
            //    {
            //        if (pc.Buff.单枪匹马)
            //        {
            //            pc.Buff.单枪匹马 = false;
            //            client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, pc, true);
            //        }
            //        if (pc.TInt["副本复活标记"] == 4)
            //            pc.TInt["副本复活标记"] = 0;
            //    }

            //    if (pc.MapID != pc.TInt["S21180001"] || pc.Party != null)
            //    {
            //        pc.TInt["临时HP"] = 0;
            //        pc.TInt["临时攻击上升"] = 0;
            //    }
            //}
            //if (pc.Party != null)
            //{
            //    if (pc.Buff.黑暗压制)
            //    {
            //        if (pc.MapID != pc.Party.TInt["S20090000"] && pc.MapID != pc.Party.TInt["S20091000"] && pc.MapID != pc.Party.TInt["S20092000"] && pc.MapID != pc.Party.TInt["S60903000"] && pc.MapID != pc.Party.TInt["S21180001"])
            //        {
            //            pc.Buff.黑暗压制 = false;
            //            client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, pc, true);
            //        }
            //        if (pc.MapID != pc.Party.TInt["S21180001"])
            //        {
            //            pc.TInt["临时HP"] = 0;
            //            pc.TInt["临时攻击上升"] = 0;
            //        }
            //    }
            //}
            //else
            //{
            //    if (pc.Buff.黑暗压制)
            //    {
            //        pc.Buff.黑暗压制 = false;
            //        client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, pc, true);
            //    }
            //}
        }
    }
}
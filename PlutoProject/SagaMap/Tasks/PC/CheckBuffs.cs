﻿using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Tasks.PC
{
    public partial class Recover : MultiRunTask
    {
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
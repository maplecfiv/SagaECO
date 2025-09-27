using System;
using System.Linq;
using SagaDB.Actor;
using SagaDB.ODWar;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Packets.Server.NPC;
using SagaMap.Packets.Server.ODWar;

namespace SagaMap.ODWar
{
    public enum SymbolReviveResult
    {
        Success,
        NotDown,
        StillTrash,
        Faild
    }

    public class ODWarManager : Singleton<ODWarManager>
    {
        public void StartODWar(uint mapID)
        {
            if (IsDefence(mapID))
                spawnSymbol(mapID);
            else
                spawnSymbolTrash(mapID);
        }

        public bool IsDefence(uint mapID)
        {
            return ScriptManager.Instance.VariableHolder.AInt["ODWar" + mapID + "Captured"] == 0;
        }

        private void spawnSymbol(uint mapID)
        {
            var war = ODWarFactory.Instance.Items[mapID];
            var map = MapManager.Instance.GetMap(mapID);
            foreach (var i in war.Symbols.Values)
            {
                var x = Global.PosX8to16(i.x, map.Width);
                var y = Global.PosY8to16(i.y, map.Height);

                i.actorID = map.SpawnMob(i.mobID, x, y, 2000, null).ActorID;
                i.broken = false;
            }
        }

        private void spawnSymbolTrash(uint mapID)
        {
            var war = ODWarFactory.Instance.Items[mapID];
            var map = MapManager.Instance.GetMap(mapID);
            foreach (var i in war.Symbols.Values)
            {
                var x = Global.PosX8to16(i.x, map.Width);
                var y = Global.PosY8to16(i.y, map.Height);
                i.actorID = map.SpawnMob(war.SymbolTrash, x, y, 2000, null).ActorID;
                i.broken = true;
            }
        }

        public void SymbolDown(uint mapID, ActorMob mob)
        {
            var war = ODWarFactory.Instance.Items[mapID];
            var map = MapManager.Instance.GetMap(mapID);
            var alldown = true;
            foreach (var i in war.Symbols.Keys)
            {
                var sym = war.Symbols[i];
                if (sym.actorID == mob.ActorID)
                {
                    if (mob.MobID == war.SymbolTrash)
                    {
                        sym.actorID = 0;
                    }
                    else
                    {
                        if (mob.MobID == sym.mobID)
                        {
                            sym.actorID = map.SpawnMob(war.SymbolTrash, mob.X, mob.Y, 10, null).ActorID;
                            sym.broken = true;
                            map.Announce(string.Format(LocalManager.Instance.Strings.ODWAR_SYMBOL_DOWN, i));
                        }
                    }
                }

                if (!sym.broken)
                    alldown = false;
            }

            if (IsDefence(mapID) && alldown) EndODWar(mapID, false);
        }

        public void UpdateScore(uint mapID, uint actorID, int delta)
        {
            if (ODWarFactory.Instance.Items.ContainsKey(mapID))
            {
                var war = ODWarFactory.Instance.Items[mapID];
                if (!war.Score.ContainsKey(actorID))
                    war.Score.Add(actorID, 0);
                war.Score[actorID] += delta;
                if (war.Score[actorID] < 0)
                    war.Score[actorID] = 0;
            }
        }

        public SymbolReviveResult ReviveSymbol(uint mapID, int number)
        {
            var war = ODWarFactory.Instance.Items[mapID];
            var map = MapManager.Instance.GetMap(mapID);
            if (war.Symbols.ContainsKey(number))
            {
                if (war.Symbols[number].broken)
                {
                    if (war.Symbols[number].actorID == 0)
                    {
                        var x = Global.PosX8to16(war.Symbols[number].x, map.Width);
                        var y = Global.PosY8to16(war.Symbols[number].y, map.Height);
                        Actor actor = map.SpawnMob(war.Symbols[number].mobID, x, y, 10, null);
                        actor.HP = actor.MaxHP / 2;
                        map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, false);
                        war.Symbols[number].actorID = actor.ActorID;
                        war.Symbols[number].broken = false;

                        map.Announce(string.Format(LocalManager.Instance.Strings.ODWAR_SYMBOL_ACTIVATE, number));
                        if (!IsDefence(mapID))
                        {
                            var win = true;
                            foreach (var i in war.Symbols.Values)
                                if (i.broken)
                                    win = false;
                            if (win) EndODWar(mapID, true);
                        }

                        return SymbolReviveResult.Success;
                    }

                    return SymbolReviveResult.StillTrash;
                }

                return SymbolReviveResult.NotDown;
            }

            return SymbolReviveResult.Faild;
        }

        public void SpawnBoss(uint mapID)
        {
            var war = ODWarFactory.Instance.Items[mapID];
            var map = MapManager.Instance.GetMap(mapID);
            foreach (var i in war.Symbols.Values)
            {
                var x = Global.PosX8to16(i.x, map.Width);
                var y = Global.PosY8to16(i.y, map.Height);
                var mobID = war.Boss[Global.Random.Next(0, war.Boss.Count - 1)];
                var pos = map.GetRandomPosAroundPos(x, y, 1500);
                map.SpawnMob(mobID, pos[0], pos[1], 2000, null);
            }
        }

        public void SpawnMob(uint mapID, bool strong)
        {
            var war = ODWarFactory.Instance.Items[mapID];
            var map = MapManager.Instance.GetMap(mapID);
            foreach (var i in war.Symbols.Values)
            {
                var x = Global.PosX8to16(i.x, map.Width);
                var y = Global.PosY8to16(i.y, map.Height);
                if (strong)
                {
                    for (var j = 0; j < war.WaveStrong.DEMChamp; j++)
                    {
                        var mobID = war.DEMChamp[Global.Random.Next(0, war.DEMChamp.Count - 1)];
                        var pos = map.GetRandomPosAroundPos(x, y, 1500);
                        map.SpawnMob(mobID, pos[0], pos[1], 2000, null);
                    }

                    for (var j = 0; j < war.WaveStrong.DEMNormal; j++)
                    {
                        var mobID = war.DEMNormal[Global.Random.Next(0, war.DEMNormal.Count - 1)];
                        var pos = map.GetRandomPosAroundPos(x, y, 1500);
                        map.SpawnMob(mobID, pos[0], pos[1], 2000, null);
                    }
                }
                else
                {
                    for (var j = 0; j < war.WaveWeak.DEMChamp; j++)
                    {
                        var mobID = war.DEMChamp[Global.Random.Next(0, war.DEMChamp.Count - 1)];
                        var pos = map.GetRandomPosAroundPos(x, y, 1500);
                        map.SpawnMob(mobID, pos[0], pos[1], 2000, null);
                    }

                    for (var j = 0; j < war.WaveWeak.DEMNormal; j++)
                    {
                        var mobID = war.DEMNormal[Global.Random.Next(0, war.DEMNormal.Count - 1)];
                        var pos = map.GetRandomPosAroundPos(x, y, 1500);
                        map.SpawnMob(mobID, pos[0], pos[1], 2000, null);
                    }
                }
            }
        }

        /// <summary>
        ///     是否可以申请城市攻防战
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns></returns>
        public bool CanApply(uint mapID)
        {
            if (!IsDefence(mapID))
                return false;
            var war = ODWarFactory.Instance.Items[mapID];
            if (war.StartTime.ContainsKey((int)DateTime.Today.DayOfWeek))
            {
                if (DateTime.Now.Hour < war.StartTime[(int)DateTime.Today.DayOfWeek])
                    return true;
                if (DateTime.Now.Minute < 15)
                    return true;
                return false;
            }

            return false;
        }

        /// <summary>
        ///     结束都市攻防战
        /// </summary>
        /// <param name="mapID"></param>
        /// <returns>是否胜利</returns>
        public void EndODWar(uint mapID, bool win)
        {
            var war = ODWarFactory.Instance.Items[mapID];
            var map = MapManager.Instance.GetMap(mapID);

            if (IsDefence(mapID))
            {
                if (!win)
                {
                    ScriptManager.Instance.VariableHolder.AInt["ODWar" + mapID + "Captured"] = 1;
                    MapClientManager.Instance.Announce(LocalManager.Instance.Strings.ODWAR_LOSE);
                    var actors = map.Actors.Values.ToList();
                    foreach (var i in actors)
                        if (i.type == ActorType.MOB)
                        {
                            var eh = (MobEventHandler)i.e;
                            if (!eh.AI.Mode.Symbol && !eh.AI.Mode.SymbolTrash)
                                eh.OnDie();
                        }
                }
                else
                {
                    MapClientManager.Instance.Announce(LocalManager.Instance.Strings.ODWAR_WIN);
                    MapClientManager.Instance.Announce(LocalManager.Instance.Strings.ODWAR_WIN2);
                    MapClientManager.Instance.Announce(LocalManager.Instance.Strings.ODWAR_WIN3);
                    MapClientManager.Instance.Announce(LocalManager.Instance.Strings.ODWAR_WIN4);

                    var actors = map.Actors.Values.ToList();
                    foreach (var i in actors)
                        if (i.type == ActorType.MOB)
                        {
                            var eh = (MobEventHandler)i.e;
                            if (!eh.AI.Mode.Symbol && !eh.AI.Mode.SymbolTrash)
                                eh.OnDie();
                        }
                }

                SendResult(mapID, win);
            }
            else
            {
                if (win)
                {
                    ScriptManager.Instance.VariableHolder.AInt["ODWar" + mapID + "Captured"] = 0;
                    MapClientManager.Instance.Announce(LocalManager.Instance.Strings.ODWAR_CAPTURE);
                    var actors = map.Actors.Values.ToList();
                    foreach (var i in actors)
                        if (i.type == ActorType.MOB)
                        {
                            var eh = (MobEventHandler)i.e;
                            if (!eh.AI.Mode.Symbol && !eh.AI.Mode.SymbolTrash)
                                eh.OnDie();
                        }
                }
            }

            var actors2 = map.Actors.Values.ToList();
            foreach (var i in actors2)
                if (i.type == ActorType.PC)
                    if (((ActorPC)i).Online)
                    {
                        var p1 = new SSMG_NPC_SET_EVENT_AREA();
                        p1.StartX = 6;
                        p1.EndX = 6;
                        p1.StartY = 127;
                        p1.EndY = 127;
                        p1.EventID = 0xF1000000;
                        p1.EffectID = 9005;
                        MapClient.FromActorPC((ActorPC)i).NetIo.SendPacket(p1);
                        p1 = new SSMG_NPC_SET_EVENT_AREA();
                        p1.StartX = 245;
                        p1.EndX = 245;
                        p1.StartY = 127;
                        p1.EndY = 127;
                        p1.EventID = 0xF1000001;
                        p1.EffectID = 9005;
                        MapClient.FromActorPC((ActorPC)i).NetIo.SendPacket(p1);
                    }

            war.Score.Clear();
            war.Started = false;
        }

        private void SendResult(uint mapID, bool win)
        {
            var war = ODWarFactory.Instance.Items[mapID];
            var map = MapManager.Instance.GetMap(mapID);

            foreach (var i in war.Score.Keys)
            {
                var actor = map.GetActor(i);
                if (actor == null)
                    continue;
                if (actor.type != ActorType.PC)
                    continue;

                var score = (uint)war.Score[i];
                var pc = (ActorPC)actor;
                if (!pc.Online)
                    continue;
                if (score > 3000)
                    score = 3000;

                if (pc.WRPRanking <= 10)
                    score = (uint)(score * 1.5f);
                if (!win)
                    score = (uint)(score * 0.75f);
                if (win)
                    if (score < 200)
                        score = 200;
                var exp = (uint)(score * 0.6f);

                pc.CP += score;
                //ExperienceManager.Instance.ApplyExp(pc, exp, exp, 1f);

                var p = new SSMG_ODWAR_RESULT();
                p.Win = win;
                p.EXP = exp;
                p.JEXP = exp;
                p.CP = score;
                MapClient.FromActorPC(pc).NetIo.SendPacket(p);
            }
        }
    }
}
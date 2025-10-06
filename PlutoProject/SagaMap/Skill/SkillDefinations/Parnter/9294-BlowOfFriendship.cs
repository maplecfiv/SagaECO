using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    public class BlowOfFriendship : ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 200, null);
            var recoveraffected = new List<Actor>();
            foreach (var act in affected)
                if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    if (act.Buff.Dead != true)
                    {
                        recoveraffected.Add(act);
                    }
                    else if (act.Buff.TurningPurple != true && act.type == ActorType.PC)
                    {
                        var m = (ActorPC)act;
                        m.Buff.TurningPurple = true;
                        MapClient.FromActorPC(m).Map
                            .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, m, true);
                        m.TInt["Revive"] = level;
                        MapClient.FromActorPC(m).EventActivate(0xF1000000);
                        m.TStr["Revive"] = sActor.Name;
                        MapClient.FromActorPC(m).SendSystemMessage(string.Format("玩家 {0} 正在请求你复活", sActor.Name));
                    }

                    if (!act.Status.Additions.ContainsKey("BlowOfFriendship") && !act.Buff.Undead)
                    {
                        var skill = new DefaultBuff(args.skill, act, "BlowOfFriendship", 120000);
                        skill.OnAdditionStart += StartEventHandler;
                        skill.OnAdditionEnd += EndEventHandler;
                        SkillHandler.ApplyAddition(act, skill);
                    }
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.TurningRed = true; //找不到对应buff图标到底是哪个了，先用这个代替，反正也没有其他地方在用……
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.TurningRed = true;
            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}
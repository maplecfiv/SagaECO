using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Stryder_风行者____rag
{
    /// <summary>
    ///     アートフルトラップ
    /// </summary>
    public class ArtFullTrap : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 300000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(dActor, 200, false);
            affected.Add(dActor);
            foreach (var act in affected)
                if (act.type == ActorType.PC || act.type == ActorType.PARTNER)
                {
                    SkillHandler.RemoveAddition(act, "ArtFullTrap1");
                    SkillHandler.RemoveAddition(act, "ArtFullTrap2");
                    SkillHandler.RemoveAddition(act, "ArtFullTrap3");
                    SkillHandler.RemoveAddition(act, "ArtFullTrap4");
                    SkillHandler.RemoveAddition(act, "ArtFullTrap5");
                    switch (level)
                    {
                        case 1:
                            var skill = new DefaultBuff(args.skill, act, "ArtFullTrap1", lifetime);
                            skill.OnAdditionStart += StartEventHandler;
                            skill.OnAdditionEnd += EndEventHandler;
                            SkillHandler.ApplyAddition(act, skill);
                            break;
                        case 2:
                            var skill2 = new DefaultBuff(args.skill, act, "ArtFullTrap2", lifetime);
                            skill2.OnAdditionStart += StartEventHandler;
                            skill2.OnAdditionEnd += EndEventHandler;
                            SkillHandler.ApplyAddition(act, skill2);
                            break;
                        case 3:
                            var skill3 = new DefaultBuff(args.skill, act, "ArtFullTrap3", lifetime);
                            skill3.OnAdditionStart += StartEventHandler;
                            skill3.OnAdditionEnd += EndEventHandler;
                            SkillHandler.ApplyAddition(act, skill3);
                            break;
                        case 4:
                            var skill4 = new DefaultBuff(args.skill, act, "ArtFullTrap4", lifetime);
                            skill4.OnAdditionStart += StartEventHandler;
                            skill4.OnAdditionEnd += EndEventHandler;
                            SkillHandler.ApplyAddition(act, skill4);
                            break;
                        case 5:
                            var skill5 = new DefaultBuff(args.skill, act, "ArtFullTrap5", lifetime);
                            skill5.OnAdditionStart += StartEventHandler;
                            skill5.OnAdditionEnd += EndEventHandler;
                            SkillHandler.ApplyAddition(act, skill5);
                            break;
                    }
                }
        }


        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.アートフルトラップ = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.アートフルトラップ = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}
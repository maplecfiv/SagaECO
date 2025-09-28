using SagaLib;
using SagaMap.Skill.SkillDefinations.Global.Active;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_
{
    public class HolyGroove : Groove, ISkill
    {
        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            ProcSub(sActor, dActor, args, level, Elements.Holy);
        }
    }
}
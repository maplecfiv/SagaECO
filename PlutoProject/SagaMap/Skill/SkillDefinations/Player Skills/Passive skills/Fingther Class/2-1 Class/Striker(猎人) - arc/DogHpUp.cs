using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Fingther_Class._2_1_Class.Striker_猎人____arc
{
    public class DogHpUp : ISkill
    {
        private readonly float[] HP_AddRate = { 0f, 6f, 6, 8f, 8f, 10f };

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet != null)
            {
                if (SkillHandler.Instance.CheckMobType(pet, "ANIMAL")) active = true;
                var skill = new DefaultPassiveSkill(args.skill, pet, "DogHpUp", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(pet, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);

            //MaxHP
            var MaxHP = (int)actor.MaxHP;
            if (skill.Variable.ContainsKey("DogHpUp_MaxHP"))
                skill.Variable.Remove("DogHpUp_MaxHP");
            skill.Variable.Add("DogHpUp_MaxHP", MaxHP);
            actor.MaxHP = (uint)(actor.MaxHP * (1 + HP_AddRate[skill.skill.Level]));
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var map = MapManager.Instance.GetMap(actor.MapID);
            actor.MaxHP -= (uint)skill.Variable["DogHpUp_MaxHP"];
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
        }

        //#endregion
    }
}
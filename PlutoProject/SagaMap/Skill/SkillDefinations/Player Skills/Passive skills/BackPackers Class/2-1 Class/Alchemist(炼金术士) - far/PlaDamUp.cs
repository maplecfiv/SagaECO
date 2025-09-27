using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     植物傷害增加（植物系ダメージ上昇）
    /// </summary>
    public class PlaDamUp : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //ushort[] Values = { 0, 3, 6, 9, 12, 15 };//%

            //ushort value = Values[level];

            var skill = new DefaultPassiveSkill(args.skill, sActor, "魔鬼体质", true);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var rate = skill.skill.Level * 0.05f;
            var addmaxatk1 = (int)(actor.Status.max_atk1 * rate);
            var addmaxatk2 = (int)(actor.Status.max_atk2 * rate);
            var addmaxatk3 = (int)(actor.Status.max_atk3 * rate);
            var addminatk1 = (int)(actor.Status.min_atk1 * rate);
            var addminatk2 = (int)(actor.Status.min_atk2 * rate);
            var addminatk3 = (int)(actor.Status.min_atk3 * rate);
            var addmaxmatk = (int)(actor.Status.max_matk * rate);
            var addminmatk = (int)(actor.Status.min_matk * rate);
            //int reducehp = (int)(actor.MaxHP * rate);

            if (skill.Variable.ContainsKey("魔鬼体质addmaxatk1"))
                skill.Variable.Remove("魔鬼体质addmaxatk1");
            skill.Variable.Add("魔鬼体质addmaxatk1", addmaxatk1);
            actor.Status.max_atk1_skill += (short)addmaxatk1;

            if (skill.Variable.ContainsKey("魔鬼体质addmaxatk2"))
                skill.Variable.Remove("魔鬼体质addmaxatk2");
            skill.Variable.Add("魔鬼体质addmaxatk2", addmaxatk2);
            actor.Status.max_atk2_skill += (short)addmaxatk2;

            if (skill.Variable.ContainsKey("魔鬼体质addmaxatk3"))
                skill.Variable.Remove("魔鬼体质addmaxatk3");
            skill.Variable.Add("魔鬼体质addmaxatk3", addmaxatk3);
            actor.Status.max_atk3_skill += (short)addmaxatk3;

            if (skill.Variable.ContainsKey("魔鬼体质addminatk1"))
                skill.Variable.Remove("魔鬼体质addminatk1");
            skill.Variable.Add("魔鬼体质addminatk1", addminatk1);
            actor.Status.min_atk1_skill += (short)addmaxatk1;

            if (skill.Variable.ContainsKey("魔鬼体质addminatk2"))
                skill.Variable.Remove("魔鬼体质addminatk2");
            skill.Variable.Add("魔鬼体质addminatk2", addminatk2);
            actor.Status.min_atk2_skill += (short)addmaxatk2;

            if (skill.Variable.ContainsKey("魔鬼体质addminatk3"))
                skill.Variable.Remove("魔鬼体质addminatk3");
            skill.Variable.Add("魔鬼体质addminatk3", addminatk3);
            actor.Status.min_atk3_skill += (short)addmaxatk3;

            if (skill.Variable.ContainsKey("魔鬼体质addmaxmatk"))
                skill.Variable.Remove("魔鬼体质addmaxmatk");
            skill.Variable.Add("魔鬼体质addmaxmatk", addmaxmatk);
            actor.Status.max_matk_skill += (short)addmaxmatk;

            if (skill.Variable.ContainsKey("魔鬼体质addminmatk"))
                skill.Variable.Remove("魔鬼体质addminmatk");
            skill.Variable.Add("魔鬼体质addminmatk", addminmatk);
            actor.Status.min_matk_skill += (short)addminmatk;

            /*
            if (skill.Variable.ContainsKey("魔鬼体质reducehp"))
                skill.Variable.Remove("魔鬼体质reducehp");
            skill.Variable.Add("魔鬼体质reducehp", reducehp);
            actor.Status.hp_skill -= (short)reducehp;
            */
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            actor.Status.max_atk1_skill -= (short)skill.Variable["魔鬼体质addmaxatk1"];
            actor.Status.max_atk2_skill -= (short)skill.Variable["魔鬼体质addmaxatk2"];
            actor.Status.max_atk3_skill -= (short)skill.Variable["魔鬼体质addmaxatk3"];
            actor.Status.min_atk1_skill -= (short)skill.Variable["魔鬼体质addminatk1"];
            actor.Status.min_atk2_skill -= (short)skill.Variable["魔鬼体质addminatk2"];
            actor.Status.min_atk3_skill -= (short)skill.Variable["魔鬼体质addminatk3"];
            actor.Status.min_matk_skill -= (short)skill.Variable["魔鬼体质addminmatk"];
            actor.Status.max_matk_skill -= (short)skill.Variable["魔鬼体质addmaxmatk"];
            //actor.Status.hp_skill += (short)skill.Variable["魔鬼体质reducehp"];
        }

        #endregion
    }
}
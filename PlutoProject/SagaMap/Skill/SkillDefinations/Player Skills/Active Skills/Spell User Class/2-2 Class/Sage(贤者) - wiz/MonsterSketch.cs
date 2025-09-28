namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz
{
    /// <summary>
    ///     魔物素描（モンスタースケッチ）
    /// </summary>
    public class MonsterSketch : ISkill
    {
        private readonly uint SKETCHBOOK = 10020757; //畫板
        private readonly uint SKETCHBOOK_Finish = 10020758; //畫板（畫作完成）

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                if (SkillHandler.Instance.CountItem(sActor, SKETCHBOOK) > 0) return 0;

                return -2;
            }

            return -12;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            SkillHandler.Instance.TakeItem((ActorPC)sActor, SKETCHBOOK, 1);
            var r = SkillHandler.Instance.GiveItem((ActorPC)sActor, SKETCHBOOK_Finish, 1, true);
            var mob = (ActorMob)dActor;
            r[0].PictID = mob.MobID;
            SkillHandler.Instance.AttractMob(sActor, dActor, 1);
        }

        #endregion
    }
}
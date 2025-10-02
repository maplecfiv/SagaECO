using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Necromancer_死灵使____lock
{
    /// <summary>
    ///     解除憑依（魂抜き）
    /// </summary>
    public class TrDrop2 : ISkill
    {
        private readonly bool MobUse;

        public TrDrop2()
        {
            MobUse = false;
        }

        public TrDrop2(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (dActor.type == ActorType.PC)
            {
                if (MobUse) level = 5;
                var rate = 20 + 10 * level;
                if (SagaLib.Global.Random.Next(0, 99) < rate)
                    SkillHandler.Instance.PossessionCancel((ActorPC)dActor, PossessionPosition.NONE);
            }
        }

        //#endregion
    }
}
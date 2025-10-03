using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Shaman_精灵使_
{
    public class ThunderBall : ISkill
    {
        private readonly bool MobUse;

        public ThunderBall()
        {
            MobUse = false;
        }

        public ThunderBall(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (MobUse) level = 5;
            float[] factors = { 0, 1.15f, 1.35f, 1.55f, 1.75f, 2.0f };
            var factor = factors[level];
            //if (SagaLib.Global.Random.Next(0, 100) < 60)
            //{
            //    factor += 0.8f + 0.2f * level;

            //    EffectArg arg = new EffectArg();
            //    arg.effectID = 5073;
            //    arg.actorID = dActor.ActorID;
            //    if (sActor.type == ActorType.PC)
            //        SagaMap.Network.Client.MapClient.FromActorPC((ActorPC)sActor).map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, (ActorPC)sActor, true);

            //}
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Wind, factor);
        }

        //#endregion
    }
}
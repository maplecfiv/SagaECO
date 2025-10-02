﻿namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    ///     火光蟲（フレアスティング）
    /// </summary>
    public class FlareSting : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factors = { 0f, 4.0f, 4.25f, 4.5f, 4.75f, 5.0f };
            var factor = factors[level];
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var rate = 25 + 5 * level;
            if (args.hp.Count > 0)
                if (SagaLib.Global.Random.Next(0, 99) < rate && args.hp[0] > 0 &&
                    !SkillHandler.Instance.isBossMob(dActor))
                    //AutoCastInfo info = new AutoCastInfo();
                    //info.delay = 1500;
                    //info.level = level;
                    //info.skillID = 2404;
                    //args.autoCast.Add(info);
                    args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(2404, level, 3000));
        }

        //#endregion
    }
}
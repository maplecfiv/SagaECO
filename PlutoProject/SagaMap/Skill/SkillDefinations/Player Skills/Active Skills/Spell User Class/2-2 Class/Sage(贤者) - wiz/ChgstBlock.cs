using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz
{
    /// <summary>
    ///     解放異常狀態（エンチャントブロック）
    /// </summary>
    public class ChgstBlock : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 30 + 10 * level;
            if (SagaLib.Global.Random.Next(0, 99) < rate)
            {
                var adds = new List<string>();
                foreach (var ad in dActor.Status.Additions)
                    if (!(ad.Value is DefaultPassiveSkill))
                        adds.Add(ad.Value.Name);
                foreach (var adn in adds) SkillHandler.RemoveAddition(dActor, adn);
            }
            //尚須加上不能被賦予狀態
        }

        //#endregion
    }
}
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     ミューテイション(突变)
    /// </summary>
    public class Mutation : MobISkill
    {
        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //Manager.MapClientManager.EnterCriticalArea();
            foreach (var item in sActor.AttackElements.Keys)
                sActor.Status.elements_skill[item] = 0;

            var rnd = SagaLib.Global.Random.Next(1, 100);
            if (rnd > 0 && rnd <= 20)
                sActor.Status.elements_skill[Elements.Fire] = 100;
            else if (rnd > 20 && rnd <= 40)
                sActor.Status.elements_skill[Elements.Water] = 100;
            else if (rnd > 40 && rnd <= 60)
                sActor.Status.elements_skill[Elements.Wind] = 100;
            else if (rnd > 60 && rnd <= 80)
                sActor.Status.elements_skill[Elements.Earth] = 100;
            else
                sActor.Status.elements_skill[Elements.Dark] = 100;
            //Manager.MapClientManager.LeaveCriticalArea();
            MapManager.Instance.GetMap(sActor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, sActor, false);
        }
    }
}
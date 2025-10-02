using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     怪物用各屬性祝福
    /// </summary>
    public class MobElementBless : ISkill
    {
        public Elements element;

        public MobElementBless(Elements e)
        {
            element = e;
        }

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (dActor.Status.Additions.ContainsKey(element + "Rise")) return -1;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (dActor.Status.Additions.ContainsKey("HolyShield"))
                SkillHandler.RemoveAddition(dActor, "HolyShield");
            if (dActor.Status.Additions.ContainsKey("DarkShield"))
                SkillHandler.RemoveAddition(dActor, "DarkShield");
            if (dActor.Status.Additions.ContainsKey("FireShield"))
                SkillHandler.RemoveAddition(dActor, "FireShield");
            if (dActor.Status.Additions.ContainsKey("WaterShield"))
                SkillHandler.RemoveAddition(dActor, "WaterShield");
            if (dActor.Status.Additions.ContainsKey("WindShield"))
                SkillHandler.RemoveAddition(dActor, "WindShield");
            if (dActor.Status.Additions.ContainsKey("EarthShield"))
                SkillHandler.RemoveAddition(dActor, "EarthShield");
            dActor.Buff.BodyDarkElementUp = false;
            dActor.Buff.BodyEarthElementUp = false;
            dActor.Buff.BodyFireElementUp = false;
            dActor.Buff.BodyWaterElementUp = false;
            dActor.Buff.BodyWindElementUp = false;
            dActor.Buff.BodyHolyElementUp = false;
            if (dActor.Status.elements_skill[Elements.Earth] != 0)
                dActor.Status.elements_skill[Elements.Earth] = 0;
            if (dActor.Status.elements_skill[Elements.Water] != 0)
                dActor.Status.elements_skill[Elements.Water] = 0;
            if (dActor.Status.elements_skill[Elements.Fire] != 0)
                dActor.Status.elements_skill[Elements.Fire] = 0;
            if (dActor.Status.elements_skill[Elements.Wind] != 0)
                dActor.Status.elements_skill[Elements.Wind] = 0;
            if (dActor.Status.elements_skill[Elements.Dark] != 0)
                dActor.Status.elements_skill[Elements.Dark] = 0;
            if (dActor.Status.elements_skill[Elements.Holy] != 0)
                dActor.Status.elements_skill[Elements.Holy] = 0;
            var lifetime = 50000;
            var skill = new DefaultBuff(args.skill, dActor, element + "Rise", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var ElementAdd = 50;

            //原屬性值
            if (skill.Variable.ContainsKey("ElementRise_" + element))
                skill.Variable.Remove("ElementRise_" + element);
            skill.Variable.Add("ElementRise_" + element, ElementAdd);

            actor.Status.elements_skill[element] += ElementAdd;
            //actor.Status.attackElements_skill[element] += ElementAdd;
            if (actor.Status.elements_skill[element] > 100)
                actor.Status.elements_skill[element] = 100;
            if (actor.Status.attackElements_skill[element] > 100)
                actor.Status.attackElements_skill[element] = 100;
            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + element + "ElementUp");
            propertyInfo.SetValue(actor.Buff, true, null);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var value = (short)skill.Variable["ElementRise_" + element];
            if (skill.Variable.ContainsKey("ElementRise_" + element))
                skill.Variable.Remove("ElementRise_" + element);
            //原屬性值
            actor.Status.elements_skill[element] -= value;
            //actor.Status.attackElements_skill[element] -= value;
            if (actor.Status.elements_skill[element] < 0)
                actor.Status.elements_skill[element] = 0;
            if (actor.Status.attackElements_skill[element] < 0)
                actor.Status.attackElements_skill[element] = 0;
            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + element + "ElementUp");
            propertyInfo.SetValue(actor.Buff, false, null);
        }

        //#endregion
    }
}
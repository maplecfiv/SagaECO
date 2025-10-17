using System;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Global.Active {
    public class ElementShield : ISkill {
        public Elements element;
        public bool monsteruse;

        public ElementShield(Elements e, bool ismonster = false) {
            element = e;
            monsteruse = ismonster;
        }

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args) {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            if (monsteruse) {
                level = 5;
                dActor = sActor;
            }

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
            var life = new[] { 0, 15000, 35000, 60000, 100000, 150000 }[level];
            var skill = new DefaultBuff(args.skill, dActor, element + "Shield", life);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill) {
            var atk1 = skill.skill.Level * 5;
            if (actor.type == ActorType.PC) {
                var pc = (ActorPC)actor;
                if ((pc.Skills2_1.ContainsKey(934) || pc.DualJobSkills.Exists(x => x.ID == 934)) &&
                    element == Elements.Holy) //GU2-1光明之魂
                {
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 934))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 934).Level;


                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(934))
                        mainlv = pc.Skills2_1[934].Level;

                    atk1 += 5 * (Math.Max(duallv, mainlv) - 1);
                }
                else if ((pc.Skills2_2.ContainsKey(935) || pc.DualJobSkills.Exists(x => x.ID == 935)) &&
                         element == Elements.Dark) //GU2-2黑暗之魂
                {
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 935))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 935).Level;


                    var mainlv = 0;
                    if (pc.Skills2_2.ContainsKey(935))
                        mainlv = pc.Skills2_2[935].Level;

                    atk1 += 5 * (Math.Max(duallv, mainlv) - 1);
                }
            }

            if (skill.Variable.ContainsKey("ElementShield"))
                skill.Variable.Remove("ElementShield");
            skill.Variable.Add("ElementShield", atk1);
            actor.Status.elements_skill[element] += atk1;

            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + element + "ElementUp");
            propertyInfo.SetValue(actor.Buff, true, null);
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill) {
            var value = skill.Variable["ElementShield"];
            actor.Status.elements_skill[element] -= (short)value;
            if (actor.Status.Additions.ContainsKey("Amplement"))
                if (element != Elements.Dark && element != Elements.Holy)
                    actor.Status.Additions["Amplement"].AdditionEnd();
            var type = actor.Buff.GetType();
            var propertyInfo = type.GetProperty("Body" + element + "ElementUp");
            propertyInfo.SetValue(actor.Buff, false, null);
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}